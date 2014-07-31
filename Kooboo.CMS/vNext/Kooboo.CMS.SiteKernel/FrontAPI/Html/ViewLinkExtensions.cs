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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public static class ViewLinkExtensions
    {
        #region ViewLink
        public static IHtmlString ViewLink(this IFrontHtmlHelper frontHtmlHelper, object linkText, string viewName)
        {
            return ViewLink(frontHtmlHelper,linkText, viewName, null);
        }
        public static IHtmlString ViewLink(this IFrontHtmlHelper frontHtmlHelper, object linkText, string viewName, object routeValues)
        {
            return ViewLink(frontHtmlHelper, linkText, viewName, routeValues, null);
        }
        public static IHtmlString ViewLink(this IFrontHtmlHelper frontHtmlHelper, object linkText, string viewName, object routeValues, object htmlAttributes)
        {
            var htmlValues = RouteValuesHelper.GetRouteValues(htmlAttributes);

            string url = frontHtmlHelper.Page_Context.FrontUrl.ViewUrl(viewName, routeValues).ToString();


            TagBuilder builder = new TagBuilder("a")
            {
                InnerHtml = linkText != null ? HttpUtility.HtmlEncode(linkText) : string.Empty
            };

            builder.MergeAttributes<string, object>(htmlValues);
            builder.MergeAttribute("href", url);
            var html = new HtmlString(builder.ToString(TagRenderMode.Normal));

            return html;
        }
        #endregion
    }
}
