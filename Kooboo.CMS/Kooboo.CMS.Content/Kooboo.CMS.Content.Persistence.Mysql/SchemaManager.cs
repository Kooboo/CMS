using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using System.IO;
using System.Data.SqlClient;
using Kooboo.Data;
using MySql.Data.MySqlClient;
namespace Kooboo.CMS.Content.Persistence.Mysql
{
    public class SchemaManager
    {
        #region variable
        static string createTable = @"    
CREATE TABLE `{0}` (
	Id int NOT NULL AUTO_INCREMENT PRIMARY KEY, 
	UUID nvarchar(36) NOT NULL ,
	Repository nvarchar(50),
	FolderName nvarchar(50),
	UserKey nvarchar(100),
	UtcCreationDate datetime,
	UtcLastModificationDate datetime,
	Published bit,
	OriginalUUID nvarchar(36),
	SchemaName nvarchar(100),
    ParentFolder nvarchar(256),
	ParentUUID nvarchar(36),
    UserId nvarchar(256),
    OriginalRepository nvarchar(256),
    OriginalFolder nvarchar(256),
    IsLocalized bit,
    Sequence int
	{1}
)DEFAULT CHARSET={2};";
        //static string createIndex = "CREATE INDEX IX_{0}_{1} ON `{0}` ([{1}] {2})";

        static string addColumn = "ALTER TABLE `{0}` ADD `{1}` {2};";
        static string dropColumn = "ALTER TABLE `{0}` DROP COLUMN `{1}`;";
        static string alterColumn = "ALTER TABLE `{0}` ALTER COLUMN `{1}` {2};";

        static string dropTable = @"
    DROP TABLE `{0}`;";
        #endregion

        public static void Add(Schema schema)
        {
            if (CheckTableExists(schema))
            {
                Delete(schema);
            }

            StringBuilder columns = new StringBuilder();

            schema = schema.AsActual();

            foreach (var column in schema.Columns.Where(it => !it.IsSystemField))
            {
                columns.AppendFormat(",`{0}` {1}", column.Name, ColumnTypeDefinition(column));
            }

            string ddl = string.Format(createTable, schema.GetTableName(), columns.ToString(), MysqlSettings.Instance.DEFAULT_CHARSET);

            ExecuteDDL(schema.Repository, ddl);
        }
        private static string ColumnTypeDefinition(Column column)
        {
            switch (column.DataType)
            {
                case DataType.String:
                    return column.Length == 0 ? "MEDIUMTEXT" : string.Format("nvarchar({0})", column.Length);
                case DataType.Int:
                    return "int";
                case DataType.Decimal:
                    return "decimal(10,2)";
                case DataType.DateTime:
                    return "datetime";
                case DataType.Bool:
                    return "bit";
                default:
                    return "nvarchar(256)";
            }
        }

        public static void Update(Schema @new, Schema old)
        {
            if (!CheckTableExists(@new))
            {
                Add(@new);
            }
            else
            {
                if (object.ReferenceEquals(old, @new))
                {
                    return;
                }
                var newSchema = @new;

                var adding = @new.Columns.Where(it => !it.IsSystemField).Where(it => !old.Columns.Any(c => string.Compare(it.Name, c.Name, true) == 0));
                var deleting = old.Columns.Where(it => !it.IsSystemField).Where(it => !newSchema.Columns.Any(c => string.Compare(it.Name, c.Name, true) == 0));
                var updating = newSchema.Columns.Where(it => !it.IsSystemField).Where(it => old.Columns
                    .Any(c => string.Compare(it.Name, c.Name, true) == 0 && (it.DataType != c.DataType || it.Length != c.Length)));

                StringBuilder sb = new StringBuilder();

                foreach (var item in deleting)
                {
                    sb.AppendFormat(dropColumn, newSchema.GetTableName(), item.Name);
                }

                foreach (var item in adding)
                {
                    sb.AppendFormat(addColumn, newSchema.GetTableName(), item.Name, ColumnTypeDefinition(item));
                }
                foreach (var item in updating)
                {
                    sb.AppendFormat(alterColumn, newSchema.GetTableName(), item.Name, ColumnTypeDefinition(item));
                }
                string ddl = sb.ToString();

                ExecuteDDL(@new.Repository, ddl);
            }
        }
        public static void Delete(Schema schema)
        {
            DropTable(schema.Repository, schema.GetTableName());
        }

        public static void InitializeRepositoryDatabase(Repository repository)
        {
            CreateCategoryTable(repository);
            //CreateMediaContentTable(repository);
        }

        private static void CreateCategoryTable(Repository repository)
        {
            string ddl = string.Format(@"
    CREATE TABLE IF NOT EXISTS `{0}` (	
	    `UUID` nvarchar(36) NOT NULL,
	    `CategoryFolder` nvarchar(256),
	    `CategoryUUID` nvarchar(36)
	    )DEFAULT CHARSET={1};", repository.GetCategoryTableName(), MysqlSettings.Instance.DEFAULT_CHARSET);
            ExecuteDDL(repository, ddl);
        }
        //        private static void CreateMediaContentTable(Repository repository)
        //        {
        //            string ddl = string.Format(@"
        //IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = `{0}`)
        //    CREATE TABLE `{0}` (	
        //	    Id int IDENTITY (100,1),
        //	    UUID nvarchar(36) NOT NULL PRIMARY KEY,
        //	    UserKey nvarchar(526),
        //	    UtcCreationDate datetime,
        //	    UtcLastModificationDate datetime,
        //	    Published bit,
        //	    OriginalUUID nvarchar(36),
        //	    FolderName nvarchar(256),
        //	    FileName nvarchar(256),
        //	    VirtualPath nvarchar(256),
        //        UserId nvarchar(256)
        //	    );", repository.GetMediaContentTableName());
        //            ExecuteDDL(repository, ddl);
        //        }

        protected static void ExecuteDDL(Repository repository, string ddl)
        {
            if (!string.IsNullOrEmpty(ddl))
            {
                MysqlHelper.BatchExecuteNonQuery(repository,
                    ddl.Split(';').Where(it => !string.IsNullOrEmpty(it)).Select(it => new MySqlCommand(it, null)).ToArray());
            }
        }
        public static void DropTable(Repository repository, string tableName)
        {
            string ddl = string.Format(dropTable, tableName);
            ExecuteDDL(repository, ddl);
        }

        private static bool CheckTableExists(Schema schema)
        {
            string sql = string.Format(@"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}.{1}'", schema.Repository.Name, schema.GetTableName());

            MySqlCommand command = new MySqlCommand() { CommandText = sql };

            return (long)MysqlHelper.ExecuteScalar(schema.Repository, command) != 0;
        }

    }
}
