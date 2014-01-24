#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Sites.View.PositionRender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.View.WebProxy
{

    public interface IWebProxy
    {
        IHtmlString ProcessRequest(ProxyRenderContext proxyRenderContext);
    }
    [Dependency(typeof(IWebProxy))]
    public class WebProxy : IWebProxy
    {
        IProxyHtmlParser _htmlParser;
        IHttpProcessor _httpProcessor;
        public WebProxy(IHttpProcessor httpProcessor, IProxyHtmlParser htmlParser)
        {
            _httpProcessor = httpProcessor;
            _htmlParser = htmlParser;
        }


        public IHtmlString ProcessRequest(ProxyRenderContext proxyRenderContext)
        {
            Func<string, bool, string> proxyUrl = (url, isForm) => new Uri(proxyRenderContext.ProxyPosition.HostUri, url).ToString();

            if (!proxyRenderContext.ProxyPosition.NoProxy && proxyRenderContext.PageRequestContext != null)
            {
                proxyUrl = (url, isForm) => GenerateProxyUrl(proxyRenderContext, url, isForm);
            }           
            IHtmlString htmlString = new HtmlString("");
            var html = _httpProcessor.ProcessRequest(proxyRenderContext.ControllerContext.HttpContext, proxyRenderContext.RequestUri.ToString(), proxyRenderContext.HttpMethod, proxyUrl);
            if (!string.IsNullOrEmpty(html))
            {
                htmlString = _htmlParser.Parse(proxyRenderContext, html, proxyUrl);
            }
            return htmlString;
        }

        protected string GenerateProxyUrl(ProxyRenderContext proxyRenderContext, string url, bool isForm)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                var destinationURI = new Uri(url);
                var host = proxyRenderContext.ProxyPosition.HostUri;
                if (destinationURI.Host.ToLower() == host.Host.ToLower())
                {
                    url = destinationURI.PathAndQuery;
                }
            }

            if (!url.StartsWith("#") && !url.StartsWith("javascript:") && !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                var encodedModuleUrl = ModuleUrlHelper.Encode(url);
                var routeValues = proxyRenderContext.PageRequestContext.ModuleUrlContext.GetRouteValuesWithModuleUrl(proxyRenderContext.ProxyPosition.PagePositionId, encodedModuleUrl, false);
                var urlHelper = new UrlHelper(proxyRenderContext.ControllerContext.RequestContext);
                var pageUrl = urlHelper.FrontUrl(proxyRenderContext.PageRequestContext.Site, proxyRenderContext.PageRequestContext.RequestChannel).PageUrl(proxyRenderContext.PageRequestContext.Page.FullName, routeValues).ToString();
                if (isForm)
                {
                    pageUrl = Kooboo.Web.Url.UrlUtility.AddQueryParam(pageUrl, Kooboo.CMS.Sites.View.ModuleUrlContext.PostModuleParameter, proxyRenderContext.ProxyPosition.PagePositionId);
                }

                return pageUrl;
            }
            else
            {
                return url;
            }
        }
    }
}
