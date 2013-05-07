#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using Kooboo.CMS.Web.Models;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Account.Services;
using System.Web.Mvc;
using Kooboo.ComponentModel;
using Kooboo.CMS.Account.Models;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.CMS.Web.Grid2;

namespace Kooboo.CMS.Web.Areas.Account.Models
{
    [MetadataFor(typeof(Role))]
    [Grid(Checkable = true, IdProperty = "Name")]
    public class Role_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [GridColumn(Order = 1, GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]
        public string Name { get; set; }
    }
}