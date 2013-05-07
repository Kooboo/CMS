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
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Mysql
{
    public class TextContentDbCommands
    {
        public static DateTime MinSQLServerDate = DateTime.Parse("1753-1-1 12:00:00");
        public MySqlCommand Add(TextContent textContent)
        {
            textContent = textContent.ConvertToUTCTime();

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

            MySqlCommand command = new MySqlCommand();

            command.Parameters.Add(new MySqlParameter("?UUID", textContent.UUID));
            command.Parameters.Add(new MySqlParameter("?Repository", textContent.Repository));
            command.Parameters.Add(new MySqlParameter("?FolderName", textContent.FolderName));
            command.Parameters.Add(new MySqlParameter("?UserKey", textContent.UserKey));
            command.Parameters.Add(new MySqlParameter("?UtcCreationDate", textContent.UtcCreationDate));
            command.Parameters.Add(new MySqlParameter("?UtcLastModificationDate", textContent.UtcLastModificationDate));
            command.Parameters.Add(new MySqlParameter("?Published", textContent.Published));
            command.Parameters.Add(new MySqlParameter("?OriginalUUID", textContent.OriginalUUID));
            command.Parameters.Add(new MySqlParameter("?SchemaName", textContent.SchemaName));
            command.Parameters.Add(new MySqlParameter("?ParentFolder", textContent.ParentFolder));
            command.Parameters.Add(new MySqlParameter("?ParentUUID", textContent.ParentUUID));
            command.Parameters.Add(new MySqlParameter("?UserId", textContent.UserId));
            command.Parameters.Add(new MySqlParameter("?OriginalRepository", textContent.OriginalRepository));
            command.Parameters.Add(new MySqlParameter("?OriginalFolder", textContent.OriginalFolder));
            command.Parameters.Add(new MySqlParameter("?IsLocalized", textContent.IsLocalized));
            command.Parameters.Add(new MySqlParameter("?Sequence", textContent.Sequence));

            foreach (var column in schema.Columns.Where(it => !it.IsSystemField))
            {
                fields.Add(string.Format("{0}", column.Name));
                command.Parameters.Add(CreateParameter(column, textContent));
            }
            string sql = string.Format("INSERT INTO `{0}` ({1}) VALUES({2})", schema.GetTableName(),
                string.Join(",", fields.Select(it => "`" + it + "`").ToArray()),
                string.Join(",", fields.Select(it => "?" + it).ToArray()));
            command.CommandText = sql;

            return command;
        }

        private IDbDataParameter CreateParameter(Column column, TextContent textContent)
        {
            string name = "?" + column.Name;
            object value = textContent[column.Name];
            if (value is DateTime)
            {
                if ((DateTime)value < MinSQLServerDate)
                {
                    value = MinSQLServerDate;
                }
            }
            MySqlParameter parameter = new MySqlParameter(name, value);
            return parameter;         
        }

        public MySqlCommand Update(TextContent textContent)
        {
            textContent = textContent.ConvertToUTCTime();

            var schema = textContent.GetSchema().AsActual();
            List<string> fields = new List<string>() { 
             "Repository", "FolderName", "UserKey", "UtcCreationDate", "UtcLastModificationDate", "Published", "OriginalUUID",
             "SchemaName","ParentFolder", "ParentUUID","UserId","OriginalRepository","OriginalFolder","IsLocalized","Sequence"
            };

            MySqlCommand command = new MySqlCommand();

            command.Parameters.Add(new MySqlParameter("?UUID", textContent.UUID));
            command.Parameters.Add(new MySqlParameter("?Repository", textContent.Repository));
            command.Parameters.Add(new MySqlParameter("?FolderName", textContent.FolderName));
            command.Parameters.Add(new MySqlParameter("?UserKey", textContent.UserKey));
            command.Parameters.Add(new MySqlParameter("?UtcCreationDate", textContent.UtcCreationDate));
            command.Parameters.Add(new MySqlParameter("?UtcLastModificationDate", textContent.UtcLastModificationDate));
            command.Parameters.Add(new MySqlParameter("?Published", textContent.Published));
            command.Parameters.Add(new MySqlParameter("?OriginalUUID", textContent.OriginalUUID));
            command.Parameters.Add(new MySqlParameter("?SchemaName", textContent.SchemaName));
            command.Parameters.Add(new MySqlParameter("?ParentFolder", textContent.ParentFolder));
            command.Parameters.Add(new MySqlParameter("?ParentUUID", textContent.ParentUUID));
            command.Parameters.Add(new MySqlParameter("?UserId", textContent.UserId));
            command.Parameters.Add(new MySqlParameter("?OriginalRepository", textContent.OriginalRepository));
            command.Parameters.Add(new MySqlParameter("?OriginalFolder", textContent.OriginalFolder));
            command.Parameters.Add(new MySqlParameter("?IsLocalized", textContent.IsLocalized));
            command.Parameters.Add(new MySqlParameter("?Sequence", textContent.Sequence));

            foreach (var column in schema.Columns.Where(it => !it.IsSystemField))
            {
                fields.Add(string.Format("{0}", column.Name));
                command.Parameters.Add(CreateParameter(column, textContent));
            }
            string sql = string.Format("UPDATE `{0}` SET {1} WHERE UUID=?UUID", schema.GetTableName()
                , string.Join(",", fields.Select(it => "`" + it + "`" + "=?" + it)));

            command.CommandText = sql;

            return command;
        }

        public MySqlCommand Delete(TextContent textContext)
        {
            var schema = textContext.GetSchema();
            MySqlCommand command = new MySqlCommand();
            command.Parameters.Add(new MySqlParameter("?UUID", textContext.UUID));
            command.CommandText = string.Format("DELETE FROM `{0}` WHERE UUID=?UUID", schema.GetTableName());
            return command;
        }

        public MySqlCommand AddCategory(Repository repository, Category category)
        {
            string sql = string.Format("INSERT INTO `{0}`(UUID,CategoryFolder,CategoryUUID) VALUES(?UUID,?CategoryFolder,?CategoryUUID)"
                , repository.GetCategoryTableName());
            MySqlCommand command = new MySqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(new MySqlParameter("?UUID", category.ContentUUID));
            command.Parameters.Add(new MySqlParameter("?CategoryFolder", category.CategoryFolder));
            command.Parameters.Add(new MySqlParameter("?CategoryUUID", category.CategoryUUID));

            return command;
        }
        public MySqlCommand DeleteCategory(Repository repository, Category category)
        {
            string sql = string.Format("DELETE FROM `{0}` WHERE UUID=?UUID AND CategoryFolder=?CategoryFolder AND CategoryUUID=?CategoryUUID"
                 , repository.GetCategoryTableName());
            MySqlCommand command = new MySqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(new MySqlParameter("?UUID", category.ContentUUID));
            command.Parameters.Add(new MySqlParameter("?CategoryFolder", category.CategoryFolder));
            command.Parameters.Add(new MySqlParameter("?CategoryUUID", category.CategoryUUID));

            return command;
        }

        public MySqlCommand ClearCategories(TextContent textContent)
        {
            string sql = string.Format("DELETE FROM `{0}` WHERE UUID=?UUID "
                 , textContent.GetRepository().GetCategoryTableName());
            MySqlCommand command = new MySqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(new MySqlParameter("?UUID", textContent.UUID));

            return command;
        }
        public MySqlCommand QueryCategories(TextContent textContent)
        {
            string sql = string.Format("SELECT * FROM [{0}] WHERE UUID = ?UUID"
             , textContent.GetRepository().GetCategoryTableName());
            MySqlCommand command = new MySqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(new MySqlParameter("?UUID", textContent.UUID));
            return command;
        }

    }
}
