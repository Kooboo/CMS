#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Common.Web.Grid;

namespace Kooboo.CMS.Web2.Grid2
{
    public class DateTimeGridItemColumn : GridItemColumn
    {
        public DateTimeGridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            string s = "-";
            if (PropertyValue != null)
            {
                var dateTime = ((DateTime)PropertyValue);
                if (dateTime == DateTime.MinValue)
                {
                    s = "-";
                }
                else
                {
                    s = dateTime.ToLocalTime().ToString();
                }
            }
            return new HtmlString(s);
        }
    }
}