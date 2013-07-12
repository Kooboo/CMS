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

namespace Kooboo.CMS.Sites.Member
{
    public class ActivateMemberModel
    {
        [Required(ErrorMessage="Required")]
        public string Member { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        public string SuccessUrl { get; set; }
        public string FailedUrl { get; set; }
    }
}
