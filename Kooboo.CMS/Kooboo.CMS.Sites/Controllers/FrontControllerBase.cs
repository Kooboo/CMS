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
using System.Web;
using System.Net;

using Kooboo.CMS.Sites.View;
using Kooboo.Common.Globalization;
using Kooboo.CMS.Sites.View.PositionRender;
using Kooboo.Common.Web;
using Kooboo.CMS.Sites.Web;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Extension;


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


            HttpErrorStatusCode statusCode = HttpErrorStatusCode.InternalServerError_500;
            HttpException httpException = filterContext.Exception as HttpException;

            if (httpException != null)
            {
                statusCode = (HttpErrorStatusCode)httpException.GetHttpCode();
            }
            if (statusCode == HttpErrorStatusCode.NotFound_404)
            {
                ProxyRender proxyRender = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<ProxyRender>();
                var actionResult = proxyRender.TryRemoteRequest(filterContext.Controller.ControllerContext);
                if (actionResult != null)
                {
                    filterContext.Result = actionResult;
                    filterContext.ExceptionHandled = true;
                }
            }
            if (filterContext.ExceptionHandled == false)
            {
                if (Site != null)
                {
                    var customError = Services.ServiceFactory.CustomErrorManager.Get(Site, statusCode.ToString());

                    if (customError != null)
                    {
                        var errorUrl = customError.RedirectUrl;

                        if (!string.IsNullOrEmpty(errorUrl) && !errorUrl.TrimStart('~').TrimStart('/').TrimEnd('/').EqualsOrNullEmpty(this.Request.AppRelativeCurrentExecutionFilePath.TrimStart('~').TrimStart('/').TrimEnd('/'), StringComparison.OrdinalIgnoreCase))
                        {
                            filterContext.Result = RedirectHelper.CreateRedirectResult(Site, FrontHttpRequest.RequestChannel, errorUrl, Request.RawUrl, (int)statusCode, customError.RedirectType, customError.ShowErrorPath);
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
                Kooboo.Common.Logging.Logger.Error(filterContext.Exception.Message, filterContext.Exception);
            }

            base.OnException(filterContext);
        }
        protected virtual ActionResult RedirectTo404()
        {
            var notFoundUrl = Url.Action("Index", "NotFound");
            notFoundUrl = notFoundUrl.AddQueryParam("returnUrl", this.Request.RawUrl);

            return new RedirectResult(notFoundUrl);
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }
        #endregion
    }
}
