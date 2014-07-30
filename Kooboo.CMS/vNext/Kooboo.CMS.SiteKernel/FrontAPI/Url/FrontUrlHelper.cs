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
using System.Web.Routing;
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public class FrontUrlHelper : IFrontUrlHelper
    {
        #region .ctor
        public UrlHelper Url
        {
            get;
            private set;
        }

        public Site Site
        {
            get;
            private set;
        }

        public FrontRequestChannel RequestChannel
        {
            get;
            private set;
        }
        public FrontUrlHelper(UrlHelper urlHelper, Site site, FrontRequestChannel requestChannel)
        {
            this.Url = urlHelper;
            this.Site = site;
            this.RequestChannel = requestChannel;
        }
        #endregion

        public System.Web.IHtmlString WrapperUrl(string url)
        {
            throw new NotImplementedException();
        }

        public System.Web.IHtmlString WrapperUrl(string url, bool? requireSSL)
        {
            throw new NotImplementedException();
        }     

        #region GeneratePageUrl
        public virtual IHtmlString GeneratePageUrl(string urlKey, object values, Func<Site, string, Page> findPage, out Page page)
        {
            var site = this.Site;

            page = new Page(site, urlKey).LastVersion(this.Site);
            if (page == null)
            {
                page = findPage(site, urlKey);
                string pageFullName = "";
                if (page != null)
                {
                    pageFullName = page.FullName;
                }
            }
            if (page != null)
            {
                var url = GeneratePageUrl(page, values);
                return url;
            }
            else
            {
                return new HtmlString("");
            }
        }

        public virtual IHtmlString GeneratePageUrl(Page page, object values)
        {
            return GeneratePageUrl(this.Url, this.Site, page, values, this.RequestChannel);
        }
        internal IHtmlString GeneratePageUrl(UrlHelper urlHelper, Site site, Page page, object values, FrontRequestChannel channel)
        {
            RouteValueDictionary routeValues = RouteValuesHelper.GetRouteValues(values);

            page = page.AsActual();

            if (page == null)
            {
                return new HtmlString("");
            }
            if (page.Route != null && !string.IsNullOrEmpty(page.Route.ExternalUrl))
            {
                return new HtmlString(page.Route.ExternalUrl);
            }

            var pageRoute = page.Route.ToMvcRoute();

            routeValues = RouteValuesHelper.MergeRouteValues(pageRoute.Defaults, routeValues);

            var routeVirtualPath = pageRoute.GetVirtualPath(urlHelper.RequestContext, routeValues);
            if (routeVirtualPath == null)
            {
                Kooboo.Common.Logging.Logger.LoggerInstance.Warn(string.Format("Invalid page URL route. Page:{0}", page.FullName));
            }
            //string contentUrl = routeVirtualPath.VirtualPath;//don't decode the url. why??
            //if do not decode the url, the route values contains Chinese character will cause bad request.
            string contentUrl = HttpUtility.UrlDecode(routeVirtualPath.VirtualPath);
            string pageUrl = contentUrl;
            if (!string.IsNullOrEmpty(contentUrl) || (string.IsNullOrEmpty(pageUrl) && !page.IsDefault))
            {
                pageUrl = Kooboo.Common.Web.UrlUtility.Combine(page.GetVirutalPath(), contentUrl);
            }
            if (string.IsNullOrEmpty(pageUrl))
            {
                pageUrl = urlHelper.Content("~/");
            }
            else
            {
                pageUrl = HttpUtility.UrlDecode(
                urlHelper.RouteUrl("Page", new { PageUrl = new HtmlString(pageUrl) }));
            }
            var url = this.WrapperUrl(pageUrl, page.RequireHttps);

            return url;
        }
        #endregion
    }
}
