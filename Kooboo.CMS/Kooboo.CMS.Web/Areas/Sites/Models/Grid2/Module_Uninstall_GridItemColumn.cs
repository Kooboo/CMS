#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Grid2;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;

namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class Module_Uninstall_GridItemColumn : GridItemColumn
    {
        public Module_Uninstall_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            var data = (ModuleInfo)DataItem;

            var linkText = "Uninstall".Localize();

            return viewContext.HtmlHelper().ActionLink(linkText, "Uninstall", viewContext.RequestContext.AllRouteValues().Merge("ModuleName", data.ModuleName),
                new Dictionary<string, object> { { "class", "o-icon delete dialog-link" } });

        }
    }
}