#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Grid2;
using Kooboo.Common.Web.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Kooboo.Common.Globalization;
using Kooboo.Common.Web.Grid.Design;
using System.Web.Mvc;
using Kooboo.Common.Web.Grid;
namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class Page_Navigation_ShowInMenu_SortableGridColumn : GridColumn
    {
        public Page_Navigation_ShowInMenu_SortableGridColumn(GridModel gridModel, GridColumnAttribute att, string propertyName, int order)
            : base(gridModel, att, propertyName, order)
        {

        }
        public override IHtmlString RenderHeaderContainerAtts(ViewContext viewContext)
        {
            return new HtmlString("class='" + SortByExtension.RenderSortHeaderClass(viewContext.RequestContext, "Navigation.Show", this.Order).ToString() + "'");
        }

        public override IHtmlString RenderHeader(ViewContext viewContext)
        {
            return SortByExtension.RenderGridHeader(viewContext.RequestContext, base.RenderHeader(viewContext).ToString(), "Navigation.Show", this.Order);
        }
    }
}
