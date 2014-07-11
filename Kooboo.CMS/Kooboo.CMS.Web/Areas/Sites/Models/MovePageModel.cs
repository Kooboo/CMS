#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.Common.Web.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class MovePageModel
    {
        public string UUID { get; set; }
        [DisplayName("Parent page")]
        [UIHint("DropDownList")]
        [DataSource(typeof(MovePageDataSource))]
        public string ParentPage { get; set; }
        [Display(Name = "Create automatic redirect")]
        public bool CreateRedirect { get; set; }
    }
}