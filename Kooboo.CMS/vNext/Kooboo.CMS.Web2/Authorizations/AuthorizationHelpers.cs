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
using Kooboo.CMS.Account.Models;

using System.Security.Principal;

namespace Kooboo.CMS.Web2.Authorizations
{
    public static class AuthorizationHelpers
    {
        public static bool Authorize(this RequestContext requestContext, Permission permission)
        {
            IPrincipal user = requestContext.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }
            //todo: always return true.

            return true;
            //var site = GetSite(requestContext);

            //return Kooboo.CMS.Sites.Services.ServiceFactory.UserManager.Authorize(site, user.Identity.Name, permission);
        }
        //private static Site GetSite(RequestContext requestContext)
        //{
        //    var siteName = requestContext.GetRequestValue("siteName");
        //    if (!string.IsNullOrEmpty(siteName))
        //    {
        //        return SiteHelper.Parse(siteName);
        //    }
        //    return null;
        //}
    }
}