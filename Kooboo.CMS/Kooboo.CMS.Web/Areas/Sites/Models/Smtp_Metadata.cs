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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Kooboo.Common.ComponentModel;
using Kooboo.CMS.Sites.Models;
namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(Smtp))]
    public class Smtp_Metadata
    {
        [DisplayName("SMTP Server")]
        public string Host { get; set; }
        public string UserName { get; set; }
        [UIHint("Password")]
        public string Password { get; set; }
        [DefaultValue(25)]
        [Required(ErrorMessage = "Required")]
        public int Port { get; set; }
        [DisplayName("Enable SSL")]
        [Required(ErrorMessage = "Required")]
        public bool EnableSsl { get; set; }
        [UIHint("Array")]
        [DisplayName("To address")]
        [Description("The receiver's Email addresses of your contact form")]
        public string[] To { get; set; }
        [DisplayName("From address")]
        [Description("The FROM email address of emails sent out by the system. For example: user registration confirmation email.")]
        public string From { get; set; }
    }
}