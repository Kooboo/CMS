using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.Sqlce
{
    public class TextContentDbCommands
    {
        public static DateTime MinSQLCeDate = DateTime.Parse("1753-1-1 12:00:00");
        public SqlCeCommand Add(TextContent textContent)
        {
            var schema = textContent.GetSchema().AsActual();
            if (schema == null)
            {
                return null;
            }
            List<string> fields = new List<string>() { 
            "UUID", "Repository", "FolderName", "UserKey", "UtcCreationDate", "UtcLastModificationDate", "Published",
            "OriginalUUID", "SchemaName","ParentFolder","ParentUUID","UserId","OriginalRepository","OriginalFolder",
            "IsLocalized","Sequence"
            };

            SqlCeCommand command = new SqlCeCommand();

            command.Parameters.Add(new SqlCeParameter("@UUID", textContent.UUID));
            command.Parameters.Add(new SqlCeParameter("@Repository", textContent.Repository));
            command.Parameters.Add(new SqlCeParameter("@FolderName", textContent.FolderName));
            command.Parameters.Add(new SqlCeParameter("@UserKey", textContent.UserKey));
            command.Parameters.Add(new SqlCeParameter("@UtcCreationDate", textContent.UtcCreationDate));
            command.Parameters.Add(new SqlCeParameter("@UtcLastModificationDate", textContent.UtcLastModificationDate));
            command.Parameters.Add(new SqlCeParameter("@Published", textContent.Published));
            command.Parameters.Add(new SqlCeParameter("@OriginalUUID", textContent.OriginalUUID));
            command.Parameters.Add(new SqlCeParameter("@SchemaName", textContent.SchemaName));
            command.Parameters.Add(new SqlCeParameter("@ParentFolder", textContent.ParentFolder));
            command.Parameters.Add(new SqlCeParameter("@ParentUUID", textContent.ParentUUID));
            command.Parameters.Add(new SqlCeParameter("@UserId", textContent.UserId));
            command.Parameters.Add(new SqlCeParameter("@OriginalRepository", textContent.OriginalRepository));
            command.Parameters.Add(new SqlCeParameter("@OriginalFolder", textContent.OriginalFolder));
            command.Parameters.Add(new SqlCeParameter("@IsLocalized", textContent.IsLocalized));
            command.Parameters.Add(new SqlCeParameter("@Sequence", textContent.Sequence));

            foreach (var column in schema.Columns.Where(it => !it.IsSystemField))
            {
                fields.Add(string.Format("{0}", column.Name));
                command.Parameters.Add(CreateParameter(column, textContent));
            }
            string sql = string.Format("INSERT INTO [{0}] ({1}) VALUES({2})", schema.GetTableName(),
                string.Join(",", fields.Select(it => "[" + it + "]").ToArray()),
                string.Join(",", fields.Select(it => "@" + it).ToArray()));
            command.CommandText = sql;

            return command;
        }

        private IDbDataParameter CreateParameter(Column column, TextContent textContent)
        {
            string name = "@" + column.Name;
            object value = textContent[column.Name];
            if (value is DateTime)
            {
                if ((DateTime)value < MinSQLCeDate)
                {
                    value = MinSQLCeDate;
                }
            }
            SqlCeParameter parameter = new SqlCeParameter(name, value);
            return parameter;
            //switch (column.DataType)
            //{
            //    case DataType.String:

            //        break;
            //    case DataType.Int:
            //        break;
            //    case DataType.Decimal:
            //        break;
            //    case DataType.DateTime:
            //        break;
            //    case DataType.Bool:
            //        break;
            //    default:
            //        break;
            //}
        }

        public SqlCeCommand Update(TextContent textContent)
        {
            var schema = textContent.GetSchema().AsActual();
            List<string> fields = new List<string>() { 
             "Repository", "FolderName", "UserKey", "UtcCreationDate", "UtcLastModificationDate", "Published", 
             "OriginalUUID", "SchemaName","ParentFolder", "ParentUUID","UserId","OriginalRepository","OriginalFolder",
             "IsLocalized","Sequence"
            };

            SqlCeCommand command = new SqlCeCommand();

            command.Parameters.Add(new SqlCeParameter("@UUID", textContent.UUID));
            command.Parameters.Add(new SqlCeParameter("@Repository", textContent.Repository));
            command.Parameters.Add(new SqlCeParameter("@FolderName", textContent.FolderName));
            command.Parameters.Add(new SqlCeParameter("@UserKey", textContent.UserKey));
            command.Parameters.Add(new SqlCeParameter("@UtcCreationDate", textContent.UtcCreationDate));
            command.Parameters.Add(new SqlCeParameter("@UtcLastModificationDate", textContent.UtcLastModificationDate));
            command.Parameters.Add(new SqlCeParameter("@Published", textContent.Published));
            command.Parameters.Add(new SqlCeParameter("@OriginalUUID", textContent.OriginalUUID));
            command.Parameters.Add(new SqlCeParameter("@SchemaName", textContent.SchemaName));
            command.Parameters.Add(new SqlCeParameter("@ParentFolder", textContent.ParentFolder));
            command.Parameters.Add(new SqlCeParameter("@ParentUUID", textContent.ParentUUID));
            command.Parameters.Add(new SqlCeParameter("@UserId", textContent.UserId));
            command.Parameters.Add(new SqlCeParameter("@OriginalRepository", textContent.OriginalRepository));
            command.Parameters.Add(new SqlCeParameter("@OriginalFolder", textContent.OriginalFolder));
            command.Parameters.Add(new SqlCeParameter("@IsLocalized", textContent.IsLocalized));
            command.Parameters.Add(new SqlCeParameter("@Sequence", textContent.Sequence));

            foreach (var column in schema.Columns.Where(it => !it.IsSystemField))
            {
                fields.Add(string.Format("{0}", column.Name));
                command.Parameters.Add(CreateParameter(column, textContent));
            }
            string sql = string.Format("UPDATE [{0}] SET {1} WHERE UUID=@UUID", schema.GetTableName()
                , string.Join(",", fields.Select(it => "[" + it + "]" + "=@" + it)));

            command.CommandText = sql;

            return command;
        }

        public SqlCeCommand Delete(TextContent textContext)
        {
            var schema = textContext.GetSchema();
            SqlCeCommand command = new SqlCeCommand();
            command.Parameters.Add(new SqlCeParameter("@UUID", textContext.UUID));
            command.CommandText = string.Format("DELETE FROM [{0}] WHERE UUID=@UUID", schema.GetTableName());
            return command;
        }

        public SqlCeCommand AddCategory(Repository repository, Category category)
        {
            string sql = string.Format("INSERT INTO [{0}](UUID,CategoryFolder,CategoryUUID) VALUES(@UUID,@CategoryFolder,@CategoryUUID)"
                 , repository.GetCategoryTableName());
            SqlCeCommand command = new SqlCeCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlCeParameter("@UUID", category.ContentUUID));
            command.Parameters.Add(new SqlCeParameter("@CategoryFolder", category.CategoryFolder));
            command.Parameters.Add(new SqlCeParameter("@CategoryUUID", category.CategoryUUID));

            return command;
        }
        public SqlCeCommand DeleteCategory(Repository repository, Category category)
        {
            string sql = string.Format("DELETE FROM [{0}] WHERE UUID=@UUID AND CategoryFolder=@CategoryFolder AND CategoryUUID=@CategoryUUID"
                 , repository.GetCategoryTableName());
            SqlCeCommand command = new SqlCeCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlCeParameter("@UUID", category.ContentUUID));
            command.Parameters.Add(new SqlCeParameter("@CategoryFolder", category.CategoryFolder));
            command.Parameters.Add(new SqlCeParameter("@CategoryUUID", category.CategoryUUID));

            return command;
        }
        public SqlCeCommand ClearCategoreis(TextContent textContent)
        {
            string sql = string.Format("DELETE FROM [{0}] WHERE UUID=@UUID"
                 , textContent.GetRepository().GetCategoryTableName());
            SqlCeCommand command = new SqlCeCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlCeParameter("@UUID", textContent.UUID));
            return command;
        }

        public SqlCeCommand QueryCategories(TextContent textContent)
        {
            string sql = string.Format("SELECT * FROM [{0}] WHERE UUID = @UUID"
             , textContent.GetRepository().GetCategoryTableName());
            SqlCeCommand command = new SqlCeCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlCeParameter("@UUID", textContent.UUID));
            return command;
        }
    }
}
