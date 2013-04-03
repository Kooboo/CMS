using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Web.Mvc.Html
{
    public static class TagBuilderUtility
    {
        public static T Build<T>(this HtmlHelper html) where T:IHtmlControl,new()
        {
            return new T();
        }
    }
}
