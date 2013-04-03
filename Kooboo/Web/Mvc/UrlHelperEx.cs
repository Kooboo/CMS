using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace Kooboo.Web.Mvc
{
    public static class UrlHelperEx
    {
        public static string Action<TController>(this System.Web.Mvc.UrlHelper helper, Expression<Func<TController, ActionResult>> func) where TController : Controller
        {
            return new UrlHelperWrapper(helper).Action<TController>(func);
        }



       
    }
}
