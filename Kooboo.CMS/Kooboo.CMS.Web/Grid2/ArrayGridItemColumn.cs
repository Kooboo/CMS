#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Grid2
{
    public class ArrayGridItemColumn : GridItemColumn
    {
        public ArrayGridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            if (PropertyValue != null)
            {
                return new HtmlString(string.Join(",", ((IEnumerable<string>)PropertyValue).ToArray()));
            }
            return new HtmlString("");
        }
    }
}