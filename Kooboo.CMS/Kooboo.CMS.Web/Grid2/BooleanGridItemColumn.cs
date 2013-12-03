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
using Kooboo.Globalization;

namespace Kooboo.CMS.Web.Grid2
{
    public class BooleanGridItemColumn : GridItemColumn
    {
        public BooleanGridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            var tip = "-".Localize();
            if (PropertyValue != null && ((bool)PropertyValue))
            {
                tip = "YES".Localize();
            }
            return new HtmlString(tip);
            //return new HtmlString(string.Format(@"<a href='javascript:;' title='{1}'><span class=""o-icon {0}""></span></a>", GetIconClass(PropertyValue), tip));
        }
    }
}