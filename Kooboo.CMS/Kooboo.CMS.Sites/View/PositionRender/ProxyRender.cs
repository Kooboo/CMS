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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Kooboo.CMS.Sites.Caching;
using Kooboo.Common.Caching;
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
        public virtual IHtmlString Render(ProxyRenderContext proxyRenderContext)
        {
            var positionId = proxyRenderContext.ProxyPosition.PagePositionId;

            Func<IHtmlString> getHtml = () =>
            {
                var html = _webProxy.ProcessRequest(proxyRenderContext);

                return html;
            };
            var cacheSetting = proxyRenderContext.ProxyPosition.OutputCache;
            if (cacheSetting != null && cacheSetting.EnableCaching != null && cacheSetting.EnableCaching == true && proxyRenderContext.HttpMethod.ToUpper() == "GET")
            {
                string cacheKey = string.Format("{0}||{1}||{2}", positionId, proxyRenderContext.RequestUri.ToString(), proxyRenderContext.ProxyPosition.NoProxy);
                return proxyRenderContext.PageRequestContext.Site.ObjectCache().GetCache(cacheKey, getHtml, cacheSetting.ToCachePolicy());
            }
            else
            {
                return getHtml();
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
