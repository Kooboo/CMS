using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.View
{
    public static class UrlExtensions
    {
        public static FrontUrlHelper FrontUrl(this UrlHelper url)
        {
            return Page_Context.Current.FrontUrl;
        }
    }
}
