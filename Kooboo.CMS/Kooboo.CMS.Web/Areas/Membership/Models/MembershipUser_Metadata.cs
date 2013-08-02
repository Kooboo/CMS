#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Web.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web.Areas.Membership.Models
{
    [MetadataFor(typeof(MembershipUser))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class MembershipUser_Metadata
    {
        [GridColumn(Order = 1, HeaderText = "User name", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]
        public virtual string UserName { get; set; }

        [GridColumn(Order = 2, GridColumnType = typeof(SortableGridColumn))]
        public virtual string Email { get; set; }

        [GridColumn(Order = 3, HeaderText = "Provider type", GridColumnType = typeof(SortableGridColumn))]
        public virtual string ProviderType { get; set; }

        //[GridColumn(Order = 4, HeaderText = "Provider user id", GridColumnType = typeof(SortableGridColumn))]
        public virtual string ProviderUserId { get; set; }

        [GridColumn(Order = 5, HeaderText = "Create date", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        public virtual DateTime UtcCreationDate { get; set; }

        [GridColumn(Order = 6, HeaderText = "Is approved", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(BooleanGridItemColumn))]
        public virtual bool IsApproved { get; set; }

        [GridColumn(Order = 7, HeaderText = "Is locked out", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(BooleanGridItemColumn))]
        public virtual bool IsLockedOut { get; set; }


        [GridColumn(Order = 8, HeaderText = "Login date", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        public virtual DateTime UtcLastLoginDate { get; set; }

        //[GridColumn(Order = 9, HeaderText = "Password change date", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        public virtual DateTime UtcLastPasswordChangedDate { get; set; }

        //[GridColumn(Order = 10, HeaderText = "Lock out date", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        public virtual DateTime UtcLastLockoutDate { get; set; }

        public virtual string PasswordQuestion { get; set; }

        public virtual string PasswordAnswer { get; set; }

        public virtual string Culture { get; set; }

        public virtual string TimeZoneId { get; set; }

        public virtual string Comment { get; set; }

        public virtual Dictionary<string, string> Profiles { get; set; }

        public virtual Dictionary<string, string> ProviderExtraData { get; set; }
    }
}
