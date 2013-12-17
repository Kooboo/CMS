using Kooboo.CMS.Sites.Extension.ModuleArea.Management;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class InstallationContext_SourceVersion_Column : GridItemColumn
    {
        public InstallationContext_SourceVersion_Column(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        { }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            var installationContext = (InstallationContext)this.DataItem;
            if (installationContext.VersionRange.SourceVersion != null)
            {
                return new HtmlString(installationContext.VersionRange.SourceVersion);
            }
            else
            {
                return new HtmlString("-");
            }
        }
    }
}