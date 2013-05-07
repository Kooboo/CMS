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
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;
using MongoDB.Driver.Builders;
using QueryBuilder = MongoDB.Driver.Builders;
using MongoDB.Bson;
namespace Kooboo.CMS.Content.Persistence.MongoDB.Query
{
    public class MongoDBCategoriesQuery : MongoDBQuery
    {
        public MongoDBCategoriesQuery(IContentQuery<TextContent> contentQuery)
            : base(contentQuery)
        {

        }


        protected override global::MongoDB.Driver.MongoCursor<global::MongoDB.Bson.BsonDocument> Query(MongoDBVisitor visitor)
        {
            var categoriesQuery = (CategoriesQuery)ContentQuery;
            MongoDBQueryTranslator translator = new MongoDBQueryTranslator();


            if (categoriesQuery.CategoryFolder != null)
            {
                visitor.SetQuery(MongoDBHelper.EQIgnoreCase("FolderName", categoriesQuery.CategoryFolder.FullName));
            }

            var subQuery = translator.Translate(categoriesQuery.InnerQuery);
            var contents = ((IEnumerable<TextContent>)subQuery.Execute());
            if (contents.Count() > 0)
            {
                var categoryQueryComplete = MongoDBHelper.EQIgnoreCase("CategoryFolder", categoriesQuery.CategoryFolder.FullName);

                var inCategories = QueryBuilder.Query.In("ContentUUID", contents.Select(it => BsonHelper.Create(it.UUID)).ToArray());

                var relation = categoriesQuery.Repository.GetCategoriesCollection().Find(QueryBuilder.Query.And(categoryQueryComplete, inCategories));

                visitor.SetQuery(QueryBuilder.Query.In("UUID", relation.Select(it => it["CategoryUUID"]).ToArray()));


                return categoriesQuery.CategoryFolder.GetSchema().GetCollection().Find(visitor.MongoQuery);
            }
            else
            {
                return null;
            }
        }
    }
}
