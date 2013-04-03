using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Web.Areas.Contents.Menus;
using Kooboo.CMS.Sites.Models;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;
namespace Kooboo.CMS.Web.Areas.Sites.Menus
{
    public class ContentFolderMenuItems : IMenuItemContainer
    {
        //public override IEnumerable<Kooboo.Web.Mvc.Menu.MenuItem> GetItems(ControllerContext controllerContext)
        //{
        //    var siteName = controllerContext.RequestContext.GetRequestValue("siteName");
        //    if (!string.IsNullOrEmpty(siteName))
        //    {
        //        var repositoryName = (new Site(SiteHelper.SplitFullName(siteName))).AsActual().Repository;
        //        if (!string.IsNullOrEmpty(repositoryName))
        //        {
        //            return GetContentFolderItems(new Content.Models.Repository(repositoryName));
        //        }
        //    }
        //    return new Kooboo.Web.Mvc.Menu.MenuItem[0];
        //}

        #region IMenuItemContainer Members

        public IEnumerable<MenuItem> GetItems(string areaName, ControllerContext controllerContext)
        {
            var siteName = controllerContext.RequestContext.GetRequestValue("siteName");
            if (!string.IsNullOrEmpty(siteName))
            {
                var site = SiteHelper.Parse(siteName).AsActual();
                if (site != null)
                {
                    var repositoryName = site.Repository;
                    if (!string.IsNullOrEmpty(repositoryName))
                    {
                        areaName = "Contents";
                        var items = MenuFactory.BuildMenu(controllerContext, areaName, false).Items;
                        ResetRouteValues(siteName, repositoryName, areaName, items);
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