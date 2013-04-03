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
    public class MongoDBParentQuery : MongoDBQuery
    {
        public MongoDBParentQuery(IContentQuery<TextContent> contentQuery)
            : base(contentQuery)
        {
        }

        protected override global::MongoDB.Driver.MongoCursor<global::MongoDB.Bson.BsonDocument> Query(MongoDBVisitor visitor)
        {
            var parentQuery = (ParentQuery)this.ContentQuery;

            MongoDBQueryTranslator translator = new MongoDBQueryTranslator();

            var children = ((IEnumerable<TextContent>)(translator.Translate(parentQuery.ChildrenQuery)).Execute());

            if (parentQuery.ParentFolder != null)
            {
                visitor.SetQuery(MongoDBHelper.EQIgnoreCase("FolderName", parentQuery.ParentFolder.FullName));
            }

            if (children.Count() > 0)
            {
                visitor.SetQuery(QueryBuilder.Query.In("UUID", children.Select(it => BsonHelper.Create(it.ParentUUID)).ToArray()));

                return parentQuery.ParentSchema.GetCollection().Find(visitor.QueryComplete);
            }

            return null;
        }
    }
}
