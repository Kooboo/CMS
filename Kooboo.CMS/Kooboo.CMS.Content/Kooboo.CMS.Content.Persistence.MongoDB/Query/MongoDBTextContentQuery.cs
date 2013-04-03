using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Query;
using MongoDB.Driver.Builders;
using QueryBuilder = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.MongoDB.Query
{
    public class MongoDBTextContentQuery : MongoDBQuery
    {
        public MongoDBTextContentQuery(IContentQuery<TextContent> contentQuery)
            : base(contentQuery)
        {

        }
        protected override global::MongoDB.Driver.MongoCursor<global::MongoDB.Bson.BsonDocument> Query(MongoDBVisitor visitor)
        {
            var textContentQuery = (TextContentQuery)ContentQuery;

            if (textContentQuery.Folder != null)
            {
                visitor.SetQuery(MongoDBHelper.EQIgnoreCase("FolderName", textContentQuery.Folder.FullName));
            }

            if (visitor.CategoryQueries != null && visitor.CategoryQueries.Count() > 0)
            {
                MongoDBQueryTranslator translator = new MongoDBQueryTranslator();

                IEnumerable<BsonDocument> categoryContents = null;

                foreach (var categoryQuery in visitor.CategoryQueries)
                {
                    var categories = ((IEnumerable<TextContent>)translator.Translate(categoryQuery).Execute()).ToArray();

                    if (categories.Count() > 0)
                    {
                        var categoryQueryComplete = MongoDBHelper.EQIgnoreCase("CategoryFolder", ((TextContentQueryBase)categoryQuery).Folder.FullName);
                        var inCategories = QueryBuilder.Query.In("CategoryUUID", categories.Select(it => BsonHelper.Create(it.UUID)).ToArray());
                        var relation = textContentQuery.Repository.GetCategoriesCollection().Find(QueryBuilder.Query.And(categoryQueryComplete, inCategories));

                        if (categoryContents == null)
                        {
                            categoryContents = relation;
                        }
                        else
                        {
                            //intersection of sets
                            categoryContents = categoryContents
                                .Where(it => relation.Any(r => r["ContentUUID"].AsString == it["ContentUUID"].AsString));
                        }
                    }

                }
                if (categoryContents != null && categoryContents.Count() != 0)
                {
                    visitor.SetQuery(QueryBuilder.Query.In("UUID", categoryContents.Select(it => it["ContentUUID"]).ToArray()));
                }
                else
                {
                    visitor.SetQuery(QueryBuilder.Query.Where(new BsonJavaScript("false")));
                }
            }

            return textContentQuery.Schema.GetCollection().Find(visitor.QueryComplete);
        }
    }
}
