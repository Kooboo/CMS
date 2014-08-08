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
using System.Web.Routing;
using Kooboo.CMS.Web.Authorizations;

namespace Kooboo.CMS.Web.Areas.Sites.Menu
{
    public class SiteAuthorizeMenuItemInitializer : AuthorizeMenuItemInitializer
    {
        protected override bool GetIsVisible(Kooboo.Common.Web.Menu.MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            if (string.IsNullOrEmpty(controllerContext.RequestContext.GetRequestValue("siteName")))
            {
                return false;
            }
            return base.GetIsVisible(menuItem, controllerContext);
        }
    }
}