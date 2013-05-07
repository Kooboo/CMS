#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Account.Services;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.Extensions;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Kooboo.CMS.Web.Areas.Sites.Models
{

    [MetadataFor(typeof(Kooboo.CMS.Sites.Models.User))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class User_Metadata
    {
        [GridColumn(Order = 1, GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]
        [DisplayName("User name")]
        [Description("Add an user to your website <br />The user must be created first.")]
        [Required(ErrorMessage = "Required")]        
        [UIHint("DropDownList")]
        [DataSource(typeof(UsersDatasource))]
        [RemoteEx("IsUserAvailable", "Users", RouteFields = "siteName", AdditionalFields = "old_Key")]
        public string UserName { get; set; }

        [GridColumn(Order = 2, GridItemColumnType = typeof(ArrayGridItemColumn))]
        [Required(ErrorMessage = "Required")]
        [UIHint("Multiple_DropDownList")]
        [DataSource(typeof(RolesDatasource))]        
        public List<string> Roles { get; set; }
    }
}