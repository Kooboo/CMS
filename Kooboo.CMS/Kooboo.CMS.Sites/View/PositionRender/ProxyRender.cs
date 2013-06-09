#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View.WebProxy;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Kooboo.CMS.Caching;
using Kooboo.CMS.Sites.Caching;
namespace Kooboo.CMS.Sites.View.PositionRender
{
    public class ProxyRender
    {
        #region ProxyRender
        IWebProxy _webProxy;

        public ProxyRender(IWebProxy webProxy)
        {
            _webProxy = webProxy;

        }
        #endregion

        #region Render
        public virtual IHtmlString Render(Page_Context pageContext, string positionId, string defaultHost, string defaultRequestPath, bool noProxy, CacheSettings cacheSetting)
        {
            string moduleURL = pageContext.PageRequestContext.ModuleUrlContext.GetModuleUrl(positionId);
            if (!string.IsNullOrEmpty(moduleURL))
            {
                defaultRequestPath = moduleURL.Trim('~');
            }
            var httpMethod = pageContext.ControllerContext.HttpContext.Request.HttpMethod;

            if (!defaultHost.StartsWith("http://"))
            {
                defaultHost = "http://" + defaultHost;
            }

            var requestUrl = UrlUtility.Combine(defaultHost, defaultRequestPath);

            Func<IHtmlString> getHtml = () =>
            {

                Func<string, bool, string> proxyUrl = null;

                if (!noProxy)
                {
                    proxyUrl = (url, isForm) => GenerateProxyUrl(pageContext, positionId, url, isForm);
                }
                if (httpMethod.ToUpper() == "POST")
                {
                    var postModule = pageContext.PageRequestContext.AllQueryString[Kooboo.CMS.Sites.View.ModuleUrlContext.PostModuleParameter];
                    if (postModule != positionId)
                    {
                        httpMethod = "GET";
                    }
                }


                var html = _webProxy.ProcessRequest(pageContext.ControllerContext.HttpContext, requestUrl, httpMethod, proxyUrl);

                return html;
            };

            if (cacheSetting != null && cacheSetting.EnableCaching != null && cacheSetting.EnableCaching == true && httpMethod.ToUpper() == "GET")
            {
                string cacheKey = string.Format("{0}-{1}-{2}", positionId, requestUrl, noProxy);
                return pageContext.PageRequestContext.Site.ObjectCache().GetCache(cacheKey, getHtml, cacheSetting.ToCachePolicy());
            }
            else
            {
                return getHtml();
            }

        }
        protected string GenerateProxyUrl(Page_Context pageContext, string positionId, string url, bool isForm)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }

            if (!url.StartsWith("#") && !url.StartsWith("javascript:") && !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                var encodedModuleUrl = ModuleUrlHelper.Encode(url);
                var routeValues = pageContext.PageRequestContext.ModuleUrlContext.GetRouteValuesWithModuleUrl(positionId, encodedModuleUrl, false);
                var pageUrl = pageContext.FrontUrl.PageUrl(pageContext.PageRequestContext.Page.FullName, routeValues).ToString();
                if (isForm)
                {
                    pageUrl = Kooboo.Web.Url.UrlUtility.AddQueryParam(pageUrl, Kooboo.CMS.Sites.View.ModuleUrlContext.PostModuleParameter, positionId);
                }

                return pageUrl;
            }
            else
            {
                return url;
            }
        }
        #endregion
    }
}
