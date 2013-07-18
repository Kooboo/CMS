#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Member.Models;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web.Areas.Membership.Models
{
    [MetadataFor(typeof(MembershipGroup))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class MembershipGroup_Metadata
    {
        [GridColumn(Order = 1, GridColumnType = typeof(SortableGridColumn))]
        [RemoteEx("IsNameAvailable", "MembershipGroup", RouteFields = "MembershipName")]
        [Required(ErrorMessage = "Required")]
        public virtual string Name { get; set; }
    }
}
