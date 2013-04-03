using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Menu;
using System.Web.Mvc;



namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public class ModuleMenuInitializer : DefaultMenuItemInitializer
    {
        public override MenuItem Initialize(MenuItem menuItem, ControllerContext controllerContext)
        {
            var isActive = GetIsActive(menuItem, controllerContext);

            foreach (var sub in menuItem.Items)
            {
                sub.Initialize(controllerContext);
                isActive = isActive || sub.IsActive;
            }

            menuItem.IsActive = isActive;

            //if (!this.IsActive)
            //{
            //    this.IsActive = DefaultActive(controllerContext);
            //}


            var isVisible =
               GetIsVisible(menuItem, controllerContext);

            if (string.IsNullOrEmpty(menuItem.Action) && menuItem.Items.Where(it => it.Visible == true).Count() == 0)
            {
                isVisible = false;
            }

            menuItem.Visible = isVisible;

            menuItem.Initialized = true;

            return menuItem;
        }
    }
    public class ModuleMenu : Menu
    {
        public static Menu GetModuleMenu(string moduleName, ControllerContext controllerContext,
            ControllerContext moduleControllerContext, RouteValueDictionary scaffoldRouteValues, string siteName)
        {
            return GetModuleMenu(moduleName, controllerContext, moduleControllerContext, scaffoldRouteValues, siteName,
                controllerContext.RouteData.Values["controller"].ToString(), controllerContext.RouteData.Values["action"].ToString());
        }

        public static Menu GetModuleMenu(string moduleName, ControllerContext controllerContext,
            ControllerContext moduleControllerContext, RouteValueDictionary scaffoldRouteValues, string siteName
            , string scaffoldController, string scaffoldAction)
        {
            UrlHelper moduleUrlHelper = new UrlHelper(controllerContext.RequestContext, RouteTables.GetRouteTable(moduleName));
            Menu menu = GetModuleMenu(moduleName, moduleControllerContext ?? controllerContext);
            string currentModule = controllerContext.RequestContext.GetRequestValue("ModuleName");

            foreach (var item in menu.Items)
            {
                InitModuleMenuItem(item, moduleUrlHelper, moduleName, scaffoldController, scaffoldAction, scaffoldRouteValues, siteName);
            }
            return menu;
        }

        private static void InitModuleMenuItem(MenuItem menuItem, UrlHelper moduleUrlHelper, string moduleName, string scaffoldController, string scaffoldAction, RouteValueDictionary scaffoldRouteValues, string siteName)
        {
            var routeValues = new RouteValueDictionary(menuItem.RouteValues);
            routeValues.Merge("SiteName", siteName);
            routeValues.Merge("ModuleName", moduleName);
            var moduleUrl = moduleUrlHelper.Action(menuItem.Action, menuItem.Controller, routeValues);
            moduleUrl = ModuleUrlHelper.RemoveApplicationPath(moduleUrl, moduleUrlHelper.RequestContext.HttpContext.Request.ApplicationPath);
            menuItem.Controller = scaffoldController;
            menuItem.Action = scaffoldAction;
            menuItem.RouteValues = scaffoldRouteValues != null ? new RouteValueDictionary(scaffoldRouteValues) : new RouteValueDictionary();
            menuItem.RouteValues.Merge("ModuleUrl", moduleUrl);
            menuItem.RouteValues.Merge("ModuleName", moduleName);

            if (menuItem.Items != null && menuItem.Items.Count > 0)
            {
                foreach (var item in menuItem.Items)
                {
                    InitModuleMenuItem(item, moduleUrlHelper, moduleName, scaffoldController, scaffoldAction, scaffoldRouteValues, siteName);
                }
            }

            var isCurrentModule = moduleUrlHelper.RequestContext.GetRequestValue("ModuleName").EqualsOrNullEmpty(moduleName, StringComparison.OrdinalIgnoreCase);
            menuItem.IsActive = menuItem.IsActive && isCurrentModule;

        }
        private static object lockerHelper = new object();
        private static Menu GetModuleMenu(string moduleName, ControllerContext controllerContext)
        {
            var areaName = string.Format("module-{0}", moduleName);
            if (!MenuFactory.ContainsAreaMenu(areaName))
            {
                lock (lockerHelper)
                {
                    if (!MenuFactory.ContainsAreaMenu(areaName))
                    {
                        ModuleEntryPath moduleMenuFile = new ModuleEntryPath(moduleName, "menu.config");
                        MenuFactory.RegisterAreaMenu(areaName, moduleMenuFile.PhysicalPath);
                    }
                }
            }
            var menu = MenuFactory.BuildMenu(controllerContext, areaName);

            return menu;
        }
    }
}
