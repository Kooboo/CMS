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
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Sites.Models;
using Kooboo.ComponentModel;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(PathResource))]
    public class PathResource_Metadata
    {
        [Required()]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        public string Name { get; set; }
    }
}