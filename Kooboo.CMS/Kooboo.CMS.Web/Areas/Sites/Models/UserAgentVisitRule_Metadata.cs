#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.ABTest;
using Kooboo.Common.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(UserAgentVisitRule))]
    public class UserAgentVisitRule_Metadata
    {
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Regex pattern")]
        [UIHint("UserAgentPattern")]
        [Description("The UserAgent regular expression pattern.")]
        public string RegexPattern { get; set; }
    }
}
