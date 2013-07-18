﻿#region License
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
using System.Web.Mvc;
using Kooboo.Web.Mvc.Menu;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Account.Models;

namespace Kooboo.CMS.Web.Areas.Sites.Menu
{
    public class ModuleAreaMenuItems : IMenuItemContainer
    {
        public IEnumerable<MenuItem> GetItems(string areaName, System.Web.Mvc.ControllerContext controllerContext)
        {
            List<MenuItem> items = new List<MenuItem>();
            if (Site.Current != null)
            {
                var modules = ServiceFactory.ModuleManager.AllModulesForSite(Site.Current.FullName);

                foreach (var moduleName in modules)
                {
                    items.Add(GetModuleMenuItem(moduleName, controllerContext));
                }
            }
            //var moduleMenu= ModuleMenu.GetModuleMenu()
            return items;
        }
        private void Initialize(string moduleName, MenuItem item)
        {
            if (item.ReadOnlyProperties != null)
            {
                var roleName = item.ReadOnlyProperties["role"];
                if (!string.IsNullOrEmpty(roleName) && item.Visible)
                {
                    item.Visible = Kooboo.CMS.Sites.Services.ServiceFactory.UserManager.IsInRole(Site.Current
                        , HttpContext.Current.User.Identity.Name
                        , roleName);
                }
                string permissionGroup = item.ReadOnlyProperties["permissionGroup"];
                string permissionName = item.ReadOnlyProperties["permissionName"];
                if (!string.IsNullOrEmpty(permissionName))
                {
                    var permission = new Permission() { AreaName = moduleName, Group = permissionGroup, Name = permissionName };
                    item.Visible = Kooboo.CMS.Sites.Services.ServiceFactory.UserManager.Authorize(Site.Current
                      , HttpContext.Current.User.Identity.Name
                      , permission);
                }
            }

            if (item.Items != null)
            {
                foreach (var chd in item.Items)
                {
                    Initialize(moduleName, chd);
                }
            }
        }
        private MenuItem GetModuleMenuItem(string moduleName, ControllerContext controllerContext)
        {
            MenuItem root;
            var items = MenuFactory.BuildMenu(controllerContext, moduleName, false).Items;
            if (items.Count == 1 && items.First().Name.ToLower() == "root")
            {
                root = items.First();
            }
            else
            {
                root = new MenuItem() { Text = moduleName, Action = "", Controller = "", HtmlAttributes = new System.Web.Routing.RouteValueDictionary(), RouteValues = new System.Web.Routing.RouteValueDictionary() };
                root.Items = items;
            }

            Initialize(moduleName, root);

            return root;
        }
    }
}
