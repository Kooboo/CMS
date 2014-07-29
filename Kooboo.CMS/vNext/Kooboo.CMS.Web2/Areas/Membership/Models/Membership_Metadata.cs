#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Web2.Areas.Membership.Models.DataSources;
using Kooboo.CMS.Web2.Models;
using Kooboo.Common.ComponentModel;
using Kooboo.Common.Misc;
using Kooboo.Common.Web.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web2.Areas.Membership.Models
{
    [MetadataFor(typeof(Kooboo.CMS.Membership.Models.Membership))]
    public class Membership_Metadata
    {
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        [Required(ErrorMessage = "Required")]
        [RemoteEx("IsNameAvailable", "Membership")]
        public string Name { get; set; }

        [Display(Name = "Auth cookie name")]
        [Description("The form authentication cookie name. If it is empty, it will use the site name as the cookie name. <br >If you want to share the same cookie name and value across different websites, you need to use the same cookie name.")]
        public string AuthCookieName { get; set; }
        [Display(Name = "Auth cookie domain")]
        [Description("The form authentication cookie domain. You can set cookies from sub-domains to use the top domain, in order to share the cookie values across sub domain sites.")]
        public string AuthCookieDomain { get; set; }

        [Display(Name = "Hash algorithm type")]
        [UIHint("DropDownList")]
        [DataSource(typeof(HashAlgorithmTypeDataSource))]
        [Description("The identifier of the algorithm used to hash passwords.")]
        public string HashAlgorithmType { get; set; }

        [Display(Name = "Max invalid password attempts")]
        [Description("Set the number of invalid password attempts allowed before the membership user is locked out.")]
        public int MaxInvalidPasswordAttempts { get; set; }

        [Display(Name = "Min required password length")]
        [Description("Set the minimum length required for a password.")]
        public int MinRequiredPasswordLength { get; set; }

        [Display(Name = "Password strength regular expression")]
        [Description("Set the regular expression used to evaluate a password. Let it blank if you do not want to use it")]
        public string PasswordStrengthRegularExpression { get; set; }       
    }
}
