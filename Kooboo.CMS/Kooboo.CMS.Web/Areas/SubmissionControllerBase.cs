using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Web.Models;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Content.Models.Binder;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.View;
using Kooboo.Globalization;
namespace Kooboo.CMS.Web.Areas
{
    [ValidateAntiForgeryToken]
    [ValidateInput(false)]
    public class SubmissionControllerBase : AreaControllerBase
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!SecurityHelper.IsSubmissionServiceAvailable())
            {
                throw new InvalidOperationException("The submission service is not available.".Localize());
            }
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
                if (exception != null && (exception is RuleViolationException))
                    ((RuleViolationException)exception).FillIssues(this.ModelState);
                JsonResultEntry resultEntry = new JsonResultEntry(this.ModelState) { Model = model, RedirectUrl = redirectUrl };
                resultEntry.AddException(exception);
                return Json(resultEntry);
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