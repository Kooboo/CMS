#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Sites.Menu
{
    public abstract class FileFolderMenuItems : IMenuItemContainer
    {
        public abstract FileManager FileManager { get; }

        public virtual IEnumerable<MenuItem> GetItems(string areaName, System.Web.Mvc.ControllerContext controllerContext)
        {
            List<MenuItem> menuList = new List<MenuItem>();
            var siteName = controllerContext.RequestContext.GetRequestValue("siteName");

            if (!string.IsNullOrEmpty(siteName))
            {
                var site = new Site(siteName);

                return FileManager.GetDirectories(site, "").Select(o => GenerateItem(site, controllerContext, o));
            }
            return menuList;
        }

        protected virtual MenuItem GenerateItem(Site site, ControllerContext controllerContext, DirectoryResource directoryResource)
        {
            var menuItem = new MenuItem
            {
                Text = directoryResource.Name,
                Area = "Sites",
                Action = "Index",
                Controller = "File",
                Initializer = new FileFolderItemInitializer(),
                Localizable = false
            };

            menuItem.RouteValues = new System.Web.Routing.RouteValueDictionary();

            menuItem.RouteValues["folderPath"] = ((DirectoryEntry)directoryResource).RelativePath;
            menuItem.RouteValues["type"] = FileManager.Type;

            var subDirs = FileManager.GetDirectories(site, ((DirectoryEntry)directoryResource).RelativePath);
            if (subDirs != null && subDirs.Count() > 0)
            {
                var subList = new List<MenuItem>();
                foreach (var sub in subDirs)
                {
                    subList.Add(GenerateItem(site, controllerContext, sub));
                }
                menuItem.Items = subList;
            }

            return menuItem;
        }
    }
}