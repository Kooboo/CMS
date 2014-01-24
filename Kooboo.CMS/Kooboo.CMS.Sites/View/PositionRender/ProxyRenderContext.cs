#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.View.PositionRender
{
    public class ProxyRenderContext
    {
        public ProxyRenderContext(ControllerContext controllerContext,
            PageRequestContext pageRequestContext, ProxyPosition proxyPosition, string requestPath)
        {
            this.ControllerContext = controllerContext;
            this.PageRequestContext = pageRequestContext;
            this.ProxyPosition = proxyPosition;


            if (string.IsNullOrEmpty(requestPath) && PageRequestContext != null)
            {
                requestPath = pageRequestContext.ModuleUrlContext.GetModuleUrl(proxyPosition.PagePositionId);
            }

            if (string.IsNullOrEmpty(requestPath))
            {
                requestPath = proxyPosition.RequestPath;
            }
            requestPath = requestPath.Trim('~').Trim();

            RequestUri = new Uri(proxyPosition.HostUri, requestPath);

            var httpMethod = controllerContext.HttpContext.Request.HttpMethod;
            if (httpMethod.ToUpper() == "POST" && pageRequestContext != null)
            {
                var postModule = pageRequestContext.AllQueryString[Kooboo.CMS.Sites.View.ModuleUrlContext.PostModuleParameter];
                if (postModule != proxyPosition.PagePositionId)
                {
                    httpMethod = "GET";
                }
            }
            HttpMethod = httpMethod;
        }
        public ControllerContext ControllerContext { get; private set; }

        public PageRequestContext PageRequestContext { get; private set; }

        public ProxyPosition ProxyPosition { get; set; }

        //public string RequestPath { get; private set; }

        public Uri RequestUri { get; private set; }

        public string HttpMethod { get; private set; }

    }
}
