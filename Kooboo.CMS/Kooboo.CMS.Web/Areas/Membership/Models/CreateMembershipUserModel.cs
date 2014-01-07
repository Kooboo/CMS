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
    public class CreateMembershipUserModel : EditMembershipUserModel
    {
        [UIHint("Password")]
        [Required(ErrorMessage = "Required")]
        public virtual string Password { get; set; }

        [Compare("Password")]
        [UIHint("Password")]
        [Required(ErrorMessage = "Required")]
        [DisplayName("Confirm password")]
        public virtual string ConfirmPassword { get; set; }

    }
}
