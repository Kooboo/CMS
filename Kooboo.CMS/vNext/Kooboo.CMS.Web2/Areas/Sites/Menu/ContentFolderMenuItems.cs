#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.Common.Web.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Kooboo.CMS.Web2.Areas.Sites.Menu
{
    public class ContentFolderMenuItems : IMenuItemContainer
    {
        #region IMenuItemContainer Members

        public IEnumerable<MenuItem> GetItems(string areaName, ControllerContext controllerContext)
        {
            var siteName = controllerContext.RequestContext.GetRequestValue("siteName");
            if (!string.IsNullOrEmpty(siteName))
            {
                var site = new Site(siteName).AsActual();
                if (site != null)
                {
                    var repository = site.GetRepository();
                    if (repository != null)
                    {
                        areaName = "Contents";
                        var items = MenuFactory.BuildMenu(controllerContext, areaName, false).Items;
                        ResetRouteValues(siteName, repository.Name, areaName, items);
                        return items;
                    }
                }
            }
            return new MenuItem[0];


        }

        private static void ResetRouteValues(string siteName, string repositoryName, string areaName, IList<MenuItem> items)
        {
            foreach (var item in items)
            {
                ResetRouteValues(siteName, repositoryName, areaName, item.Items);

                item.RouteValues["repositoryName"] = repositoryName;
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