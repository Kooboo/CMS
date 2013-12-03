using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.Grid2
{
    public class ShortDateTimeGridItemColumnType : GridItemColumn
    {
        public ShortDateTimeGridItemColumnType(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            string s = "";
            if (PropertyValue != null)
            {
                var dateTime = ((DateTime)PropertyValue);
                if (dateTime == DateTime.MinValue)
                {
                    s = "-";
                }
                else
                {
                    s = dateTime.ToLocalTime().ToString("yy/MM/dd HH:mm");
                }
            }
            return new HtmlString(s);
        }
    }
}