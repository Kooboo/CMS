using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.CMS.Content.Models.Binder;
using System.Collections.Specialized;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Formula;
namespace Kooboo.CMS.Sites.Extension
{
    public static class PluginHelper
    {
        public static NameValueCollection ApplySubmissionSettings(SubmissionSetting submissionSetting, NameValueCollection defaultValues, Kooboo.CMS.Common.Formula.IValueProvider valueProvider)
        {
            IFormulaParser formulaParser = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IFormulaParser>();
          
            if (submissionSetting.Settings != null)
            {
                foreach (var item in submissionSetting.Settings)
                {
                    defaultValues[item.Key] = formulaParser.Populate(item.Value, valueProvider);
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
