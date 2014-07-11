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
using System.Web;
using System.Web.Routing;
using System.Linq.Dynamic;

using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Common.Persistence;
namespace Kooboo.CMS.Web
{
    public static class SortByExtension
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> list, string sortField, string sortDir)
        {
            if (!string.IsNullOrEmpty(sortField))
            {
                var sort = sortField;
                if (sortDir == "desc")
                {
                    sort = sort + " descending";
                }
                list = list.OrderBy(sort);
            }
            else if (typeof(IChangeTimeline).IsAssignableFrom(typeof(T)))
            {
                list = list.OrderBy("UtcCreationDate descending");
            }
            return list;
        }

        public static IContentQuery<T> SortBy<T>(this IContentQuery<T> list, string sortField, string sortDir)
            where T : ContentBase
        {
            if (!string.IsNullOrEmpty(sortField))
            {
                var sort = sortField;
                if (sortDir == "desc")
                {
                    return list.OrderByDescending(sortField);
                }
                else
                {
                    return list.OrderBy(sortField);
                }
            }
            return list;
        }

        #region RenderGridItemHeaderAtts RenderGridHeader
        public static IHtmlString RenderSortHeaderClass(RequestContext requestContext, string propertyName, int propertyOrder)
        {
            if (IsSortField(requestContext, propertyName, propertyOrder))
            {
                var sortDir = requestContext.GetRequestValue("sortDir") ?? "asc";

                return new HtmlString(string.Format("sort {0}", sortDir));
            }

            return new HtmlString("sort");
        }

        private static bool IsSortField(RequestContext requestContext, string propertyName, int propertyOrder)
        {
            var sortField = requestContext.GetRequestValue("sortField");

            return sortField != null && sortField.ToLower() == propertyName.ToLower();
        }

        public static IHtmlString RenderGridHeader(RequestContext requestContext, string headerText, string propertyName, int propertyOrder)
        {
            var html = @"<a href=""{0}"">{1}<img class=""icon arrow"" src=""{2}""></a>";
            var sortDir = "asc";
            if (IsSortField(requestContext, propertyName, propertyOrder))
            {
                sortDir = requestContext.GetRequestValue("sortDir") == "asc" ? "desc" : "asc";
            }
            var sortUrl = requestContext.UrlHelper().Action(requestContext.GetRequestValue("action"),
                requestContext.AllRouteValues().Merge("sortField", propertyName).Merge("sortDir", sortDir));
            return new HtmlString(string.Format(html, sortUrl, headerText, requestContext.UrlHelper().Content("~/Images/invis.gif")));
        }
        #endregion
    }
}