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
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
namespace Kooboo.CMS.Content.Persistence.Sqlce
{
    /// <summary>
    /// 
    /// </summary>
    public class SchemaManager
    {
        #region variable
        static string createTable = @"CREATE TABLE [{0}] (
	Id int IDENTITY (100,1), 
	UUID nvarchar(36) NOT NULL PRIMARY KEY,
	Repository nvarchar(256),
	FolderName nvarchar(256),
	UserKey nvarchar(100),
	UtcCreationDate datetime,
	UtcLastModificationDate datetime,
	Published bit,
	OriginalUUID nvarchar(36),
	SchemaName nvarchar(100),
    ParentFolder nvarchar(256),
	ParentUUID nvarchar(256),
    UserId nvarchar(256),
    OriginalRepository nvarchar(256),
    OriginalFolder nvarchar(256),
    IsLocalized bit,
    Sequence int 
    {1}
);";
        //static string createIndex = "CREATE INDEX IX_{0}_{1} ON [{0}] ([{1}] {2})";

        static string addColumn = "ALTER TABLE [{0}] ADD COLUMN [{1}] {2};";
        static string dropColumn = "ALTER TABLE [{0}] DROP COLUMN [{1}];";
        static string alterColumn = "ALTER TABLE [{0}] ALTER COLUMN [{1}] {2};";

        static string dropTable = "DROP TABLE [{0}];";
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
                columns.AppendFormat(",[{0}] {1}", column.Name, ColumnTypeDefinition(column));
            }

            string ddl = string.Format(createTable, schema.GetTableName(), columns.ToString());

            ExecuteDDL(schema.Repository, ddl);
        }
        private static string ColumnTypeDefinition(Column column)
        {
            switch (column.DataType)
            {
                case DataType.String:
                    if (column.Length == 0)
                    {
                        return "ntext";
                    }
                    else
                        return string.Format("nvarchar({1})", column.Name, column.Length);
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
            string ddl = string.Format(dropTable, schema.GetTableName());
            ExecuteDDL(schema.Repository, ddl);
        }

        public static void InitializeDatabase(Repository repository)
        {
            CreateCategoryTable(repository);
            //CreateMediaContentTable(repository);
        }

        private static void CreateCategoryTable(Repository repository)
        {
            string ddl = string.Format(@"CREATE TABLE [{0}] (	
	UUID nvarchar(36) NOT NULL,
	CategoryFolder nvarchar(256),
	CategoryUUID nvarchar(36)
    );", repository.GetCategoryTableName());
            ExecuteDDL(repository, ddl);
        }
        //        private static void CreateMediaContentTable(Repository repository)
        //        {
        //            string ddl = string.Format(@"CREATE TABLE [{0}] (	
        //    Id int IDENTITY (100,1),
        //	UUID nvarchar(36) NOT NULL PRIMARY KEY,
        //    UserKey nvarchar(526),
        //    UtcCreationDate datetime,
        //	UtcLastModificationDate datetime,
        //	Published bit,
        //	OriginalUUID nvarchar(36),
        //	FolderName nvarchar(256),
        //	FileName nvarchar(256),
        //    VirtualPath nvarchar(256),
        //    UserId nvarchar(256)
        //    );", repository.GetMediaContentTableName());
        //            ExecuteDDL(repository, ddl);
        //        }
        protected static void ExecuteDDL(Repository repository, string ddl)
        {
            if (!string.IsNullOrEmpty(ddl))
            {
                SQLCeHelper.ExecuteNonQuery(repository.GetConnectionString(),
                    ddl.Split(';').Where(it => !string.IsNullOrEmpty(it)).Select(it => new SqlCeCommand(it, null)).ToArray());
            }
        }
        private static bool CheckTableExists(Schema schema)
        {
            string sql = string.Format(@"SELECT count(*)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = '{0}' AND TABLE_TYPE='TABLE'", schema.GetTableName());

            SqlCeCommand command = new SqlCeCommand() { CommandText = sql };

            return (int)SQLCeHelper.ExecuteScalar(schema.Repository.GetConnectionString(), command) != 0;
        }

    }
}
