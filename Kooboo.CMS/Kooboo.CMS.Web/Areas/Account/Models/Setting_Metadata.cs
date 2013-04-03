using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Kooboo.CMS.Web.Areas.Account.Models
{
    public class Setting_Metadata
    {
        [Display(Name = "Password strength")]
        [Description("The regular expression for valid the password strength.")]
        public string PasswordStrength { get; set; }

        [Display(Name = "Invalid message")]
        [Description("The message will be showed when password is invalid.")]
        public string PasswordInvalidMessage { get; set; }
        [Display(Name = "Enable lockout")]
        public bool EnableLockout { get; set; }
        [Display(Name = "Failed count to lock")]
        public int FailedPasswordAttemptCount { get; set; }
        [Display(Name = "Minutes to unlock")]
        public int MinutesToUnlock { get; set; }
    }
}