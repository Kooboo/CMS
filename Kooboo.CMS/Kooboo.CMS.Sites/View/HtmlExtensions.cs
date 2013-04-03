using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.View
{
    public static class HtmlExtensions
    {
        public static FrontHtmlHelper FrontHtml(this HtmlHelper html)
        {
            return new FrontHtmlHelper(Page_Context.Current, html);
        }
    }
}
