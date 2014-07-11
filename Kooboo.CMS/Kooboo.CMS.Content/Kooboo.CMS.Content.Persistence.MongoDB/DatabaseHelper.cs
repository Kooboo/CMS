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
using MongoDB.Driver;
using Kooboo.CMS.Content.Models;
using MongoDB.Bson;

namespace Kooboo.CMS.Content.Persistence.MongoDB
{

    public static class DatabaseHelper
    {
        static MongoClient mongoClient = new MongoClient(DatabaseSettings.Instance.ConnectionString);
        public static MongoServer GetServer()
        {
            MongoServer server = mongoClient.GetServer();
            return server;
        }
        public static MongoDatabase GetDatabase(this Repository repository)
        {
            var server = GetServer();
            var db = server.GetDatabase(repository.GetDatabaseName());
            return db;
        }
        public static void DropDatabase(this Repository repository)
        {
            var server = GetServer();
            server.DropDatabase(repository.GetDatabaseName());
        }
        public static void CreateCateogryIndex(this Repository repository)
        {
            var collection = repository.GetCategoriesCollection();
            try
            {
                collection.EnsureIndex(ModelExtensions.GetCaseInsensitiveFieldName("ContentUUID"), ModelExtensions.GetCaseInsensitiveFieldName("CategoryFolder"), ModelExtensions.GetCaseInsensitiveFieldName("CategoryUUID"));
            }//ignore the index creating exception
            catch (Exception e)
            {
                Kooboo.Common.Logging.Logger.Error(e.Message, e);
            }

        }
        public static MongoCollection<BsonDocument> GetCollection(this Schema schema)
        {
            var database = schema.Repository.GetDatabase();
            MongoCollection<BsonDocument> collection = database.GetCollection(schema.GetSchemaCollectionName());
            return collection;
        }
        public static void CreateIndex(this Schema schema)
        {
            MongoCollection<BsonDocument> collection = schema.GetCollection();
            try
            {
                collection.EnsureIndex(ModelExtensions.GetCaseInsensitiveFieldName("FolderName"), ModelExtensions.GetCaseInsensitiveFieldName("UUID"), ModelExtensions.GetCaseInsensitiveFieldName("Published"));
                collection.EnsureIndex(ModelExtensions.GetCaseInsensitiveFieldName("FolderName"), ModelExtensions.GetCaseInsensitiveFieldName("UserKey"), ModelExtensions.GetCaseInsensitiveFieldName("Published"));
                collection.EnsureIndex(ModelExtensions.GetCaseInsensitiveFieldName("FolderName"), ModelExtensions.GetCaseInsensitiveFieldName("UUID"), ModelExtensions.GetCaseInsensitiveFieldName("Published"));
                collection.EnsureIndex(ModelExtensions.GetCaseInsensitiveFieldName("FolderName"), ModelExtensions.GetCaseInsensitiveFieldName("UserKey"), ModelExtensions.GetCaseInsensitiveFieldName("Published"));
                collection.EnsureIndex(ModelExtensions.GetCaseInsensitiveFieldName("FolderName"), ModelExtensions.GetCaseInsensitiveFieldName("ParentFolder"), ModelExtensions.GetCaseInsensitiveFieldName("ParentUUID"));
            }//ignore the index creating exception
            catch (Exception e)
            {
                Kooboo.Common.Logging.Logger.Error(e.Message, e);
            }
        }
        public static void DropCollection(this Schema schema)
        {
            var database = schema.Repository.GetDatabase();
            database.DropCollection(schema.GetSchemaCollectionName());
        }
        public static MongoCollection<BsonDocument> GetCategoriesCollection(this Repository repository)
        {
            var database = repository.GetDatabase();
            MongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(repository.GetCategoryCollectionName());
            return collection;
        }
    }
}
