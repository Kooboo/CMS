#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.ABTest;
using Kooboo.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(IPVisitRule))]
    public class IPVisitRule_Metadata
    {
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name="Regex pattern")]
        [Description("The ip regular expression pattern.")]
        public string RegexPattern { get; set; }

    }
}