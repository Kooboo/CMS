#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Web2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web2.Areas.Membership.Models
{
    public class ImportModel
    {
        [Required(ErrorMessage = "Required")]
        [RemoteEx("IsNameAvailable", "Membership")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Required")]
        [RegularExpression(".+\\.(zip)$", ErrorMessage = "Required a zip file.")]
        [UIHint("File")]
        [Description("Only .zip file is accepted.")]
        public string File { get; set; }
    }
}
