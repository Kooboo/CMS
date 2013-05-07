#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Account.Models;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Account.Models
{
    public class EditUserModel
    {
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.EmailAddress, ErrorMessage = "Invalid email address")]
        public string Email { get; set; }


        [Display(Name = "Is administrator")]
        public bool IsAdministrator { get; set; }
        [Display(Name = "Is locked out")]
        public bool IsLockedOut { get; set; }

        [UIHintAttribute("DropDownList")]
        [DataSource(typeof(CultureSelectListDataSource))]
        [Description("The culture represents the current culture used by the Resource Manager to look up culture-specific resources at run time.")]
        public string UICulture { get; set; }

        public EditUserModel()
        {

        }
        public EditUserModel(User user)
        {
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.IsAdministrator = user.IsAdministrator;
            this.IsLockedOut = user.IsLockedOut;
            this.UICulture = user.UICulture;
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
                user.LastLockoutDate = DateTime.UtcNow;
            }
            user.UICulture = this.UICulture;
            return user;
        }
    }
}