#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
namespace Kooboo.CMS.Sites.Models
{
    public static class ModelExtensions
    {
        public static Repository GetRepository(this Site site)
        {
            site = site.AsActual();
            if (site != null && !string.IsNullOrEmpty(site.Repository))
            {
                return new Repository(site.Repository);
            }
            return null;
        }

        public static Kooboo.CMS.Membership.Models.Membership GetMembership(this Site site)
        {
            site = site.AsActual();

            if (site != null && !string.IsNullOrEmpty(site.Membership))
            {
                return new Kooboo.CMS.Membership.Models.Membership(site.Membership).AsActual();
            }
            return null;
        }

        public static void SendMailToCustomer(this Site site, string to, string subject, string body, bool isBodyHtml, HttpFileCollectionBase files = null)
        {
            var smtp = site.Smtp;
            if (smtp == null)
            {
                throw new ArgumentNullException("smtp");
            }

            MailMessage message = new MailMessage() { From = new MailAddress(smtp.From) };
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isBodyHtml;
            SmtpClient smtpClient = smtp.ToSmtpClient();

            smtpClient.Send(message);
        }

        public static void SendMailToSiteManager(this Site site, string from, string subject, string body, bool isBodyHtml, HttpFileCollectionBase files = null)
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


            message.IsBodyHtml = isBodyHtml;

            if (files != null && files.Count > 0)
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
    }
}
