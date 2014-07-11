#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Menu
{
    public class ContentFolderMenuItemInitializer : Kooboo.Common.Web.Menu.DefaultMenuItemInitializer
    {
        public override Kooboo.Common.Web.Menu.MenuItem Initialize(Kooboo.Common.Web.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            var site = Site.Current;
            if (site != null)
            {
                menuItem.RouteValues["repositoryName"] = site.AsActual().Repository;
            }

            return base.Initialize(menuItem, controllerContext);
        }
    }
}