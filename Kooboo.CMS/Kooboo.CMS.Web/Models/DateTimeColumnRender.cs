using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;

namespace Kooboo.CMS.Web.Models
{
    public class DateTimeColumnRender : IItemColumnRender
    {
        public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            string s = "";
            if (value != null)
            {
                s = ((DateTime)value).ToLocalTime().ToString();
            }
            return new HtmlString(s);
        }
    }
}