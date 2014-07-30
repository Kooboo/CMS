#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public static class PageLinkExtensions
    {
        #region PageLink
        public static IHtmlString PageLink(this IFrontHtmlHelper frontHtml, string urlKey)
        {
            return PageLink(frontHtml, null, urlKey);
        }
        public static IHtmlString PageLink(this IFrontHtmlHelper frontHtml, object linkText, string urlKey)
        {
            return PageLink(frontHtml, linkText, urlKey, null);
        }
        public static IHtmlString PageLink(this IFrontHtmlHelper frontHtml, object linkText, string urlKey, object routeValues)
        {
            return PageLink(frontHtml, linkText, urlKey, routeValues, null);
        }
        public static IHtmlString PageLink(this IFrontHtmlHelper frontHtml, object linkText, string urlKey, object routeValues, object htmlAttributes)
        {
            var htmlValues = RouteValuesHelper.GetRouteValues(htmlAttributes);
            Page page;

            string url = frontHtml.Page_Context.FrontUrl.PageUrl(urlKey, routeValues, out page).ToString();

            page = page.AsActual();

            var innerHtml = "";
            if (linkText == null)
            {
                innerHtml = page.GetLinkText();
            }
            else
            {
                innerHtml = HttpUtility.HtmlEncode(linkText);
            }
            TagBuilder builder = new TagBuilder("a")
            {
                InnerHtml = innerHtml
            };
            builder.MergeAttributes<string, object>(htmlValues);
            builder.MergeAttribute("href", url);
            if (page != null && page.Route != null)
            {
                builder.MergeAttribute("target", page.Route.LinkTarget.ToString());
            }
            var html = new HtmlString(builder.ToString(TagRenderMode.Normal));

#if Page_Trace
            stopwatch.Stop();
            HttpContext.Current.Response.Write(string.Format("PageLink,{0}.</br>", stopwatch.Elapsed));
#endif
            return html;
        }
        #endregion
    }
}
