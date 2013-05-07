#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Web.Authorizations;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Web.Areas.Sites.Menus
{
    public class StartMenuItemInitializer : AuthorizeMenuItemInitializer
    {
        protected override bool GetIsVisible(Kooboo.Web.Mvc.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            if (Site.Current != null)
            {
                return Site.Current.ShowSitemap.HasValue ? Site.Current.ShowSitemap.Value : true;
            }
            return false;
        }
    }
}