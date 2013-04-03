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
    public class MongoDBChildrenQuery : MongoDBQuery
    {
        public MongoDBChildrenQuery(IContentQuery<TextContent> contentQuery)
            : base(contentQuery)
        {
        }
        protected override global::MongoDB.Driver.MongoCursor<global::MongoDB.Bson.BsonDocument> Query(MongoDBVisitor visitor)
        {
            var childrenQuery = (ChildrenQuery)this.ContentQuery;

            MongoDBQueryTranslator translator = new MongoDBQueryTranslator();

            var parents = ((IEnumerable<TextContent>)(translator.Translate(childrenQuery.ParentQuery)).Execute());

            if (childrenQuery.EmbeddedFolder != null)
            {
                visitor.SetQuery(MongoDBHelper.EQIgnoreCase("FolderName", childrenQuery.EmbeddedFolder.FullName));
            }
            if (parents.Count() > 0)
            {
                visitor.SetQuery(QueryBuilder.Query.In("ParentUUID", parents.Select(it => BsonHelper.Create(it.UUID)).ToArray()));

                return childrenQuery.ChildSchema.GetCollection().Find(visitor.QueryComplete);
            }

            return null;
        }
    }
}
