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
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(QueryStringVisitRule))]
    public class QueryStringVisitRule_Metadata
    {
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Query string key")]
        [Description("The key of query string in URL.")]
        public string QueryName { get; set; }
        [Display(Name = "Query string value")]
        public string QueryValue { get; set; }
    }
}
