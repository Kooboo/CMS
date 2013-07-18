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

namespace Kooboo.CMS.Sites.Member
{
    public class ChangeMemberPasswordModel
    {
        [Required(ErrorMessage = "Required")]
        public virtual string OldPassword { get; set; }

        [Required(ErrorMessage = "Required")]
        public virtual string NewPassword { get; set; }

        [Compare("NewPassword")]
        [Required(ErrorMessage = "Required")]
        public virtual string ConfirmPassword { get; set; }
    }
}
