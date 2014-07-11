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
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDBQuery = MongoDB.Driver.Builders;
using Kooboo.CMS.Content.Persistence.Default;
using MongoDB.Driver.Builders;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Content.Persistence.MongoDB
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ITextContentProvider), Order = 2)]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IContentProvider<TextContent>), Order = 2)]
    public class TextContentProvider : ITextContentProvider
    {
        #region Categories
        public void ClearCategories(Models.TextContent content)
        {
            //((IPersistable)content).OnSaving();
            MongoCollection<BsonDocument> collection = content.GetRepository().GetCategoriesCollection();
            var query = new QueryDocument { { "ContentUUID", content.UUID } };
            collection.Remove(query);
            //((IPersistable)content).OnSaved();
        }

        public void AddCategories(Models.TextContent content, params Models.Category[] categories)
        {
            //((IPersistable)content).OnSaving();
            if (categories != null && categories.Length > 0)
            {
                MongoCollection<BsonDocument> collection = content.GetRepository().GetCategoriesCollection();
                collection.InsertBatch(categories.Select(it => it.ToBsonDocument()).ToArray());
            }

            //((IPersistable)content).OnSaved();
        }

        public void DeleteCategories(Models.TextContent content, params Models.Category[] categories)
        {
            //((IPersistable)content).OnSaving();
            if (categories != null && categories.Length > 0)
            {
                MongoCollection<BsonDocument> collection = content.GetRepository().GetCategoriesCollection();
                var query = MongoDBQuery.Query.Or(categories.Select(it => it.ToQuery()).ToArray());
                collection.Remove(query);
            }
            //((IPersistable)content).OnSaved();
        }
        #endregion

        #region Import/export
        public IEnumerable<IDictionary<string, object>> ExportSchemaData(Models.Schema schema)
        {
            var collection = schema.GetCollection();
            return collection.FindAll().Select(it => it.ToContent());
        }

        public IEnumerable<Models.Category> ExportCategoryData(Models.Repository repository)
        {
            var collection = repository.GetCategoriesCollection();
            return collection.FindAll().Select(it => it.ToCategory());
        }

        public void ImportSchemaData(Models.Schema schema, IEnumerable<IDictionary<string, object>> data)
        {
            var dataCollection = schema.GetCollection();
            var docs = data.Select(it => (new TextContent(it) { Repository = schema.Repository.Name }).ToBsonDocument()).ToArray();
            if (docs.Length > 0)
            {
                dataCollection.InsertBatch(docs);
            }
        }

        public void ImportCategoryData(Models.Repository repository, IEnumerable<Models.Category> data)
        {
            var dataCollection = repository.GetCategoriesCollection();
            var docs = data.Select(it => it.ToBsonDocument()).ToArray();
            if (docs.Length > 0)
            {
                dataCollection.InsertBatch(docs);
            }
        }
        #endregion

        #region insert/update/delete

        public void Add(Models.TextContent content)
        {
            content.Remove("_id");

            content.StoreFiles();

            ((IPersistable)content).OnSaving();
            var database = content.GetRepository().GetDatabase();
            MongoCollection<BsonDocument> collection = database.GetCollection(content.GetSchema().GetSchemaCollectionName());
            collection.Insert(content.ToBsonDocument());
            ((IPersistable)content).OnSaved();
        }

        public void Update(Models.TextContent @new, Models.TextContent old)
        {
            if (@new["_id"] == null && old["_id"] != null)
            {
                @new["_id"] = old["_id"];
            }

            @new.StoreFiles();
            ((IPersistable)@new).OnSaving();
            var database = old.GetRepository().GetDatabase();
            MongoCollection<BsonDocument> collection = database.GetCollection(old.GetSchema().GetSchemaCollectionName());
            var query = new QueryDocument { { "UUID", old.UUID } };
            collection.Update(query, @new.ToUpdateDocument());
            ((IPersistable)@new).OnSaved();
        }

        public void Delete(Models.TextContent content)
        {
            var database = content.GetRepository().GetDatabase();
            MongoCollection<BsonDocument> collection = database.GetCollection(content.GetSchema().GetSchemaCollectionName());
            var query = new QueryDocument { { "UUID", content.UUID } };
            collection.Remove(query);
            TextContentFileHelper.DeleteFiles(content);
        }
        #endregion

        public object Execute(IContentQuery<Models.TextContent> query)
        {
            Query.MongoDBQueryTranslator translator = new Query.MongoDBQueryTranslator();
            return translator.Translate(query).Execute();
        }


        #region Execute query
        public void ExecuteNonQuery(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params  KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Not supported for mongoDB");
        }

        public IEnumerable<IDictionary<string, object>> ExecuteQuery(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params  KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Not supported for mongoDB");
        }

        public object ExecuteScalar(Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params  KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Not supported for mongoDB");
        }
        #endregion

        #region CreateTransaction
        public ITransactionUnit CreateTransaction(Repository repository)
        {
            return new EmptyTransactionUnit();
        }
        #endregion

        #region QueryCategories
        public IEnumerable<Category> QueryCategories(TextContent content)
        {
            MongoCollection<BsonDocument> collection = content.GetRepository().GetCategoriesCollection();
            var query = MongoDBHelper.EQIgnoreCase("ContentUUID", content.UUID);
            var documents = collection.Find(query);
            List<Category> list = new List<Category>();

            foreach (var doc in documents)
            {
                Category category = new Category()
                {
                    CategoryFolder = doc["CategoryFolder"].AsString,
                    CategoryUUID = doc["CategoryUUID"].AsString,
                    ContentUUID = doc["ContentUUID"].AsString
                };

                list.Add(category);
            }

            return list;
        }
        #endregion
    }
}
