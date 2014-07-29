#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.Web.Grid.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Common.Web.Grid;

namespace Kooboo.CMS.Web2.Grid2
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