using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Menu;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites.Menus
{
    public class ModuleMenuItems : IMenuItemContainer
    {
        public IEnumerable<MenuItem> GetItems(string areaName, System.Web.Mvc.ControllerContext controllerContext)
        {
            List<MenuItem> items = new List<MenuItem>();
            if (Site.Current != null)
            {
                var modules = ServiceFactory.ModuleManager.AllModulesForSite(Site.Current.FullName);
                ControllerContext moduleControllerContext = null;
                if (controllerContext.Controller.ViewData.Model is ModuleResultExecutedContext)
                {
                    moduleControllerContext = ((ModuleResultExecutedContext)(controllerContext.Controller.ViewData.Model)).Controller.ControllerContext;
                }
                foreach (var moduleName in modules)
                {
                    items.Add(GetModuleMenuItem(moduleName, controllerContext, moduleControllerContext));
                }
            }
            //var moduleMenu= ModuleMenu.GetModuleMenu()
            return items;
        }
        private MenuItem GetModuleMenuItem(string moduleName, ControllerContext controllerContext, ControllerContext moduleControllerContext)
        {
            MenuItem moduleMenuItem = new MenuItem() { Text = moduleName, Action = "", Controller = "", HtmlAttributes = new System.Web.Routing.RouteValueDictionary(), RouteValues = new System.Web.Routing.RouteValueDictionary() };
            var moduleMenu = ModuleMenu.GetModuleMenu(moduleName, controllerContext, moduleControllerContext, new RouteValueDictionary(new { area = "Sites" }), Site.Current.FullName, "Module", "Scaffold");
            moduleMenuItem.Items = moduleMenu.Items;
            return moduleMenuItem;
        }
    }
}