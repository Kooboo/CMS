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
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
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
        [DisplayName("Require member")]
        public bool RequireMember { get; set; }
        [DisplayName("Allow groups")]
        [UIHint("Multiple_DropDownList")]
        [DataSource(typeof(SiteMembershipGroupDataSource))]
        public string[] AllowGroups { get; set; }

        [DisplayName("Authorize menu")]
        public bool AuthorizeMenu { get; set; }

        [DisplayName("Unauthorized URL")]
        [UIHint("AutoComplete")]
        [DataSource(typeof(AutoCompletePageListDataSouce))]
        public string UnauthorizedUrl { get; set; }
    }
}
