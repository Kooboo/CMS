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
        public static MongoServer Connect()
        {
            MongoServer server = new MongoClient(DatabaseSettings.Instance.ConnectionString).GetServer();
            server.Connect();
            return server;
        }
        public static MongoDatabase GetDatabase(this Repository repository)
        {
            var server = Connect();
            var db = server.GetDatabase(repository.GetDatabaseName());
            return db;
        }
        public static void DropDatabase(this Repository repository)
        {
            var server = Connect();
            server.Connect();
            server.DropDatabase(repository.GetDatabaseName());
            server.Disconnect();
        }
        public static void CreateCateogryIndex(this Repository repository)
        {
            var collection = repository.GetCategoriesCollection();
            collection.CreateIndex("ContentUUID", "CategoryFolder", "CategoryUUID");
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
            collection.CreateIndex("FolderName", "UUID", "Published", "UtcCreationDate");
            collection.CreateIndex("FolderName", "UserKey", "Published", "UtcCreationDate");
            collection.CreateIndex("FolderName", "UUID", "Published", "Sequence");
            collection.CreateIndex("FolderName", "UserKey", "Published", "Sequence");
            collection.CreateIndex("FolderName", "ParentFolder", "ParentUUID", "Published", "UtcCreationDate");
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
