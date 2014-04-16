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
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Areas.Sites.Models.Grid2;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(VersionInfo))]
    [Grid(Checkable = true, IdProperty = "Version")]
    public class VersionInfo_Metadata
    {
        [GridColumn()]
        public int Version { get; set; }
        [GridColumn(GridItemColumnType = typeof(DateTimeGridItemColumn))]
        public DateTime Date { get; set; }
        [GridColumn(HeaderText = "Commit user")]
        public string UserName { get; set; }
    }
}