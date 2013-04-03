using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using Kooboo.CMS.Sites.Models;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Web
{
    public class FrontUrlRoutingModule : UrlRoutingModule
    {
        protected override void Init(HttpApplication application)
        {
            application.PostResolveRequestCache += new EventHandler(application_PostResolveRequestCache);
        }

        void application_PostResolveRequestCache(object sender, EventArgs e)
        {
            HttpContextBase context = new FrontHttpContextWrapper(((HttpApplication)sender).Context);
            this.PostResolveRequestCache(context);
            //if (Site.Current != null)
            //{
            //    UrlRedirect(context);
            //}
        }

        //internal class TransferHttpHandler : MvcHttpHandler
        //{
        //    public void ProcessRequestEx(HttpContextBase httpContext)
        //    {
        //        base.ProcessRequest(httpContext);
        //    }
        //}
        //void UrlRedirect(HttpContextBase httpContext)
        //{
        //    var request = (FrontHttpRequestWrapper)httpContext.Request;
        //    RedirectType redirectType;
        //    string inputUrl = request.RequestUrl;
        //    if (string.IsNullOrEmpty(inputUrl))
        //    {
        //        inputUrl = "/";
        //    }
        //    if (!string.IsNullOrEmpty(request.Url.Query))
        //    {
        //        inputUrl = inputUrl + request.Url.Query;
        //    }
        //    string redirectUrl;
        //    if (UrlMapperFactory.Default.Map(Site.Current, inputUrl, out redirectUrl, out redirectType))
        //    {
        //        if (redirectType == RedirectType.Found_Redirect_302 || redirectType == RedirectType.Moved_Permanently_301)
        //        {
        //            httpContext.Response.StatusCode = (int)redirectType;
        //            httpContext.Response.RedirectLocation = redirectUrl;
        //            httpContext.Response.End();
        //        }
        //        else
        //        {
        //            request.RequestUrl = redirectUrl;

        //            //httpContext.Response.StatusCode = 200;
        //            // MVC 3 running on IIS 7+
        //            if (HttpRuntime.UsingIntegratedPipeline)
        //            {
        //                httpContext.Server.TransferRequest(redirectUrl, true);
        //            }
        //            else
        //            {
        //                // Pre MVC 3
        //                httpContext.RewritePath(redirectUrl, false);

        //                //var httpHandler = new TransferHttpHandler();
        //                //httpHandler.ProcessRequestEx(httpContext);
        //            }
        //        }
        //    }
        //}
    }
}
