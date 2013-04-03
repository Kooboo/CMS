using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence.Default;
using System.Diagnostics;

namespace Kooboo.CMS.Content.Persistence.SqlServer
{
    public class SQLServerTransactionUnit : ITransactionUnit
    {
        public static SQLServerTransactionUnit Current
        {
            get
            {
                return CallContext.Current.GetObject<SQLServerTransactionUnit>("TextContent-SQLServerTransactionUnit");
            }
            set
            {
                CallContext.Current.RegisterObject("TextContent-SQLServerTransactionUnit", value);
            }
        }

        public SQLServerTransactionUnit(Repository repository)
        {
            this.repository = repository;
        }
        private Repository repository;
        private IEnumerable<SqlCommand> commands = new SqlCommand[0];
        private List<Action> postActions = new List<Action>();
        public void RegisterCommand(params SqlCommand[] command)
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
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    foreach (var command in commands)
                    {
                        SQLServerHelper.LogCommand(repository, command);
                        try
                        {
                            SQLServerHelper.ResetParameterNullValue(command);
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
            commands = new SqlCommand[0];
            postActions = new List<Action>();
        }
        public void Dispose()
        {
            Clear();
            Current = null;
        }
    }
    public class TextContentProvider : ITextContentProvider
    {
        TextContentDbCommands dbCommands = new TextContentDbCommands();
        #region ITextContentProvider Members

        public void AddCategories(Models.TextContent content, params Models.Category[] categories)
        {
            SQLServerHelper.BatchExecuteNonQuery(content.GetRepository(),
                categories.Select(it => dbCommands.AddCategory(content.GetRepository(), it)).ToArray());
        }

        public void DeleteCategories(Models.TextContent content, params Models.Category[] categories)
        {
            SQLServerHelper.BatchExecuteNonQuery(content.GetRepository(),
                 categories.Select(it => dbCommands.DeleteCategory(content.GetRepository(), it)).ToArray());
        }
        public void ClearCategories(TextContent content)
        {
            SQLServerHelper.BatchExecuteNonQuery(content.GetRepository(),
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
                    if (SQLServerTransactionUnit.Current != null)
                    {
                        SQLServerTransactionUnit.Current.RegisterCommand(command);
                        SQLServerTransactionUnit.Current.RegisterPostAction(delegate() { ((IPersistable)content).OnSaved(); });
                    }
                    else
                    {
                        SQLServerHelper.BatchExecuteNonQuery(content.GetRepository(), command);
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
            if (SQLServerTransactionUnit.Current != null)
            {
                SQLServerTransactionUnit.Current.RegisterCommand(command);
                SQLServerTransactionUnit.Current.RegisterPostAction(delegate() { ((IPersistable)@new).OnSaved(); });
            }
            else
            {
                SQLServerHelper.BatchExecuteNonQuery(@new.GetRepository(), command);
                ((IPersistable)@new).OnSaved();
            }

        }

        public void Delete(Models.TextContent content)
        {
            var command = dbCommands.Delete(content);
            if (SQLServerTransactionUnit.Current != null)
            {
                SQLServerTransactionUnit.Current.RegisterCommand(command);
                SQLServerTransactionUnit.Current.RegisterPostAction(delegate() { TextContentFileHelper.DeleteFiles(content); });
            }
            else
            {
                SQLServerHelper.BatchExecuteNonQuery(content.GetRepository(), command);
                TextContentFileHelper.DeleteFiles(content);
            }

        }

        public object Execute(Query.IContentQuery<Models.TextContent> query)
        {

            var translator = new QueryProcessor.TextContentTranslator();
            var executor = translator.Translate(query);
            var result = executor.Execute();

            return result;
        }

        #endregion


        #region Import/Export
        public IEnumerable<IDictionary<string, object>> ExportSchemaData(Schema schema)
        {
            string sql = string.Format("SELECT * FROM [{0}] ", schema.GetTableName());
            List<IDictionary<string, object>> list = new List<IDictionary<string, object>>();
            SqlConnection connection;
            using (var reader = SQLServerHelper.ExecuteReader(schema.Repository, new SqlCommand() { CommandText = sql }, out connection))
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
            string sql = string.Format("SELECT UUID,CategoryFolder,CategoryUUID FROM [{0}] ", repository.GetCategoryTableName());
            List<Category> list = new List<Category>();
            SqlConnection connection;
            using (var reader = SQLServerHelper.ExecuteReader(repository, new SqlCommand() { CommandText = sql }, out connection))
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
            SQLServerHelper.ExecuteNonQuery(schema.Repository,
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
            SQLServerHelper.ExecuteNonQuery(repository,
               data.Select(it => dbCommands.AddCategory(repository, it)).ToArray());
        }
        #endregion

        #region ExecuteQuery

        public void ExecuteNonQuery(Repository repository, string queryText, params  KeyValuePair<string, object>[] parameters)
        {
            var command = new System.Data.SqlClient.SqlCommand(queryText);
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters.Select(it => new SqlParameter() { ParameterName = it.Key, Value = it.Value }).ToArray());
            }

            SQLServerHelper.ExecuteNonQuery(repository, command);
        }

        public IEnumerable<IDictionary<string, object>> ExecuteQuery(Repository repository, string queryText, params  KeyValuePair<string, object>[] parameters)
        {
            var command = new System.Data.SqlClient.SqlCommand(queryText);
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters.Select(it => new SqlParameter() { ParameterName = it.Key, Value = it.Value }).ToArray());
            }
            List<IDictionary<string, object>> list = new List<IDictionary<string, object>>();
            SqlConnection connection;
            using (var dataReader = SQLServerHelper.ExecuteReader(repository, command, out connection))
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

        public object ExecuteScalar(Repository repository, string queryText, params  KeyValuePair<string, object>[] parameters)
        {
            var command = new System.Data.SqlClient.SqlCommand(queryText);
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters.Select(it => new SqlParameter() { ParameterName = it.Key, Value = it.Value }).ToArray());
            }

            return SQLServerHelper.ExecuteScalar(repository, command);
        }
        #endregion

        public ITransactionUnit CreateTransaction(Repository repository)
        {
            var unit = new SQLServerTransactionUnit(repository);

            SQLServerTransactionUnit.Current = unit;

            return unit;
        }


        public IEnumerable<Category> QueryCategories(TextContent content)
        {
            List<Category> list = new List<Category>();
            SqlConnection connection;
            using (var dataReader = SQLServerHelper.ExecuteReader(content.GetRepository(),
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
    }
}
