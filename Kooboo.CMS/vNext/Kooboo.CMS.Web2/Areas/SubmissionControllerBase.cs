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
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Kooboo.CMS.Web2.Models;

using Kooboo.CMS.Content.Models.Binder;
using Kooboo.CMS.Sites;
using Kooboo.Common.ObjectContainer;
using Kooboo.Common.Globalization;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Web2.Areas
{
    [Obsolete("Please use new SubmssionSetting function for security reason.")]
    [ValidateAntiForgeryToken]
    [ValidateInput(false)]
    public class SubmissionControllerBase : AreaControllerBase
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);            
            //if (!Site.Security.TurnOnSubmissionAPI)
            //{
            //    throw new InvalidOperationException("The submission service is not available.".Localize());
            //}
        }
        protected ActionResult ReturnActionResult(object model, Exception exception)
        {
            var jsonResult = this.ControllerContext.RequestContext.GetRequestValue("JsonResult");
            string redirectUrl = "";
            if (exception == null)
            {
                redirectUrl = this.ControllerContext.RequestContext.GetRequestValue("SuccessedUrl");
            }
            else
            {
                Session["Exception"] = exception;
                redirectUrl = this.ControllerContext.RequestContext.GetRequestValue("FailedUrl");
            }



            if (jsonResult.EqualsOrNullEmpty("true", StringComparison.OrdinalIgnoreCase))
            {
                var data = new JsonResultData() { Model = model, RedirectUrl = redirectUrl };
                if (exception != null)
                {
                    data.AddException(exception);
                    if ((exception is RuleViolationException))
                    {
                        ((RuleViolationException)exception).FillIssues(this.ModelState);
                        data.AddModelState(this.ModelState);
                    }
                }
                return Json(data);
            }
            if (exception != null && string.IsNullOrEmpty(redirectUrl))
            {
                throw exception;
            }

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                return Redirect(redirectUrl);
            }

            if (this.Request.UrlReferrer != null)
            {
                return Redirect(this.Request.UrlReferrer.OriginalString);
            }
            return new EmptyResult();
        }
    }
}