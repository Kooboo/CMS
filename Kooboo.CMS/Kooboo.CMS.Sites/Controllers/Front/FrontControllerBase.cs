using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Web;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Extension;
using Kooboo.Web.Url;
using System.Web;
using System.Net;
using Kooboo.CMS.Sites.View;
using Kooboo.Web.Mvc;
using Kooboo.Globalization;

namespace Kooboo.CMS.Sites.Controllers.Front
{

    public class Redirect301Result : RedirectResult
    {
        public Redirect301Result(string redirectUrl)
            : base(redirectUrl)
        {
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var httpContext = context.HttpContext;
            var request = (FrontHttpRequestWrapper)httpContext.Request;

            context.HttpContext.Response.StatusCode = 301;
            context.HttpContext.Response.RedirectLocation = Url;
            context.HttpContext.Response.End();

        }
    }

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

    public static class RedirectHelper
    {
        public static ActionResult CreateRedirectResult(Site site, FrontRequestChannel channel, string url, string rawUrl, int? statusCode, RedirectType redirectType)
        {
            var redirectUrl = url;
            if (!UrlUtility.IsAbsoluteUrl(redirectUrl))
            {
                redirectUrl = UrlUtility.ResolveUrl(redirectUrl);
                //WrapperUrl will cause endless loop if site host by ASP.NET development server when transfer redirect.
                if (redirectType != RedirectType.Transfer || Settings.IsHostByIIS)
                {
                    redirectUrl = FrontUrlHelper.WrapperUrl(redirectUrl, site, channel).ToString();
                }
            }
            if (!string.IsNullOrEmpty(rawUrl))
            {
                redirectUrl = redirectUrl.AddQueryParam("errorpath", rawUrl);
            }
            if (statusCode != null)
            {
                redirectUrl = redirectUrl.AddQueryParam("statusCode", statusCode.ToString());
            }

            switch (redirectType)
            {
                case RedirectType.Moved_Permanently_301:
                    return new Redirect301Result(redirectUrl);

                case RedirectType.Transfer:
                    return new TransferResult(redirectUrl, statusCode ?? 200);
                    break;
                case RedirectType.Found_Redirect_302:
                default:
                    return new RedirectResult(redirectUrl);
            }

        }
    }

    public class FrontControllerBase : Controller
    {
        #region Public properties

        public FrontHttpContextWrapper FrontPageHttpContext
        {
            get
            {
                return (FrontHttpContextWrapper)this.HttpContext;
            }
        }
        public FrontHttpRequestWrapper FrontHttpRequest
        {
            get
            {
                return (FrontHttpRequestWrapper)this.FrontPageHttpContext.Request;
            }
        }
        public Site Site
        {
            get
            {
                return FrontHttpRequest.Site;
            }
        }

        public string RequestUrl
        {
            get
            {
                return FrontHttpRequest.RequestUrl;
            }
        }
        #endregion

        protected override void ExecuteCore()
        {
            if (this.Site == null)
            {
                OnSiteNotExists();
            }
            else
            {
                SiteEventDispatcher.OnPreSiteRequestExecute(Site, this.HttpContext);

                base.ExecuteCore();
            }
        }
        #region Exception handle
        protected virtual void OnSiteNotExists()
        {
            throw new HttpException(0x194, string.Format(SR.GetString("Path_not_found"), new object[] { HttpContext.Request.Path }));
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Kooboo.HealthMonitoring.Log.LogException(filterContext.Exception);

            HttpErrorStatusCode statusCode = HttpErrorStatusCode.InternalServerError_500;
            HttpException httpException = filterContext.Exception as HttpException;

            if (httpException != null)
            {
                statusCode = (HttpErrorStatusCode)httpException.GetHttpCode();
            }
            if (Site != null)
            {
                var customError = Services.ServiceFactory.CustomErrorManager.Get(Site, statusCode.ToString());

                if (customError != null)
                {
                    var errorUrl = customError.RedirectUrl;

                    if (!string.IsNullOrEmpty(errorUrl) && !errorUrl.TrimStart('~').TrimStart('/').TrimEnd('/').EqualsOrNullEmpty(this.Request.AppRelativeCurrentExecutionFilePath.TrimStart('~').TrimStart('/').TrimEnd('/'), StringComparison.OrdinalIgnoreCase))
                    {
                        filterContext.Result = RedirectHelper.CreateRedirectResult(Site, FrontHttpRequest.RequestChannel, errorUrl, Request.RawUrl, (int)statusCode, customError.RedirectType);
                        filterContext.ExceptionHandled = true;
                    }
                }
            }
            else
            {
                if (statusCode == HttpErrorStatusCode.NotFound_404)
                {
                    filterContext.Result = RedirectTo404();
                    filterContext.ExceptionHandled = true;
                }
            }
            base.OnException(filterContext);
        }
        protected virtual ActionResult RedirectTo404()
        {
            var notFoundUrl = Url.Action("Index", "NotFound");
            notFoundUrl = notFoundUrl.AddQueryParam("errorpath", this.Request.RawUrl);

            return new RedirectResult(notFoundUrl);
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            if (Site != null)
            {
                SiteEventDispatcher.OnPostSiteRequestExecute(Site, this.HttpContext);
            }
        }
        #endregion

        #region Cache setting
        protected virtual void CacheThisRequest()
        {
            SetCache(HttpContext.Response, 2592000, "*");
        }
        protected virtual void SetCache(HttpResponseBase response, int cacheDuration, params string[] varyByParams)
        {
            // Cache
            if (cacheDuration > 0)
            {
                DateTime timestamp = HttpContext.Timestamp;

                HttpCachePolicyBase cache = response.Cache;
                int duration = cacheDuration;

                cache.SetCacheability(HttpCacheability.Public);
                cache.SetAllowResponseInBrowserHistory(true);
                cache.SetExpires(timestamp.AddSeconds(duration));
                cache.SetMaxAge(new TimeSpan(0, 0, duration));
                cache.SetValidUntilExpires(true);
                cache.SetLastModified(timestamp);
                cache.VaryByHeaders["Accept-Encoding"] = true;
                if (varyByParams != null)
                {
                    foreach (var p in varyByParams)
                    {
                        cache.VaryByParams[p] = true;
                    }
                }
                cache.SetOmitVaryStar(true);
            }
        }
        #endregion
    }
}
