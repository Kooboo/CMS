using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Web.Areas.Sites.Menus
{
    public class ContentFolderMenuItemInitializer : Kooboo.Web.Mvc.Menu.DefaultMenuItemInitializer
    {
        public override Kooboo.Web.Mvc.Menu.MenuItem Initialize(Kooboo.Web.Mvc.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            var site = Site.Current;
            if (site!=null)
            {
                menuItem.RouteValues["repositoryName"] = site.AsActual().Repository;
            }
            
            return base.Initialize(menuItem, controllerContext);
        }
    }
}