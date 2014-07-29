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
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Models
{
    public class Smtp
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        private int port = 25;
        public int Port { get { return port; } set { port = value; } }
        public bool EnableSsl { get; set; }
        public string[] To { get; set; }
        public string From { get; set; }

        public SmtpClient ToSmtpClient()
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = this.Host;
            smtpClient.Port = this.Port;
            smtpClient.EnableSsl = this.EnableSsl;
            if (!string.IsNullOrEmpty(this.UserName))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(this.UserName, this.Password);
            }
            return smtpClient;
        }
    }
}
