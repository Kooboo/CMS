﻿#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.ABTest;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [Kooboo.ComponentModel.MetadataFor(typeof(LanguageVisitRule))]
    public class LanguageVisitRule_Metadata
    {
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Language")]
        [UIHint("Dropdownlist")]
        [DataSource(typeof(CultureSelectListDataSource))]
        public string LanguageName { get; set; }
    }
}
