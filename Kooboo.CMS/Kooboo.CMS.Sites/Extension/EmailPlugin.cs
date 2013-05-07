using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.CMS.Sites.Extension
{
    public class EmailPlugin : ISubmissionPlugin
    {
        #region ISubmissionPlugin
        public System.Web.Mvc.ActionResult Submit(Models.Site site, System.Web.Mvc.ControllerContext controllerContext, Models.SubmissionSetting submissionSetting)
        {
            Exception exception = null;
            try
            {
                var formValues = new NameValueCollection(controllerContext.HttpContext.Request.Form);

                formValues = PluginHelper.ApplySubmissionSettings(submissionSetting, formValues);

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

            var smtp = site.Smtp;
            if (smtp == null)
            {
                throw new ArgumentNullException("smtp");
            }


            MailMessage message = new MailMessage() { From = new MailAddress(from) };
            foreach (var item in smtp.To)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    message.To.Add(item);
                }
            }
            message.Subject = subject;
            message.Body = body;

            if (isBodyHtml.HasValue)
            {
                message.IsBodyHtml = isBodyHtml.Value;
            }
            if (files.Count > 0)
            {
                foreach (string key in files.AllKeys)
                {
                    HttpPostedFileBase file = files[key] as HttpPostedFileBase;

                    message.Attachments.Add(new Attachment(file.InputStream, file.FileName, IO.IOUtility.MimeType(file.FileName)));
                }
            }

            SmtpClient smtpClient = smtp.ToSmtpClient();

            smtpClient.Send(message);
        }
        #endregion

    }
}
