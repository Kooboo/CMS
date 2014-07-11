#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.Common.ComponentModel;
using Kooboo.Common.Web.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(PagePermission))]
    public class PagePermission_Metadata
    {
        [DisplayName("Require membership authentication")]
        public bool RequireMember { get; set; }
        [DisplayName("Allow membership groups")]
        [UIHint("Multiple_DropDownList")]
        [DataSource(typeof(SiteMembershipGroupDataSource))]
        public string[] AllowGroups { get; set; }

        [DisplayName("Apply to menu generation")]
        public bool AuthorizeMenu { get; set; }

        [DisplayName("Unauthorized redirect URL")]
        [UIHint("AutoComplete")]
        [DataSource(typeof(AutoCompletePageListDataSouce))]
        public string UnauthorizedUrl { get; set; }
    }
}
