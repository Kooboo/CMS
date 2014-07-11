#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Web.Models;
using Kooboo.Common.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class SiteTemplate_ViewModel
    {
        public string Category { get; set; }

        [DisplayName("Name")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [Required(ErrorMessage = "Required")]
        [RemoteEx("IsNameAvailable", "SiteTemplate", AdditionalFields = "Category")]
        public string TemplateName { get; set; }

        [DisplayName("File")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(".+\\.(zip)$", ErrorMessage = "Requires a zip file.")]
        [UIHint("File")]
        [Description("Only .zip file is accepted.")]
        public HttpPostedFileBase TemplateFile { get; set; }

        [DisplayName("Thumbnail")]
        [RegularExpression(".+\\.(png)$", ErrorMessage = "Requires a png file.")]
        [UIHint("File")]
        [Description("Only .png file is accepted.(100 * 100)")]
        public HttpPostedFileBase ThumbnailFile { get; set; }
    }
}