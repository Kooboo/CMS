using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Menu;

using Kooboo.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Services;

namespace Kooboo.CMS.Web.Areas.Sites.Menus
{
    public class ThemeResourceMenuItems : ResourceMenuItems
    {

        public override string Controller
        {
            get
            {
                return "ThemeResource";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override FileManager FileManager
        {
            get { return ServiceFactory.GetService<ThemeFileManager>(); }
        }
        protected override MenuItem GenerateItem(Site site, ControllerContext controllerContext, DirectoryResource directoryResource)
        {
            var item = base.GenerateItem(site, controllerContext, directoryResource);
            if (controllerContext.RequestContext.GetRequestValue("ThemeName") == null)
            {
                item.RouteValues["ThemeName"] = directoryResource.Name;
            }

            return item;
        }
    }

    public class FileResourceManeuItems : ResourceMenuItems
    {


        public override string Controller
        {
            get
            {
                return "FileResource";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override FileManager FileManager
        {
            get { return new CustomFileManagerEx(); }
        }
    }


    public abstract class ResourceMenuItems : IMenuItemContainer
    {
        private class ResourceItemInitializer : ViewMenuItemInitializer
        {
            protected override bool GetIsActive(MenuItem item, System.Web.Mvc.ControllerContext controllerContext)
            {
                string directoryPath = controllerContext.RequestContext.GetRequestValue("directoryPath");



                return item.RouteValues["directoryPath"] != null && item.RouteValues["directoryPath"].ToString() == directoryPath;
            }
        }


        public abstract FileManager FileManager { get; }
        public abstract string Controller { get; set; }

        public virtual IEnumerable<MenuItem> GetItems(string areaName, System.Web.Mvc.ControllerContext controllerContext)
        {
            List<MenuItem> menuList = new List<MenuItem>();
            var siteName = controllerContext.RequestContext.GetRequestValue("siteName");



            if (!string.IsNullOrEmpty(siteName))
            {
                var site = new Site(SiteHelper.SplitFullName(siteName));

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
                Controller = Controller,
                Initializer = new ResourceItemInitializer(),
                Localizable = false
            };

            menuItem.RouteValues = new System.Web.Routing.RouteValueDictionary();

            menuItem.RouteValues["directoryPath"] = ((DirectoryEntry)directoryResource).RelativePath;


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