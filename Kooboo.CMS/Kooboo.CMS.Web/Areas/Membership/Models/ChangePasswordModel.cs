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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Membership.Models
{
    public class ChangePasswordModel
    {
        public string UUID { get; set; }
        [DisplayName("New password")]
        [UIHint("Password")]
        [Required(ErrorMessage = "Required")]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        [UIHint("Password")]
        [Required(ErrorMessage = "Required")]
        [DisplayName("Confirm password")]
        public string ConfirmNewPassword { get; set; }
    }
}
