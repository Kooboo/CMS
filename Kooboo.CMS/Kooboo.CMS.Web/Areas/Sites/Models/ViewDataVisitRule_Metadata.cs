#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.VisitRule;
using Kooboo.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(ViewDataVisitRule))]
    public class ViewDataVisitRule_Metadata
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Field name")]
        [Description("The data name which store in the ViewData.")]
        public string FieldName { get; set; }
        public string Value { get; set; }
    }
}
