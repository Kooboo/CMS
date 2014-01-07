using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Couchbase.Query
{
    public class CouchbaseQueryTranslator
    {
        public CouchbaseQuery Translate(IContentQuery<TextContent> contentQuery)
        {
            if (contentQuery is CategoriesQuery)
            {
                return new CouchbaseCategoryQuery((CategoriesQuery)contentQuery);
            }
            else if (contentQuery is ChildrenQuery)
            {
                return new CouchbaseChildrenQuery((ChildrenQuery)contentQuery);
            }
            else if (contentQuery is ParentQuery)
            {
                return new CouchbaseParentQuery((ParentQuery)contentQuery);
            }
            else if (contentQuery is TextContentQuery)
            {
                return new CouchbaseTextContentQuery((TextContentQuery)contentQuery);
            }
            return null;
        }
    }
}
