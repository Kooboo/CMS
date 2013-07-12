#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Member.Models;
using Kooboo.CMS.Web.Areas.Membership.Models.DataSources;
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web.Areas.Membership.Models
{
    [MetadataFor(typeof(Kooboo.CMS.Member.Models.Membership))]
    public class Membership_Metadata
    {
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        [Required(ErrorMessage = "Required")]
        [RemoteEx("IsNameAvailable", "Membership")]
        public string Name { get; set; }

        [Display(Name = "Auth cookie name")]
        [Description("The cookie name of form authentication. If it is empty, will use the site name as the cookie name.")]
        public string AuthCookieName { get; set; }
        [Display(Name = "Auth cookie domain")]
        [Description("The cookie domain form authentication. You can set the cookie domain to share the auth cookie between the sites have a common sub-domain. for example: domain.com")]
        public string AuthCookieDomain { get; set; }

        [Display(Name = "Hash algorithm type")]
        [UIHint("DropDownList")]
        [DataSource(typeof(HashAlgorithmTypeDataSource))]
        [Description("The identifier of the algorithm used to hash passwords.")]
        public string HashAlgorithmType { get; set; }

        [Display(Name = "Max invalid password attempts")]
        [Description("Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.")]
        public int MaxInvalidPasswordAttempts { get; set; }

        [Display(Name = "Min required password length")]
        [Description("Gets the minimum length required for a password.")]
        public int MinRequiredPasswordLength { get; set; }

        [Display(Name = "Password strength regular expression")]
        [Description("Gets the regular expression used to evaluate a password.")]
        public string PasswordStrengthRegularExpression { get; set; }
    }
}
