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
using Kooboo.Common.Web.Menu;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites
{
    public static class MenuExtensions
    {
        #region SetCurrentSite
        public static Kooboo.Common.Web.Menu.Menu SetCurrentSite(this Kooboo.Common.Web.Menu.Menu menu, ViewContext viewContext)
        {
            var siteName = viewContext.RequestContext.GetRequestValue("siteName");
            if (siteName != null)
            {
                foreach (var item in menu.Items)
                {
                    SetCurrentSite(item, siteName.ToString());
                }
            }
            return menu;
        }
        private static void SetCurrentSite(this MenuItem menuItem, string siteName)
        {
            menuItem.RouteValues["siteName"] = siteName;
            if (menuItem.Items != null)
            {
                foreach (var item in menuItem.Items)
                {
                    if (item.Items != null)
                    {
                        SetCurrentSite(item, siteName);
                    }
                }
            }
        }
        #endregion

        #region SetCurrentRepository
        public static Kooboo.Common.Web.Menu.Menu SetCurrentRepository(this Kooboo.Common.Web.Menu.Menu menu, ViewContext viewContext)
        {
            var repositoryName = viewContext.RequestContext.GetRequestValue("repositoryName");
            if (repositoryName != null)
            {
                foreach (var item in menu.Items)
                {
                    SetCurrentRepository(item, repositoryName.ToString());
                }
            }
            return menu;
        }
        private static void SetCurrentRepository(this MenuItem menuItem, string repositoryName)
        {
            menuItem.RouteValues["repositoryName"] = repositoryName;
            if (menuItem.Items != null)
            {
                foreach (var item in menuItem.Items)
                {
                    if (item.Items != null)
                    {
                        SetCurrentRepository(item, repositoryName);
                    }
                }
            }
        }

        #endregion
    }
}