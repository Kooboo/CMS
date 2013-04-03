using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using System.IO;
using System.Data.SqlClient;

namespace Kooboo.CMS.Content.Persistence.SqlServer
{
    public static class DatabaseHelper
    {
        private static object locker = new object();

        public static void InitializeDatabase(Repository repository)
        {
            //if (!SqlServerSettings.Instance.SharingDatabase)
            //{
            //    if (SqlServerSettings.Instance.GetConnection(repository.Name) == null)
            //    {
            //        lock (locker)
            //        {
            //            if (SqlServerSettings.Instance.GetConnection(repository.Name) == null)
            //            {
            //                var dataFile = GetDatabaseFile(repository);

            //                var connectionString = GetIndividualDatabaseConnectionString(SqlServerSettings.Instance.CreateDatabaseSetting, repository);
            //                if (!File.Exists(dataFile))
            //                {
            //                    CreateDatabase(repository, SqlServerSettings.Instance.CreateDatabaseSetting);
            //                }

            //                SqlServerSettings.Instance.AddConnection(new ConnectionSetting() { Name = repository.Name, ConnectionString = connectionString });

            //                SqlServerSettings.Instance.Save();
            //            }
            //        }
            //    }
            //}
            SchemaManager.InitializeRepositoryDatabase(repository);
        }

        //private static string GetDatabaseFile(Repository repository)
        //{
        //    var dataPath = new DataPath(repository.Name);
        //    var dataFile = Path.Combine(dataPath.PhysicalPath, repository.Name + ".mdf");
        //    return dataFile;
        //}

        //private static string GetIndividualDatabaseConnectionString(string connectionString, Repository repository)
        //{
        //    var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

        //    connectionStringBuilder.InitialCatalog = repository.Name;

        //    return connectionStringBuilder.ConnectionString;
        //}
        //private static void CreateDatabase(Repository repository, string connectionString)
        //{
        //    var dataFile = GetDatabaseFile(repository);
        //    var dir = Path.GetDirectoryName(dataFile);
        //    IO.IOUtility.EnsureDirectoryExists(dir);
        //    //do not specify the file path.
        //    string sql = string.Format(@"CREATE DATABASE [{0}]"
        //        , repository.Name
        //        , Path.GetFileName(dataFile));
        //    SqlCommand command = new SqlCommand() { CommandText = sql };
        //    SQLServerHelper.ExecuteNonQuery(connectionString, command);
        //}
        //        private static void DropDatabase(Repository repository, string connectionString)
        //        {
        //            string sql = string.Format(@"IF EXISTS(SELECT * FROM sys.databases WHERE name = '{0}')
        //                                                DROP DATABASE {0}", repository.Name);

        //            SqlCommand command = new SqlCommand() { CommandText = sql };
        //            SQLServerHelper.ExecuteNonQuery(connectionString, command);
        //        }

        public static void DisposeDatabase(Repository repository)
        {
            SchemaManager.DropTable(repository, repository.GetCategoryTableName());
        }
    }
}