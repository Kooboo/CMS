using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Content.Models.Binder;
using System.Collections.Specialized;
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Sites.Extension
{
    public static class PluginHelper
    {
        public static NameValueCollection ApplySubmissionSettings(SubmissionSetting submissionSetting, NameValueCollection defaultValues, Kooboo.Common.TokenTemplate.IValueProvider valueProvider)
        {
            Kooboo.Common.TokenTemplate.ITemplateParser templateParser = new Kooboo.Common.TokenTemplate.TemplateParser();

            if (submissionSetting.Settings != null)
            {
                foreach (var item in submissionSetting.Settings)
                {
                    defaultValues[item.Key] = templateParser.Merge(item.Value, valueProvider);
                }
            }
            return defaultValues;
        }
        public static ActionResult ReturnActionResult(ControllerContext controllerContext, object model, Exception exception)
        {
            var jsonResult = controllerContext.RequestContext.GetRequestValue("JsonResult");
            string redirectUrl = "";
            if (exception == null)
            {
                redirectUrl = controllerContext.RequestContext.GetRequestValue("SuccessedUrl");
            }
            else
            {
                controllerContext.HttpContext.Session["Exception"] = exception;
                redirectUrl = controllerContext.RequestContext.GetRequestValue("FailedUrl");
            }
            if (jsonResult.EqualsOrNullEmpty("true", StringComparison.OrdinalIgnoreCase))
            {
                var data = new JsonResultData() { Model = model, RedirectUrl = redirectUrl };
                if (exception != null)
                {
                    data.AddException(exception);
                    if ((exception is RuleViolationException))
                    {
                        ((RuleViolationException)exception).FillIssues(controllerContext.Controller.ViewData.ModelState);
                        data.AddModelState(controllerContext.Controller.ViewData.ModelState);
                    }
                }
                return new JsonResult() { Data = data };
            }
            if (exception != null && string.IsNullOrEmpty(redirectUrl))
            {
                throw exception;
            }

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                return new RedirectResult(redirectUrl);
            }

            if (controllerContext.HttpContext.Request.UrlReferrer != null)
            {
                return new RedirectResult(controllerContext.HttpContext.Request.UrlReferrer.OriginalString);
            }
            return new EmptyResult();
        }
    }
}
