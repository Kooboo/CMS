#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.Common.Misc;
using Kooboo.Common.Web.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web2.Areas.Contents.Models
{   
    public class CreateRepositoryModel
    {
        [Required(ErrorMessage = "Required")]
        [Remote("IsNameAvailable", "Repository")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Name { get; set; }

        [UIHint("DropDownList")]
        [DataSource(typeof(Kooboo.CMS.Web2.Areas.Contents.Models.DataSources.RepositoryTemplatesDataSource))]
        public string Template { get; set; }
    }
}