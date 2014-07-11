#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.Web.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using Kooboo.Common.Globalization;
using System.Web.Mvc.Html;
using Kooboo.Common.Web.Grid;
namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class Label_Value_GridItemColumn : GridItemColumn
    {
        public Label_Value_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }

        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            return viewContext.HtmlHelper().Partial("_ValueGridItemColumn", this.DataItem);
        }
    }
}
