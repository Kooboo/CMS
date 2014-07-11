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
using Kooboo.Common.Web.Grid;
namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class Page_Navigation_ShowInMenu_GridItemColumn : GridItemColumn
    {
        public Page_Navigation_ShowInMenu_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override System.Web.IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            Page page = (Page)this.DataItem;

            var tip = "-".Localize();
            if (page.Navigation.Show)
            {
                tip = "YES".Localize();
            }
            return new HtmlString(tip);
        }
    }
}
