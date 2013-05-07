#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Default.ContentQuery
{
    internal static class ContentQueryExecutor
    {
        public static object Execute(IContentQuery<TextContent> contentQuery)
        {
            QueryExecutorBase queryExecutor = null;
            if (contentQuery is CategoriesQuery)
            {
                queryExecutor = new CategoriesQueryExecutor((CategoriesQuery)contentQuery);
            }          
            else if (contentQuery is ParentQuery)
            {
                queryExecutor = new ParentQueryExecutor((ParentQuery)contentQuery);
            }
            else if (contentQuery is ChildrenQuery)
            {
                queryExecutor = new ChildrenQueryExecutor((ChildrenQuery)contentQuery);
            }
            else if (contentQuery is TextContentQuery)
            {
                queryExecutor = new TextContentQueryExecutor((TextContentQuery)contentQuery);
            }
            return queryExecutor.Execute();
        }
    }
}
