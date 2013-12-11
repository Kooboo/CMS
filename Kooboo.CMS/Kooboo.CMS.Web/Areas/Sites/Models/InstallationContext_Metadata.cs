using Kooboo.CMS.Sites.Extension.ModuleArea.Management;
using Kooboo.CMS.Web.Areas.Sites.Models.Grid2;
using Kooboo.CMS.Web.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(InstallationContext))]
    [Grid(Checkable = true, IdProperty = "VersionRange")]
    public class InstallationContext_Metadata
    {
        [GridColumn(Order = 1, HeaderText = "Source version", GridItemColumnType = typeof(InstallationContext_SourceVersion_Column))]
        [GridColumn(Order = 2, HeaderText = "Target version", GridItemColumnType = typeof(InstallationContext_TargetVersion_Column))]
        public VersionRange VersionRange { get; set; }

        [GridColumn(Order = 3)]
        public string User { get; set; }
        [GridColumn(HeaderText = "Datetime", Order = 4, GridItemColumnType = typeof(DateTimeGridItemColumn))]
        public DateTime UtcDatetime { get; set; }
    }
}