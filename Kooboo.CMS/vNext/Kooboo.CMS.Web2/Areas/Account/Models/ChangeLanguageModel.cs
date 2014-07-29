#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Web2.Areas.Account.Models.DataSources;
using Kooboo.Common.Web.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web2.Areas.Account.Models
{
    public class ChangeLanguageModel
    {
        [Display(Name = "UI culture")]
        [UIHint("DropDownList")]
        [DataSource(typeof(EnabledLanguagesDataSource))]
        public string UICulture { get; set; }
    }
}