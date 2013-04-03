using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.SqlServer
{
    public class TextContentDbCommands
    {
        public static DateTime MinSQLServerDate = DateTime.Parse("1753-1-1 12:00:00");
        public SqlCommand Add(TextContent textContent)
        {
            var schema = textContent.GetSchema().AsActual();
            if (schema == null)
            {
                return null;
            }
            List<string> fields = new List<string>() { 
            "UUID", "Repository", "FolderName", "UserKey", "UtcCreationDate", "UtcLastModificationDate", "Published", "OriginalUUID",
            "SchemaName","ParentFolder", "ParentUUID","UserId","OriginalRepository","OriginalFolder",
            "IsLocalized","Sequence"
            };

            SqlCommand command = new SqlCommand();

            command.Parameters.Add(new SqlParameter("@UUID", textContent.UUID));
            command.Parameters.Add(new SqlParameter("@Repository", textContent.Repository));
            command.Parameters.Add(new SqlParameter("@FolderName", textContent.FolderName));
            command.Parameters.Add(new SqlParameter("@UserKey", textContent.UserKey));
            command.Parameters.Add(new SqlParameter("@UtcCreationDate", textContent.UtcCreationDate));
            command.Parameters.Add(new SqlParameter("@UtcLastModificationDate", textContent.UtcLastModificationDate));
            command.Parameters.Add(new SqlParameter("@Published", textContent.Published));
            command.Parameters.Add(new SqlParameter("@OriginalUUID", textContent.OriginalUUID));
            command.Parameters.Add(new SqlParameter("@SchemaName", textContent.SchemaName));
            command.Parameters.Add(new SqlParameter("@ParentFolder", textContent.ParentFolder));
            command.Parameters.Add(new SqlParameter("@ParentUUID", textContent.ParentUUID));
            command.Parameters.Add(new SqlParameter("@UserId", textContent.UserId));
            command.Parameters.Add(new SqlParameter("@OriginalRepository", textContent.OriginalRepository));
            command.Parameters.Add(new SqlParameter("@OriginalFolder", textContent.OriginalFolder));
            command.Parameters.Add(new SqlParameter("@IsLocalized", textContent.IsLocalized));
            command.Parameters.Add(new SqlParameter("@Sequence", textContent.Sequence));

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
                if ((DateTime)value < MinSQLServerDate)
                {
                    value = MinSQLServerDate;
                }
            }
            SqlParameter parameter = new SqlParameter(name, value);
            return parameter;
            //switch (column.DataType)
            //{
            //    case Kooboo.Form.DataType.String:

            //        break;
            //    case Kooboo.Form.DataType.Int:
            //        break;
            //    case Kooboo.Form.DataType.Decimal:
            //        break;
            //    case Kooboo.Form.DataType.DateTime:
            //        break;
            //    case Kooboo.Form.DataType.Bool:
            //        break;
            //    default:
            //        break;
            //}
        }

        public SqlCommand Update(TextContent textContent)
        {
            var schema = textContent.GetSchema().AsActual();
            List<string> fields = new List<string>() { 
             "Repository", "FolderName", "UserKey", "UtcCreationDate", "UtcLastModificationDate", "Published", "OriginalUUID",
             "SchemaName","ParentFolder", "ParentUUID","UserId","OriginalRepository","OriginalFolder","IsLocalized","Sequence"
            };

            SqlCommand command = new SqlCommand();

            command.Parameters.Add(new SqlParameter("@UUID", textContent.UUID));
            command.Parameters.Add(new SqlParameter("@Repository", textContent.Repository));
            command.Parameters.Add(new SqlParameter("@FolderName", textContent.FolderName));
            command.Parameters.Add(new SqlParameter("@UserKey", textContent.UserKey));
            command.Parameters.Add(new SqlParameter("@UtcCreationDate", textContent.UtcCreationDate));
            command.Parameters.Add(new SqlParameter("@UtcLastModificationDate", textContent.UtcLastModificationDate));
            command.Parameters.Add(new SqlParameter("@Published", textContent.Published));
            command.Parameters.Add(new SqlParameter("@OriginalUUID", textContent.OriginalUUID));
            command.Parameters.Add(new SqlParameter("@SchemaName", textContent.SchemaName));
            command.Parameters.Add(new SqlParameter("@ParentFolder", textContent.ParentFolder));
            command.Parameters.Add(new SqlParameter("@ParentUUID", textContent.ParentUUID));
            command.Parameters.Add(new SqlParameter("@UserId", textContent.UserId));
            command.Parameters.Add(new SqlParameter("@OriginalRepository", textContent.OriginalRepository));
            command.Parameters.Add(new SqlParameter("@OriginalFolder", textContent.OriginalFolder));
            command.Parameters.Add(new SqlParameter("@IsLocalized", textContent.IsLocalized));
            command.Parameters.Add(new SqlParameter("@Sequence", textContent.Sequence));

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

        public SqlCommand Delete(TextContent textContext)
        {
            var schema = textContext.GetSchema();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add(new SqlParameter("@UUID", textContext.UUID));
            command.CommandText = string.Format("DELETE FROM [{0}] WHERE UUID=@UUID", schema.GetTableName());
            return command;
        }

        public SqlCommand AddCategory(Repository repository, Category category)
        {
            string sql = string.Format("INSERT INTO [{0}](UUID,CategoryFolder,CategoryUUID) VALUES(@UUID,@CategoryFolder,@CategoryUUID)"
                , repository.GetCategoryTableName());
            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlParameter("@UUID", category.ContentUUID));
            command.Parameters.Add(new SqlParameter("@CategoryFolder", category.CategoryFolder));
            command.Parameters.Add(new SqlParameter("@CategoryUUID", category.CategoryUUID));

            return command;
        }
        public SqlCommand DeleteCategory(Repository repository, Category category)
        {
            string sql = string.Format("DELETE FROM [{0}] WHERE UUID=@UUID AND CategoryFolder=@CategoryFolder AND CategoryUUID=@CategoryUUID"
                 , repository.GetCategoryTableName());
            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlParameter("@UUID", category.ContentUUID));
            command.Parameters.Add(new SqlParameter("@CategoryFolder", category.CategoryFolder));
            command.Parameters.Add(new SqlParameter("@CategoryUUID", category.CategoryUUID));

            return command;
        }

        public SqlCommand ClearCategories(TextContent textContent)
        {
            string sql = string.Format("DELETE FROM [{0}] WHERE UUID=@UUID "
                 , textContent.GetRepository().GetCategoryTableName());
            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlParameter("@UUID", textContent.UUID));

            return command;
        }

        public SqlCommand QueryCategories(TextContent textContent)
        {
            string sql = string.Format("SELECT * FROM [{0}] WHERE UUID = @UUID"
             , textContent.GetRepository().GetCategoryTableName());
            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlParameter("@UUID", textContent.UUID));
            return command;
        }
    }
}
