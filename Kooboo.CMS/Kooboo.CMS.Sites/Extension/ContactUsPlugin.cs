using Kooboo.CMS.Common.Formula;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
namespace Kooboo.CMS.Sites.Extension
{
    public class ContactUsPlugin : ISubmissionPlugin
    {
        #region Parameters
        public Dictionary<string, object> Parameters
        {
            get
            {
                return new Dictionary<string, object>() { 
                    {"Body","{Body}"},
                    {"IsBodyHtml","true"}                   
                };
            }
        }
        #endregion

        #region ISubmissionPlugin
        public System.Web.Mvc.ActionResult Submit(Models.Site site, System.Web.Mvc.ControllerContext controllerContext, Models.SubmissionSetting submissionSetting)
        {
            Exception exception = null;
            try
            {
                var defaultValues = new NameValueCollection(controllerContext.HttpContext.Request.Unvalidated().Form);
                var valueProvider = new MvcValueProvider(controllerContext.Controller.ValueProvider);

                var formValues = PluginHelper.ApplySubmissionSettings(submissionSetting, defaultValues, valueProvider);

                var from = formValues["From"];
                var subject = formValues["Subject"];
                var body = formValues["Body"];
                var isBodyHtml = !string.IsNullOrEmpty(formValues["isBodyHtml"]) && formValues["isBodyHtml"].ToLower() == "true";
                SendMail(site, from, subject, body, isBodyHtml, controllerContext.HttpContext.Request.Files);
            }
            catch (Exception e)
            {
                exception = e;
            }
            return PluginHelper.ReturnActionResult(controllerContext, null, exception);
        }
        #endregion

        #region SendMail

        public void SendMail(Site site, string from, string subject, string body, bool? isBodyHtml, HttpFileCollectionBase files)
        {
            site.SendMailToSiteManager(from, subject, body, isBodyHtml == null ? false : isBodyHtml.Value, files);
        }
        #endregion

    }
}
