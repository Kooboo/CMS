#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence.Default;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Mysql
{
    public class MySQLTransactionUnit : ITransactionUnit
    {
        public static MySQLTransactionUnit Current
        {
            get
            {
                return Kooboo.CMS.Common.ContextVariables.Current.GetObject<MySQLTransactionUnit>("TextContent-MySQLTransactionUnit");
            }
            set
            {
                Kooboo.CMS.Common.ContextVariables.Current.SetObject("TextContent-MySQLTransactionUnit", value);
            }
        }

        public MySQLTransactionUnit(Repository repository)
        {
            this.repository = repository;
        }
        private Repository repository;
        private IEnumerable<MySqlCommand> commands = new MySqlCommand[0];
        private List<Action> postActions = new List<Action>();
        public void RegisterCommand(params MySqlCommand[] command)
        {
            commands = commands.Concat(command);
        }
        public void RegisterPostAction(Action action)
        {
            postActions.Add(action);
        }
        public void Rollback()
        {
            Clear();
        }

        public void Commit()
        {
            var connectionString = repository.GetConnectionString();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    foreach (var command in commands)
                    {
                        MysqlHelper.LogCommand(repository, command);
                        try
                        {
                            MysqlHelper.ResetParameterNullValue(command);
                            command.Transaction = trans;
                            command.Connection = conn;
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.Message + "SQL:" + command.CommandText, e);
                        }
                    }

                    trans.Commit();
                }
            }
            //Execute post content events
            foreach (var action in postActions)
            {
                action();
            }
            Clear();
        }

        private void Clear()
        {
            commands = new MySqlCommand[0];
            postActions = new List<Action>();
        }
        public void Dispose()
        {
            Clear();
            Current = null;
        }
    }
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ITextContentProvider), Order = 2)]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IContentProvider<TextContent>), Order = 2)]
    public class TextContentProvider : ITextContentProvider
    {
        TextContentDbCommands dbCommands = new TextContentDbCommands();
        #region ITextContentProvider Members

        public void AddCategories(Models.TextContent content, params Models.Category[] categories)
        {
            MysqlHelper.BatchExecuteNonQuery(content.GetRepository(),
                categories.Select(it => dbCommands.AddCategory(content.GetRepository(), it)).ToArray());
        }

        public void DeleteCategories(Models.TextContent content, params Models.Category[] categories)
        {
            MysqlHelper.BatchExecuteNonQuery(content.GetRepository(),
                 categories.Select(it => dbCommands.DeleteCategory(content.GetRepository(), it)).ToArray());
        }
        public void ClearCategories(TextContent content)
        {
            MysqlHelper.BatchExecuteNonQuery(content.GetRepository(),
                dbCommands.ClearCategories(content));
        }

        #endregion

        #region IContentProvider<TextContent> Members

        public void Add(Models.TextContent content)
        {
            try
            {
                content.StoreFiles();

                ((IPersistable)content).OnSaving();
                var command = dbCommands.Add(content);
                if (command != null)
                {
                    if (MySQLTransactionUnit.Current != null)
                    {
                        MySQLTransactionUnit.Current.RegisterCommand(command);
                        MySQLTransactionUnit.Current.RegisterPostAction(delegate() { ((IPersistable)content).OnSaved(); });
                    }
                    else
                    {
                        MysqlHelper.BatchExecuteNonQuery(content.GetRepository(), command);
                        ((IPersistable)content).OnSaved();
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void Update(Models.TextContent @new, Models.TextContent old)
        {
            @new.StoreFiles();

            ((IPersistable)@new).OnSaving();
            var command = dbCommands.Update(@new);
            if (MySQLTransactionUnit.Current != null)
            {
                MySQLTransactionUnit.Current.RegisterCommand(command);
                MySQLTransactionUnit.Current.RegisterPostAction(delegate() { ((IPersistable)@new).OnSaved(); });
            }
            else
            {
                MysqlHelper.BatchExecuteNonQuery(@new.GetRepository(), command);
                ((IPersistable)@new).OnSaved();
            }
        }

        public void Delete(Models.TextContent content)
        {
            var command = dbCommands.Delete(content);
            if (MySQLTransactionUnit.Current != null)
            {
                MySQLTransactionUnit.Current.RegisterCommand(command);
                MySQLTransactionUnit.Current.RegisterPostAction(delegate() { ((IPersistable)content).OnSaved(); });
            }
            else
            {
                MysqlHelper.BatchExecuteNonQuery(content.GetRepository(), command);
            }
        }

        public object Execute(Query.IContentQuery<Models.TextContent> query)
        {
            var translator = new QueryProcessor.TextContentTranslator();
            var executor = translator.Translate(query);
            return executor.Execute();
        }

        #endregion


        #region Import/Export
        public IEnumerable<IDictionary<string, object>> ExportSchemaData(Schema schema)
        {
            string sql = string.Format("SELECT * FROM `{0}` ", schema.GetTableName());
            List<TextContent> list = new List<TextContent>();
            MySqlConnection connection;
            using (var reader = MysqlHelper.ExecuteReader(schema.Repository, new MySqlCommand() { CommandText = sql }, out connection))
            {
                while (reader.Read())
                {
                    list.Add(reader.ToContent<TextContent>(new TextContent()));
                }
                connection.Close();
            }
            return list;
        }

        public IEnumerable<Category> ExportCategoryData(Repository repository)
        {
            string sql = string.Format("SELECT UUID,CategoryFolder,CategoryUUID FROM `{0}` ", repository.GetCategoryTableName());
            List<Category> list = new List<Category>();
            MySqlConnection connection;
            using (var reader = MysqlHelper.ExecuteReader(repository, new MySqlCommand() { CommandText = sql }, out connection))
            {
                while (reader.Read())
                {
                    Category category = new Category();
                    category.ContentUUID = reader.GetString(0);
                    category.CategoryFolder = reader.GetString(1);
                    category.CategoryUUID = reader.GetString(2);
                    list.Add(category);
                }
                connection.Close();
            }
            return list;
        }

        public void ImportSchemaData(Schema schema, IEnumerable<IDictionary<string, object>> data)
        {
            MysqlHelper.ExecuteNonQuery(schema.Repository,
             data.Select(it => dbCommands.Add(GetContent(schema, it))).Where(it => it != null).ToArray());

        }
        private static TextContent GetContent(Schema schema, IDictionary<string, object> item)
        {
            var content = new TextContent(item);
            content.Repository = schema.Repository.Name;
            return content;
        }

        public void ImportCategoryData(Repository repository, IEnumerable<Category> data)
        {
            MysqlHelper.ExecuteNonQuery(repository,
               data.Select(it => dbCommands.AddCategory(repository, it)).ToArray());
        }
        #endregion


        #region ExecuteQuery

        public void ExecuteNonQuery(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params KeyValuePair<string, object>[] parameters)
        {
            var command = new MySql.Data.MySqlClient.MySqlCommand(queryText);
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters.Select(it => new MySql.Data.MySqlClient.MySqlParameter() { ParameterName = it.Key, Value = it.Value }).ToArray());
            }
            command.CommandType = commandType;
            Mysql.MysqlHelper.ExecuteNonQuery(repository, command);
        }

        public IEnumerable<IDictionary<string, object>> ExecuteQuery(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params KeyValuePair<string, object>[] parameters)
        {
            var command = new MySql.Data.MySqlClient.MySqlCommand(queryText);
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters.Select(it => new MySql.Data.MySqlClient.MySqlParameter() { ParameterName = it.Key, Value = it.Value }).ToArray());
            }
            command.CommandType = commandType;
            MySqlConnection connection;
            using (var dataReader = Mysql.MysqlHelper.ExecuteReader(repository, command, out connection))
            {
                while (dataReader.Read())
                {
                    TextContent content = new TextContent();
                    dataReader.ToContent(content);
                    yield return content;
                }

                dataReader.Close();
                connection.Close();
            }
        }

        public object ExecuteScalar(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params KeyValuePair<string, object>[] parameters)
        {
            var command = new MySql.Data.MySqlClient.MySqlCommand(queryText);
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters.Select(it => new MySql.Data.MySqlClient.MySqlParameter() { ParameterName = it.Key, Value = it.Value }).ToArray());
            }
            command.CommandType = commandType;
            return Mysql.MysqlHelper.ExecuteScalar(repository, command);
        }
        #endregion

        #region CreateTransaction
        public ITransactionUnit CreateTransaction(Repository repository)
        {
            var unit = new MySQLTransactionUnit(repository);
            MySQLTransactionUnit.Current = unit;
            return unit;
        }
        #endregion

        #region QueryCategories
        public IEnumerable<Category> QueryCategories(TextContent content)
        {
            List<Category> list = new List<Category>();
            MySqlConnection connection;
            using (var dataReader = MysqlHelper.ExecuteReader(content.GetRepository(),
                dbCommands.QueryCategories(content), out connection))
            {
                try
                {
                    while (dataReader.Read())
                    {
                        Category category = new Category()
                        {
                            CategoryFolder = dataReader.GetString(dataReader.GetOrdinal("CategoryFolder")),
                            CategoryUUID = dataReader.GetString(dataReader.GetOrdinal("CategoryUUID")),
                            ContentUUID = dataReader.GetString(dataReader.GetOrdinal("UUID")),
                        };
                        list.Add(category);
                    }

                }
                finally
                {
                    dataReader.Close();
                    connection.Close();
                }
            }
            return list;
        }
        #endregion
    }
}
