#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.CMS.Web.Grid2;
using Kooboo.Common.Globalization;

using Kooboo.Common.Web.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Web.Routing;
namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class Version_Preview_GridItemColumn : GridItemColumn
    {
        public Version_Preview_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            var data = (VersionInfo)DataItem;

            var linkText = "Preview".Localize();

            return viewContext.HtmlHelper().ActionLink(linkText, "PreviewVersion", viewContext.RequestContext.AllRouteValues().Merge("Version", data.Version),
                new Dictionary<string, object> { { "class", "o-icon preview dialog-link" } });
        }
    }
}