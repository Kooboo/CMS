#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Web.Mvc.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Grid2
{
    public class ActionGridColumn : GridColumn
    {
        public ActionGridColumn(GridModel gridModel, GridColumnAttribute att, string propertyName, int order)
            : base(gridModel, att, propertyName, order)
        {

        }
        public override IHtmlString RenderHeaderContainerAtts(ViewContext viewContext)
        {
            return new HtmlString("class='action'");
        }
    }
}