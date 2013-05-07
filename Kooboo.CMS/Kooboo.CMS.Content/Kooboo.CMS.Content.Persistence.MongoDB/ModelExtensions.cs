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
using Kooboo.CMS.Content.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Builders = MongoDB.Driver.Builders;
namespace Kooboo.CMS.Content.Persistence.MongoDB
{
    public static class ModelExtensions
    {
        public static string GetCategoryCollectionName(this Repository repository)
        {
            return (repository.Name + ".__ContentCategory").ToLower();
        }
        public static string GetDatabaseName(this Repository repository)
        {
            if (DatabaseSettings.Instance.SharingDatabase)
            {
                return "Kooboo_CMS_Sharing_DB";
            }
            return repository.Name.ToLower();
        }
        public static string GetSchemaCollectionName(this Schema schema)
        {
            return (schema.Repository.Name + "." + schema.Name).ToLower();
        }
        public static BsonDocument ToBsonDocument(this TextContent textContent)
        {
            textContent = textContent.ConvertToUTCTime();
            BsonDocument doc = new BsonDocument();
            foreach (var item in textContent)
            {
                doc[item.Key] = BsonHelper.Create(item.Value);
                doc[GetCaseInsensitiveFieldName(item.Key)] = BsonHelper.Create(ToCaseInsensitiveValue(item.Value));
            }
            return doc;
        }
        public static UpdateDocument ToUpdateDocument(this TextContent textContent)
        {
            textContent = textContent.ConvertToUTCTime();
            UpdateDocument doc = new UpdateDocument();
            foreach (var item in textContent)
            {
                doc[item.Key] = BsonHelper.Create(item.Value);
                doc[GetCaseInsensitiveFieldName(item.Key)] = BsonHelper.Create(ToCaseInsensitiveValue(item.Value));
            }
            return doc;
        }
        public static TextContent ToContent(this BsonDocument doc)
        {
            if (doc == null)
            {
                return null;
            }
            TextContent content = new TextContent();
            foreach (var item in doc)
            {
                if (item.Name != "_id" && !IsCaseInsensitiveFieldName(item.Name))
                {
                    content[item.Name] = BsonHelper.GetValue(doc[item.Name]);
                }
            }
            return content.ConvertToLocalTime();
        }

        public static BsonDocument ToBsonDocument(this Category category)
        {
            BsonDocument doc = new BsonDocument();
            doc["CategoryFolder"] = category.CategoryFolder;
            doc[GetCaseInsensitiveFieldName("CategoryFolder")] = ToCaseInsensitiveValue(category.CategoryFolder).ToString();
            doc["CategoryUUID"] = category.CategoryUUID;
            doc[GetCaseInsensitiveFieldName("CategoryUUID")] = ToCaseInsensitiveValue(category.CategoryUUID).ToString();
            doc["ContentUUID"] = category.ContentUUID;
            doc[GetCaseInsensitiveFieldName("ContentUUID")] = ToCaseInsensitiveValue(category.ContentUUID).ToString();
            return doc;
        }
        public static IMongoQuery ToQuery(this Category category)
        {
            return Builders.Query.And(Builders.Query.EQ("CategoryFolder", BsonHelper.Create(category.CategoryFolder)),
                  Builders.Query.EQ("CategoryUUID", BsonHelper.Create(category.CategoryUUID)),
                  Builders.Query.EQ("ContentUUID", BsonHelper.Create(category.ContentUUID)));
        }
        public static Category ToCategory(this BsonDocument doc)
        {
            Category category = new Category();
            category.CategoryFolder = doc["CategoryFolder"].ToString();
            category.CategoryUUID = doc["CategoryUUID"].ToString();
            category.ContentUUID = doc["ContentUUID"].ToString();

            return category;
        }

        public static string GetCaseInsensitiveFieldName(string fieldName)
        {
            return "__Lowered__" + fieldName;
        }
        public static bool IsCaseInsensitiveFieldName(string fieldName)
        {
            return fieldName.StartsWith("__Lowered__");
        }
        public static object ToCaseInsensitiveValue(object value)
        {
            if (value is string)
            {
                return ((string)value).ToLower();
            }
            return value;
        }
    }
}
