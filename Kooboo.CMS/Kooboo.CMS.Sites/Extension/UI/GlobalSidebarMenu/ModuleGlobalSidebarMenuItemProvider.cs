#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Services;
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.UI.GlobalSidebarMenu
{
   [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IGlobalSidebarMenuItemProvider), Key = "ModuleGlobalSidebarMenuItemProvider")]
    public class ModuleGlobalSidebarMenuItemProvider : IGlobalSidebarMenuItemProvider
    {
        public static string GetGlobalSidebarMenuTemplateName(string moduleName)
        {
            return moduleName + "||GlobalMenu";
        }
        public IEnumerable<Kooboo.Web.Mvc.Menu.MenuItem> GetMenuItems(System.Web.Mvc.ControllerContext controllerContext)
        {
            return GetItems(controllerContext);
        }

        public IEnumerable<MenuItem> GetItems(System.Web.Mvc.ControllerContext controllerContext)
        {
            List<MenuItem> items = new List<MenuItem>();

            var modules = ServiceFactory.ModuleManager.All();

            foreach (var moduleName in modules)
            {
                items.AddRange(GetModuleMenuItem(moduleName, controllerContext));
            }

            return items;
        }

        private IEnumerable<MenuItem> GetModuleMenuItem(string moduleName, ControllerContext controllerContext)
        {
            var items = MenuFactory.BuildMenu(controllerContext, GetGlobalSidebarMenuTemplateName(moduleName), moduleName, true).Items;
            return items;
        }
    }
}
