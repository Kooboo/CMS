using Kooboo.CMS.Sites.Extension.ModuleArea.Management;
using Kooboo.Common.Web.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class InstallationContext_TargetVersion_Column : GridItemColumn
    {
        public InstallationContext_TargetVersion_Column(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        { }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            var installationContext = (InstallationContext)this.DataItem;
            if (installationContext.VersionRange.TargetVersion != null)
            {
                return new HtmlString(installationContext.VersionRange.TargetVersion);
            }
            else
            {
                return new HtmlString("-");
            }
        }
    }
}