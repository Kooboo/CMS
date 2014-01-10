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
using Kooboo.CMS.Account.Models;
using Kooboo.Web.Mvc;
using System.ComponentModel;
using System.Web.Mvc;
using Kooboo.CMS.Web.Areas.Account.Models.DataSources;

namespace Kooboo.CMS.Web.Areas.Account.Models
{

    public class CreateUserModel
    {
        [DisplayName("User name")]
        [System.Web.Mvc.Remote("IsNameAvailable", "Users")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A user name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.EmailAddress, ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [UIHint("PasswordStrength")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Is administrator")]
        public bool IsAdministrator { get; set; }
        [Display(Name = "Is locked out")]
        public bool IsLockedOut { get; set; }

        [UIHintAttribute("DropDownList")]
        [DataSource(typeof(CultureSelectListDataSource))]
        [Description("The culture represents the current culture used by the Resource Manager to look up culture-specific resources at run time.")]
        public string UICulture { get; set; }

        [DisplayName("Global roles")]
        [UIHint("Multiple_DropDownList")]
        [DataSource(typeof(RolesDatasource))]
        public string[] GlobalRoles { get; set; }

        public CreateUserModel()
        {

        }
        public CreateUserModel(User user)
        {
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.Password = user.Password;
            this.IsAdministrator = user.IsAdministrator;
            this.IsLockedOut = user.IsLockedOut;
            this.UICulture = user.UICulture;
            this.GlobalRoles = string.IsNullOrEmpty(user.GlobalRoles) ? null : user.GlobalRoles.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }
        public User ToUser()
        {
            User user = new User();
            user.UserName = this.UserName;
            user.Email = this.Email;
            user.IsAdministrator = this.IsAdministrator;
            user.IsLockedOut = this.IsLockedOut;
            if (user.IsLockedOut == true)
            {
                user.UtcLastLockoutDate = DateTime.UtcNow;
            }
            user.Password = this.Password;
            user.UICulture = this.UICulture;
            if (this.GlobalRoles != null)
            {
                user.GlobalRoles = string.Join(",", this.GlobalRoles);
            }
            return user;
        }
    }
}