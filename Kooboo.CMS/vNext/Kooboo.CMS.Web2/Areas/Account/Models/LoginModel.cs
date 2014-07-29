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

namespace Kooboo.CMS.Web2.Areas.Account.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Required")]
        [UIHint("Password")]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
        [Display(Name = "Redirect to home page")]
        public bool RedirectToHome { get; set; }
    }
}