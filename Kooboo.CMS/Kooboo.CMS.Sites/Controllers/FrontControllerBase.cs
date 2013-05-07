#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
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

namespace Kooboo.CMS.Sites.Controllers
{
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

        #region Exception handle

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
                response.AddHeader("Cache-Control", string.Format("public, max-age={0}", cacheDuration));
                //DateTime timestamp = httpContext.Timestamp;

                //HttpCachePolicyBase cache = response.Cache;
                //int duration = cacheDuration;

                //cache.SetCacheability(HttpCacheability.Public);
                //cache.SetAllowResponseInBrowserHistory(true);
                //cache.SetExpires(timestamp.AddSeconds(duration));
                //cache.SetMaxAge(new TimeSpan(0, 0, duration));
                //cache.SetValidUntilExpires(true);
                //cache.SetLastModified(timestamp);
                //cache.VaryByHeaders["Accept-Encoding"] = true;
                //if (varyByParams != null)
                //{
                //    foreach (var p in varyByParams)
                //    {
                //        cache.VaryByParams[p] = true;
                //    }
                //}

                //cache.SetOmitVaryStar(true);
            }
        }
        #endregion
    }
}
