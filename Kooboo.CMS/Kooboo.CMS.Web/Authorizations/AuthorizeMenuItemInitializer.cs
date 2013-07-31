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
using Kooboo.Web.Mvc.Menu;
using System.Web.Routing;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Mvc;
using System.Security.Principal;

namespace Kooboo.CMS.Web.Authorizations
{
    public class AuthorizeMenuItemInitializer : DefaultMenuItemInitializer
    {
        protected override bool GetIsVisible(MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            var visible = base.GetIsVisible(menuItem, controllerContext);

            if (menuItem.ReadOnlyProperties != null)
            {
                var requiredAdministrator = menuItem.ReadOnlyProperties["requiredAdministrator"];
                if (!string.IsNullOrEmpty(requiredAdministrator) && requiredAdministrator.ToLower() == "true")
                {
                    return Kooboo.CMS.Sites.Services.ServiceFactory.UserManager.IsAdministrator(controllerContext.HttpContext.User.Identity.Name);
                }
                if (!string.IsNullOrEmpty(menuItem.ReadOnlyProperties["permissionName"]))
                {
                    var permission = new Kooboo.CMS.Account.Models.Permission()
                    {
                        AreaName = menuItem.ReadOnlyProperties["permissionArea"],
                        Group = menuItem.ReadOnlyProperties["permissionGroup"],
                        Name = menuItem.ReadOnlyProperties["permissionName"]
                    };
                    return controllerContext.RequestContext.Authorize(permission);
                }
            }
            return visible;
        }
    }
}