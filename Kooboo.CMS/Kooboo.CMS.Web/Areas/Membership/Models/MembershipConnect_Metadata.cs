#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Member.Models;
using Kooboo.CMS.Web.Areas.Membership.Models.DataSources;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web.Areas.Membership.Models
{
    [MetadataFor(typeof(MembershipConnect))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class MembershipConnect_Metadata
    {
        [GridColumn(Order = 1, GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]
        [RemoteEx("IsNameAvailable", "MembershipConnect", RouteFields = "MembershipName")]
        [Required(ErrorMessage = "Required")]
        [UIHint("DropDownList")]
        [DataSource(typeof(AuthClientDataSource))]
        public virtual string Name { get; set; }


        [GridColumn(Order = 2, HeaderText = "Display name", GridColumnType = typeof(SortableGridColumn))]
        public virtual string DisplayName { get; set; }

        [GridColumn(Order = 3, HeaderText = "App id", GridColumnType = typeof(SortableGridColumn))]
        public virtual string AppId { get; set; }

        [GridColumn(Order = 4, HeaderText = "App secret", GridColumnType = typeof(SortableGridColumn))]
        public virtual string AppSecret { get; set; }

        [UIHint("Dictionary")]
        public virtual Dictionary<string, string> Options { get; set; }


        [DisplayName("Membership groups")]
        [UIHint("Multiple_DropDownList")]
        [DataSource(typeof(MembershipGroupDataSource))]
        public virtual string[] MembershipGroups { get; set; }
    }
}
