#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Versioning;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Models;
using Kooboo.Common.ComponentModel;

using Kooboo.Common.Web.Grid.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    [MetadataFor(typeof(VersionInfo))]
    [Grid(Checkable = true, IdProperty = "Version")]
    public class VersionInfo_Metadata
    {
        [GridColumn]
        public int Version { get; set; }

        [GridColumn(HeaderText = "Commit user")]
        public string CommitUser { get; set; }

        [GridColumn(HeaderText = "Commit date", GridItemColumnType = typeof(DateTimeGridItemColumn))]
        public DateTime UtcCommitDate { get; set; }

    }
}