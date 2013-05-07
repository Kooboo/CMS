#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Controllers
{
    public class TransferResult : ActionResult
    {
        public string TransferUrl { get; set; }
        public TransferResult(string transferUrl, int statusCode)
        {
            this.TransferUrl = transferUrl;
            this.StatusCode = statusCode;
        }
        public int StatusCode { get; private set; }

        internal class TransferHttpHandler : MvcHttpHandler
        {
            public void ProcessRequestEx(HttpContextBase httpContext)
            {
                base.ProcessRequest(httpContext);
            }
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;

            var request = (FrontHttpRequestWrapper)httpContext.Request;

            request.RequestUrl = TransferUrl;

            httpContext.Response.StatusCode = StatusCode;
            // MVC 3 running on IIS 7+
            if (HttpRuntime.UsingIntegratedPipeline)
            {
                httpContext.Server.TransferRequest(TransferUrl, true);
            }
            else
            {
                // Pre MVC 3
                httpContext.RewritePath(TransferUrl, false);

                var httpHandler = new TransferHttpHandler();
                httpHandler.ProcessRequestEx(httpContext);
            }

        }
    }
}
