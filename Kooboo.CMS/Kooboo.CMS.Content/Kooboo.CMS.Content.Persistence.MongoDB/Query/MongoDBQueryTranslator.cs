using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.MongoDB.Query
{
    public class MongoDBQueryTranslator
    {
        public MongoDBQuery Translate(IContentQuery<TextContent> contentQuery)
        {
            if (contentQuery is CategoriesQuery)
            {
                return new MongoDBCategoriesQuery(contentQuery);
            }
            else if (contentQuery is ChildrenQuery)
            {
                return new MongoDBChildrenQuery(contentQuery);
            }
            else if (contentQuery is ParentQuery)
            {
                return new MongoDBParentQuery(contentQuery);
            }
            else if (contentQuery is TextContentQuery)
            {
                return new MongoDBTextContentQuery(contentQuery);
            }
            return null;
        }
    }
}
