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
using Kooboo.CMS.Web.Areas.Contents.Menu;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Menu
{
    public class MembershipMenuItems : IMenuItemContainer
    {
        #region IMenuItemContainer Members

        public IEnumerable<MenuItem> GetItems(string areaName, ControllerContext controllerContext)
        {
            var siteName = controllerContext.RequestContext.GetRequestValue("siteName");
            if (!string.IsNullOrEmpty(siteName))
            {
                var site = SiteHelper.Parse(siteName).AsActual();
                if (site != null)
                {
                    var membershipName = site.Membership;
                    if (!string.IsNullOrEmpty(membershipName))
                    {
                        areaName = "Membership";
                        var items = MenuFactory.BuildMenu(controllerContext, areaName, false).Items;
                        ResetRouteValues(siteName, membershipName, areaName, items);
                        return items;
                    }
                }
            }
            return new MenuItem[0];


        }

        private static void ResetRouteValues(string siteName, string membershipName, string areaName, IList<MenuItem> items)
        {
            foreach (var item in items)
            {
                ResetRouteValues(siteName, membershipName, areaName, item.Items);

                item.RouteValues["membershipName"] = membershipName;
                item.RouteValues["siteName"] = siteName;
                if (string.IsNullOrEmpty(item.Area))
                {
                    item.Area = areaName;
                }
            }
        }

        #endregion
    }
}