#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.CMS.Sites.View.WebProxy
{

    public interface IWebProxy
    {
        IHtmlString ProcessRequest(HttpContextBase httpContext, string url, string httpMethod, Func<string, bool, string> proxyUrlFunc);
    }
    [Dependency(typeof(IWebProxy))]
    public class WebProxy : IWebProxy
    {
        IProxyHtmlFixer _htmlFixer;
        IHttpProcessor _httpProcessor;
        public WebProxy(IHttpProcessor httpProcessor, IProxyHtmlFixer htmlFixer)
        {
            _httpProcessor = httpProcessor;
            _htmlFixer = htmlFixer;
        }


        public IHtmlString ProcessRequest(HttpContextBase httpContext, string url, string httpMethod, Func<string, bool, string> proxyUrlFunc)
        {
            IHtmlString htmlString = new HtmlString("");
            var html = _httpProcessor.ProcessRequest(httpContext, url, httpMethod, proxyUrlFunc);
            if (!string.IsNullOrEmpty(html))
            {
                htmlString = _htmlFixer.Fix(url, html, proxyUrlFunc);
            }
            return htmlString;
        }
    }
}
