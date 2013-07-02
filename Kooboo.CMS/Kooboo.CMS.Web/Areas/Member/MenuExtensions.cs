#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Member
{
    public static class MenuExtensions
    {
        public static Menu SetMembership(this Menu menu, ViewContext viewContext)
        {
            var membership = viewContext.RequestContext.GetRequestValue("membershipName");
            var siteName = viewContext.RequestContext.GetRequestValue("siteName");
            if (membership != null)
            {
                foreach (var item in menu.Items)
                {
                    SetMembershipInRouteValues(item, siteName, membership.ToString());
                }
            }
            return menu;
        }
        private static void SetMembershipInRouteValues(this MenuItem menuItem, string siteName, string membership)
        {
            menuItem.RouteValues["siteName"] = siteName;
            menuItem.RouteValues["membershipName"] = membership;
            if (menuItem.Items != null)
            {
                foreach (var item in menuItem.Items)
                {

                    if (item.Items != null)
                    {
                        SetMembershipInRouteValues(item, siteName, membership);
                    }
                }
            }
        }
    }
}
