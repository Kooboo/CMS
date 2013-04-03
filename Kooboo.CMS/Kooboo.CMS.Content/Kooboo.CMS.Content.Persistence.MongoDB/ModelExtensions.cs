using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using MongoDB.Bson;
using MongoDB.Driver;

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
        public static BsonDocument ToBsonDocument(this TextContent content)
        {
            BsonDocument doc = new BsonDocument();
            foreach (var item in content)
            {
                doc[item.Key] = BsonHelper.Create(item.Value);
            }
            return doc;
        }
        public static UpdateDocument ToUpdateDocument(this TextContent content)
        {
            UpdateDocument doc = new UpdateDocument();
            foreach (var item in content)
            {
                doc[item.Key] = BsonHelper.Create(item.Value);
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
                if (item.Name != "_id")
                {
                    content[item.Name] = BsonHelper.GetValue(doc[item.Name]);
                }
            }
            return content;
        }

        public static BsonDocument ToBsonDocument(this Category category)
        {
            BsonDocument doc = new BsonDocument();
            doc["CategoryFolder"] = category.CategoryFolder;
            doc["CategoryUUID"] = category.CategoryUUID;
            doc["ContentUUID"] = category.ContentUUID;
            return doc;
        }
        public static Category ToCategory(this BsonDocument doc)
        {
            Category category = new Category();
            category.CategoryFolder = doc["CategoryFolder"].ToString();
            category.CategoryUUID = doc["CategoryUUID"].ToString();
            category.ContentUUID = doc["ContentUUID"].ToString();

            return category;
        }
    }
}
