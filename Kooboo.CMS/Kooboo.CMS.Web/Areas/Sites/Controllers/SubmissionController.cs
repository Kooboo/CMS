using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using Kooboo.CMS.Sites.Models;
using Kooboo.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    public class SubmissionController : SubmissionControllerBase
    {
        public virtual ActionResult Email(string from, string subject, string body, string email_template, bool? isBodyHtml)
        {
            Exception exception = null;
            try
            {
                var smtp = Site.Current.Smtp;
                if (smtp == null)
                {
                    throw new KoobooException("Smtp setting is null".Localize());
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
                if (!string.IsNullOrEmpty(email_template))
                {
                    message.Body = EvaluateStringFormulas(email_template, Request);
                }
                if (isBodyHtml.HasValue)
                {
                    message.IsBodyHtml = isBodyHtml.Value;
                }
                if (this.Request.Files.Count > 0)
                {
                    foreach (string key in this.Request.Files.AllKeys)
                    {
                        HttpPostedFileBase file = Request.Files[key] as HttpPostedFileBase;

                        message.Attachments.Add(new Attachment(file.InputStream, file.FileName, IO.IOUtility.MimeType(file.FileName)));
                    }
                }


                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = smtp.Host;
                smtpClient.Port = smtp.Port;
                smtpClient.EnableSsl = smtp.EnableSsl;
                if (!string.IsNullOrEmpty(smtp.UserName))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(smtp.UserName, smtp.Password);
                }

                smtpClient.Send(message);
            }
            catch (Exception e)
            {
                exception = e;
            }
            return ReturnActionResult(null, exception);
        }
        protected virtual string EvaluateStringFormulas(string formulas, HttpRequestBase request)
        {
            if (string.IsNullOrEmpty(formulas))
            {
                return null;
            }
            var matches = Regex.Matches(formulas, "{(?<Name>[^{^}]+)}");
            var s = formulas;
            foreach (Match match in matches)
            {
                var value = request[match.Groups["Name"].Value];
                s = s.Replace(match.Value, value == null ? "" : value.ToString());
            }
            return s;
        }
    }
}
