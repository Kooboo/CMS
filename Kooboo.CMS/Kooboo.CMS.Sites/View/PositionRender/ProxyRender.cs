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
using System.Web.Mvc;
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
        public virtual IHtmlString Render(ControllerContext controllerContext, PageRequestContext pageRequestContext, ProxyPosition proxyPosition, string remoteUrl)
        {
            var positionId = proxyPosition.PagePositionId;
            if (string.IsNullOrEmpty(remoteUrl) && pageRequestContext != null)
            {
                remoteUrl = pageRequestContext.ModuleUrlContext.GetModuleUrl(proxyPosition.PagePositionId);
            }
            var defaultRequestPath = proxyPosition.RequestPath;
            if (!string.IsNullOrEmpty(remoteUrl))
            {
                defaultRequestPath = remoteUrl.Trim('~');
            }
            var httpMethod = controllerContext.HttpContext.Request.HttpMethod;
            var defaultHost = proxyPosition.Host;
            if (!defaultHost.StartsWith("http://"))
            {
                defaultHost = "http://" + defaultHost;
            }

            var requestUrl = UrlUtility.Combine(defaultHost, defaultRequestPath);

            Func<IHtmlString> getHtml = () =>
            {

                Func<string, bool, string> proxyUrl = (url, isForm) => new Uri(new Uri(defaultHost), url).ToString();

                if (!proxyPosition.NoProxy && pageRequestContext != null)
                {
                    proxyUrl = (url, isForm) => GenerateProxyUrl(controllerContext, pageRequestContext, positionId, url, isForm);
                }
                if (httpMethod.ToUpper() == "POST" && pageRequestContext != null)
                {
                    var postModule = pageRequestContext.AllQueryString[Kooboo.CMS.Sites.View.ModuleUrlContext.PostModuleParameter];
                    if (postModule != positionId)
                    {
                        httpMethod = "GET";
                    }
                }


                var html = _webProxy.ProcessRequest(controllerContext.HttpContext, requestUrl, httpMethod, proxyUrl);

                return html;
            };
            var cacheSetting = proxyPosition.OutputCache;
            if (cacheSetting != null && cacheSetting.EnableCaching != null && cacheSetting.EnableCaching == true && httpMethod.ToUpper() == "GET")
            {
                string cacheKey = string.Format("{0}-{1}-{2}", positionId, requestUrl, proxyPosition.NoProxy);
                return pageRequestContext.Site.ObjectCache().GetCache(cacheKey, getHtml, cacheSetting.ToCachePolicy());
            }
            else
            {
                return getHtml();
            }

        }
        protected string GenerateProxyUrl(ControllerContext controllerContext, PageRequestContext pageRequestContext, string positionId, string url, bool isForm)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }

            if (!url.StartsWith("#") && !url.StartsWith("javascript:") && !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                var encodedModuleUrl = ModuleUrlHelper.Encode(url);
                var routeValues = pageRequestContext.ModuleUrlContext.GetRouteValuesWithModuleUrl(positionId, encodedModuleUrl, false);
                var urlHelper = new UrlHelper(controllerContext.RequestContext);
                var pageUrl = urlHelper.FrontUrl(pageRequestContext.Site, pageRequestContext.RequestChannel).PageUrl(pageRequestContext.Page.FullName, routeValues).ToString();
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

        #region TryRemoteRequest
        public virtual ActionResult TryRemoteRequest(ControllerContext controllerContext)
        {
            var hasRemoteProxy = controllerContext.HttpContext.Request.QueryString["hasRemoteProxy"];

            if (!string.IsNullOrEmpty(hasRemoteProxy) && hasRemoteProxy.ToLower() == "true")
            {
                return new RemoteRequestActionResult(this);
            }
            return null;
        }
        #endregion
    }
}
