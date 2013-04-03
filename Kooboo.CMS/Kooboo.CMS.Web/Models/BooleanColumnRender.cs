using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Models
{
    public class BooleanColumnRender : IItemColumnRender
    {

        public virtual IHtmlString Render(object dataItem, object value, ViewContext viewContext)
        {
            return new HtmlString(string.Format(@"<span class=""o-icon {0}""></span>", GetIconClass(value)));
        }

        protected virtual string GetIconClass(object value)
        {
            var @class = "cross";
            if (value != null && Convert.ToBoolean(value) == true)
            {
                @class = "tick";
            }
            return @class;
        }
    }
}