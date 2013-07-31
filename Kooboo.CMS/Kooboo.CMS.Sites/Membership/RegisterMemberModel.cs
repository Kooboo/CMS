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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Membership
{
    public class RegisterMemberModel
    {
        [Required(ErrorMessage = "Required")]
        public virtual string UserName { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.EmailAddress, ErrorMessage = "Invalid email address")]
        public virtual string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        public virtual string Password { get; set; }

        [Compare("Password")]
        [Required(ErrorMessage = "Required")]
        public virtual string ConfirmPassword { get; set; }

        public virtual bool IsApproved { get; set; }

        public virtual string PasswordQuestion { get; set; }

        public virtual string PasswordAnswer { get; set; }

        public virtual string Culture { get; set; }

        public virtual string TimeZoneId { get; set; }

        public virtual Dictionary<string, string> Profiles { get; set; }

        public virtual string RedirectUrl { get; set; }

        public virtual string EmailSubject { get; set; }

        public virtual string EmailBody { get; set; }

        public virtual string ActivateUrl { get; set; }
    }
}
