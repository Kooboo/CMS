#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using Kooboo.Common.Globalization;
using Kooboo.Common.Web.Grid;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
namespace Kooboo.CMS.Web2.Areas.Contents.Models.Grid2
{
    public class PendingWorflowItem_Edit_GridItemColumn : GridItemColumn
    {
        public PendingWorflowItem_Edit_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }

        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {

            var data = (PendingWorkflowItem)DataItem;
            var linkText = "Edit".Localize();
            var @class = "dialog-link";
            if (!string.IsNullOrEmpty(this.GridColumn.PropertyName))
            {
                linkText = this.PropertyValue == null ? "" : PropertyValue.ToString();
            }
            else
            {
                @class = "o-icon edit " + @class;
            }
            return viewContext.HtmlHelper().ActionLink(linkText, "Edit", viewContext.RequestContext.AllRouteValues()
                .Merge("Controller", "TextContent").Merge("FolderName", data.UUID).Merge("UUID", data.UUID)
                , new Dictionary<string, object> { { "class", @class } });

        }
    }
}