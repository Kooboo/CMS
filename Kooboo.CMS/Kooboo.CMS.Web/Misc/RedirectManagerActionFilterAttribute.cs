using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Web.Misc
{
    public class RedirectManagerActionFilterAttribute : System.Web.Mvc.FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var _swithSite = filterContext.RequestContext.GetRequestValue("_swithSite");

            if (!string.IsNullOrEmpty(_swithSite) && _swithSite == "true" && Site.Current != null)
            {
                var url = filterContext.RequestContext.UrlHelper().Action("SiteMap", new { Area = "Sites", controller = "Home", siteName = Site.Current.FullName, repositoryName = Site.Current.Repository });
                filterContext.HttpContext.Response.Redirect(url);
            }
        }
    }
}