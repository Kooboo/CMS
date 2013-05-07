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
using Kooboo.Web.Mvc.Paging;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query.Expressions;

namespace Kooboo.CMS.Content.Query
{
    public static class IContentQueryToPagedListExtension
    {
        public static PagedList<T> ToPageList<T>(this IContentQuery<T> contentQuery, int pageIndex, int pageSize, bool enablePaging = true)
            where T : ContentBase
        {
            if (enablePaging == true)
            {
                if (pageIndex < 1)
                    pageIndex = 1;
                var itemIndex = (pageIndex - 1) * pageSize;
                var totalItemCount = contentQuery.Count();
                var pageOfItems = contentQuery.Skip(itemIndex).Take(pageSize);
                return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
            }
            else
            {
                var totalItemCount = contentQuery.Count();
                var pageOfItems = contentQuery.Take(totalItemCount);
                return new PagedList<T>(pageOfItems, 1, totalItemCount, totalItemCount);
            }
        }


    }
}
