using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Web.Authorizations;
using Kooboo.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Contents.Menus
{
    public class ContentAuthorizeMenuItemInitializer : AuthorizeMenuItemInitializer
    {
        protected override bool GetIsVisible(Kooboo.Web.Mvc.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            if (string.IsNullOrEmpty(controllerContext.RequestContext.GetRequestValue("repositoryName")) &&
                string.IsNullOrEmpty(controllerContext.RequestContext.GetRequestValue("siteName")))
            {
                return false;
            }
            return base.GetIsVisible(menuItem, controllerContext);
        }
    }
}