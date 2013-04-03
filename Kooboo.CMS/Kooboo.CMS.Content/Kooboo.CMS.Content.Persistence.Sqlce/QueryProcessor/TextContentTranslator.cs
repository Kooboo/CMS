using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;

namespace Kooboo.CMS.Content.Persistence.Sqlce.QueryProcessor
{
    public class TextContentTranslator : IContentQueryTranslator<TextContent>
    {
        #region IContentQueryTranslator<TextContent> Members

        public IQueryExecutor<TextContent> Translate(Query.IContentQuery<TextContent> contentQuery)
        {
            if (contentQuery is CategoriesQuery)
            {
                return new CategoriesQueryExecutor((CategoriesQuery)contentQuery);
            }
            //else if (contentQuery is CategorizablesQuery)
            //{
            //    return new CategorizablesQueryExecutor((CategorizablesQuery)contentQuery);
            //}
            else if (contentQuery is ChildrenQuery)
            {
                return new ChildrenQueryExecutor((ChildrenQuery)contentQuery);
            }
            else if (contentQuery is ParentQuery)
            {
                return new ParentQueryExecutor((ParentQuery)contentQuery);
            }
            else if (contentQuery is TextContentQuery)
            {
                return new SimpleQueryExecutor((TextContentQuery)contentQuery);
            }
            return null;
        }

        #endregion
    }
}
