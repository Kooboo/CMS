#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence.Default;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Persistence.Sqlce.QueryProcessor;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Content.Persistence.Sqlce
{
    public class SQLCeTransactionUnit : ITransactionUnit
    {
        public static SQLCeTransactionUnit Current
        {
            get
            {
                return CallContext.Current.GetObject<SQLCeTransactionUnit>("TextContent-SQLCeTransactionUnit");
            }
            set
            {
                CallContext.Current.RegisterObject("TextContent-SQLCeTransactionUnit", value);
            }
        }

        public SQLCeTransactionUnit(Repository repository)
        {
            this.repository = repository;
        }
        private Repository repository;
        private IEnumerable<SqlCeCommand> commands = new SqlCeCommand[0];
        private List<Action> postActions = new List<Action>();
        public void RegisterCommand(params SqlCeCommand[] command)
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
            using (var conn = new SqlCeConnection(connectionString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    foreach (var command in commands)
                    {
                        try
                        {
                            SQLCeHelper.ResetParameterNullValue(command);
                            command.Transaction = trans;
                            command.Connection = conn;
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            throw new KoobooException(e.Message + "SQL:" + command.CommandText, e);
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
            commands = new SqlCeCommand[0];
            postActions = new List<Action>();
        }
        public void Dispose()
        {
            Clear();
            Current = null;
        }
    }
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ITextContentProvider), Order = 2)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IContentProvider<TextContent>), Order = 2)]
    public class TextContentProvider : ITextContentProvider
    {
        TextContentDbCommands dbCommands = new TextContentDbCommands();
        #region ITextContentProvider Members

        public void AddCategories(Models.TextContent content, params Models.Category[] categories)
        {
            SQLCeHelper.ExecuteNonQuery(content.GetRepository().GetConnectionString(),
                categories.Select(it => dbCommands.AddCategory(content.GetRepository(), it)).ToArray());
        }

        public void DeleteCategories(Models.TextContent content, params Models.Category[] categories)
        {
            SQLCeHelper.ExecuteNonQuery(content.GetRepository().GetConnectionString(),
                 categories.Select(it => dbCommands.DeleteCategory(content.GetRepository(), it)).ToArray());
        }
        public void ClearCategories(TextContent content)
        {
            SQLCeHelper.ExecuteNonQuery(content.GetRepository().GetConnectionString(), dbCommands.ClearCategoreis(content));
        }
        #endregion

        #region IContentProvider<TextContent> Members

        public void Add(Models.TextContent content)
        {
            content.StoreFiles();

            ((IPersistable)content).OnSaving();
            var command = dbCommands.Add(content);
            if (command != null)
            {
                if (SQLCeTransactionUnit.Current != null)
                {
                    SQLCeTransactionUnit.Current.RegisterCommand(command);
                    SQLCeTransactionUnit.Current.RegisterPostAction(delegate() { ((IPersistable)content).OnSaved(); });
                }
                else
                {
                    SQLCeHelper.ExecuteNonQuery(content.GetRepository().GetConnectionString(), command);
                    ((IPersistable)content).OnSaved();
                }
            }

        }

        public void Update(Models.TextContent @new, Models.TextContent old)
        {
            @new.StoreFiles();

            ((IPersistable)@new).OnSaving();
            var command = dbCommands.Update(@new);
            if (SQLCeTransactionUnit.Current != null)
            {
                SQLCeTransactionUnit.Current.RegisterCommand(command);
                SQLCeTransactionUnit.Current.RegisterPostAction(delegate() { ((IPersistable)@new).OnSaved(); });
            }
            else
            {
                SQLCeHelper.ExecuteNonQuery(@new.GetRepository().GetConnectionString(), command);
                ((IPersistable)@new).OnSaved();
            }
        }

        public void Delete(Models.TextContent content)
        {
            var command = dbCommands.Delete(content);
            if (SQLCeTransactionUnit.Current != null)
            {
                SQLCeTransactionUnit.Current.RegisterCommand(command);
                SQLCeTransactionUnit.Current.RegisterPostAction(delegate() { TextContentFileHelper.DeleteFiles(content); });
            }
            else
            {
                SQLCeHelper.ExecuteNonQuery(content.GetRepository().GetConnectionString(), command);
                TextContentFileHelper.DeleteFiles(content);
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
            var connectionString = schema.Repository.GetConnectionString();
            string sql = string.Format("SELECT * FROM [{0}] ", schema.GetTableName());
            List<IDictionary<string, object>> list = new List<IDictionary<string, object>>();
            SqlCeConnection connection;
            using (var reader = SQLCeHelper.ExecuteReader(connectionString, new SqlCeCommand() { CommandText = sql }, out connection))
            {
                try
                {
                    while (reader.Read())
                    {
                        list.Add(reader.ToContent<TextContent>(new TextContent()));
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public IEnumerable<Category> ExportCategoryData(Repository repository)
        {
            var connectionString = repository.GetConnectionString();
            string sql = string.Format("SELECT UUID,CategoryFolder,CategoryUUID FROM [{0}] ", repository.GetCategoryTableName());
            List<Category> list = new List<Category>();
            SqlCeConnection connection;
            using (var reader = SQLCeHelper.ExecuteReader(connectionString, new SqlCeCommand() { CommandText = sql }, out connection))
            {
                try
                {
                    while (reader.Read())
                    {
                        Category category = new Category();
                        category.ContentUUID = reader.GetString(0);
                        category.CategoryFolder = reader.GetString(1);
                        category.CategoryUUID = reader.GetString(2);
                        list.Add(category);
                    }
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        public void ImportSchemaData(Schema schema, IEnumerable<IDictionary<string, object>> data)
        {
            SQLCeHelper.ExecuteNonQuery(schema.Repository.GetConnectionString(),
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
            SQLCeHelper.ExecuteNonQuery(repository.GetConnectionString(),
               data.Select(it => dbCommands.AddCategory(repository, it)).ToArray());
        }
        #endregion

        #region ExecuteQuery

        public void ExecuteNonQuery(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params KeyValuePair<string, object>[] parameters)
        {
            var connectionString = repository.GetConnectionString();

            var command = new System.Data.SqlServerCe.SqlCeCommand(queryText);
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters.Select(it => new System.Data.SqlServerCe.SqlCeParameter() { ParameterName = it.Key, Value = it.Value }).ToArray());
            }
            command.CommandType = commandType;
            SQLCeHelper.ExecuteNonQuery(connectionString, command);
        }

        public IEnumerable<IDictionary<string, object>> ExecuteQuery(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params KeyValuePair<string, object>[] parameters)
        {
            var connectionString = repository.GetConnectionString();

            var command = new System.Data.SqlServerCe.SqlCeCommand(queryText);
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters.Select(it => new System.Data.SqlServerCe.SqlCeParameter() { ParameterName = it.Key, Value = it.Value }).ToArray());
            }
            command.CommandType = commandType;
            List<IDictionary<string, object>> list = new List<IDictionary<string, object>>();
            SqlCeConnection connection;
            using (var dataReader = SQLCeHelper.ExecuteReader(connectionString, command, out connection))
            {
                try
                {
                    while (dataReader.Read())
                    {
                        TextContent content = new TextContent();
                        dataReader.ToContent(content);
                        list.Add(content);
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

        public object ExecuteScalar(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params KeyValuePair<string, object>[] parameters)
        {
            var connectionString = repository.GetConnectionString();

            var command = new System.Data.SqlServerCe.SqlCeCommand(queryText);
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters.Select(it => new System.Data.SqlServerCe.SqlCeParameter() { ParameterName = it.Key, Value = it.Value }).ToArray());
            }
            command.CommandType = commandType;
            return SQLCeHelper.ExecuteScalar(connectionString, command);
        }
        #endregion

        #region CreateTransaction
        public ITransactionUnit CreateTransaction(Repository repository)
        {
            var unit = new SQLCeTransactionUnit(repository);
            SQLCeTransactionUnit.Current = unit;

            return unit;
        } 
        #endregion

        #region QueryCategories
        public IEnumerable<Category> QueryCategories(TextContent content)
        {
            List<Category> list = new List<Category>();
            SqlCeConnection connection;
            using (var dataReader = SQLCeHelper.ExecuteReader(content.GetRepository().GetConnectionString(),
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
