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
using System.Text;
using System.Web.Mvc;
using Kooboo.Collections;
using System.Web.Routing;


namespace Kooboo.Web.Mvc.Menu
{
    public class MenuTemplate
    {
        public IEnumerable<IMenuItemContainer> ItemContainers { get; set; }
    }
    public class MenuFactory
    {
        static MenuTemplate defaultMenu = null;

        static IDictionary<string, MenuTemplate> menuTemplates = new Dictionary<string, MenuTemplate>(StringComparer.OrdinalIgnoreCase);


        #region Menu Template
        static MenuFactory()
        {
            defaultMenu = new MenuTemplate();
            Configuration.MenuSection menuSection = Configuration.MenuSection.GetSection();
            if (menuSection != null)
            {
                defaultMenu.ItemContainers = CreateItems(menuSection.Items, new List<IMenuItemContainer>());
            }
        }
        static IEnumerable<IMenuItemContainer> CreateItems(Configuration.MenuItemElementCollection itemElementCollection, List<IMenuItemContainer> itemContainers)
        {
            if (itemElementCollection != null)
            {
                if (!string.IsNullOrEmpty(itemElementCollection.Type))
                {
                    itemContainers.Add((IMenuItemContainer)Activator.CreateInstance(Type.GetType(itemElementCollection.Type)));
                }
                else
                {
                    foreach (Configuration.MenuItemElement element in itemElementCollection)
                    {
                        IMenuItemContainer itemContainer = null;
                        if (!string.IsNullOrEmpty(element.Type))
                        {
                            itemContainer = (IMenuItemContainer)Activator.CreateInstance(Type.GetType(element.Type));
                        }
                        else
                        {
                            itemContainer = new MenuItemTemplate();
                        }
                        itemContainers.Add(itemContainer);
                        if (itemContainer is MenuItemTemplate)
                        {
                            var itemTemplate = (MenuItemTemplate)itemContainer;

                            itemTemplate.Name = element.Name;
                            itemTemplate.Text = element.Text;
                            itemTemplate.Action = element.Action;
                            itemTemplate.Controller = element.Controller;
                            itemTemplate.Visible = element.Visible;
                            itemTemplate.Area = element.Area;
                            itemTemplate.RouteValues = new System.Web.Routing.RouteValueDictionary(element.RouteValues.Attributes);
                            itemTemplate.HtmlAttributes = new System.Web.Routing.RouteValueDictionary(element.HtmlAttributes.Attributes);
                            itemTemplate.ReadOnlyProperties = element.UnrecognizedProperties;

                            if (!string.IsNullOrEmpty(element.Initializer))
                            {
                                Type type = Type.GetType(element.Initializer);
                                itemTemplate.Initializer = (IMenuItemInitializer)Activator.CreateInstance(type);
                            }

                            List<IMenuItemContainer> subItems = new List<IMenuItemContainer>();
                            if (element.Items != null)
                            {
                                itemTemplate.ItemContainers = CreateItems(element.Items, subItems);
                            }
                        }
                    }
                }
            }
            return itemContainers;

        }

        public static void RegisterAreaMenu(string templateName, string menuFileName)
        {
            lock (menuTemplates)
            {
                Configuration.MenuSection menuSection = Configuration.MenuSection.GetSection(menuFileName);
                if (menuSection != null)
                {
                    MenuTemplate areaMenu = new MenuTemplate();
                    areaMenu.ItemContainers = CreateItems(menuSection.Items, new List<IMenuItemContainer>());
                    menuTemplates.Add(templateName, areaMenu);
                }
            }
        }
        public static bool ContainsAreaMenu(string templateName)
        {
            return menuTemplates.ContainsKey(templateName);
        }
        public static MenuTemplate GetMenuTemplate(string area)
        {
            if (ContainsAreaMenu(area))
            {
                return menuTemplates[area];
            }
            return null;
        }
        #endregion

        public static Menu BuildMenu(ControllerContext controllerContext)
        {
            string areaName = AreaHelpers.GetAreaName(controllerContext.RouteData);
            return BuildMenu(controllerContext, areaName);
        }
        public static Menu BuildMenu(ControllerContext controllerContext, string areaName)
        {
            return BuildMenu(controllerContext, areaName, areaName, true);
        }
        public static Menu BuildMenu(ControllerContext controllerContext, string areaName, bool initialize)
        {
            return BuildMenu(controllerContext, areaName, areaName, initialize);
        }
        public static Menu BuildMenu(ControllerContext controllerContext, string templateName, string areaName, bool initialize)
        {
            Menu menu = new Menu(templateName);

            MenuTemplate menuTemplate = new MenuTemplate();
            if (!string.IsNullOrEmpty(templateName) && menuTemplates.ContainsKey(templateName))
            {
                menuTemplate = menuTemplates[templateName];
            }
            else
            {
                menuTemplate = defaultMenu;
            }

            menu.Items = GetItems(areaName, menuTemplate.ItemContainers, controllerContext);

            InjectMenu(menu, controllerContext);
            if (initialize)
            {
                menu.Initialize(controllerContext);
            }

            return menu;
        }
        private static void InjectMenu(Menu menu, ControllerContext controllerContext)
        {
            var injections = TypeActivator.CreateInstancesMethod(typeof(IMenuInjection)).OfType<IMenuInjection>().Where(it => it != null).ToArray();
            foreach (var injection in injections)
            {
                injection.Inject(menu, controllerContext);
            }
        }
        #region IMenuItemContainer Members

        private static IList<MenuItem> GetItems(string areaName, IEnumerable<IMenuItemContainer> itemContainers, ControllerContext controllerContext)
        {
            var items = new List<MenuItem>();

            if (itemContainers != null)
            {
                foreach (var item in itemContainers)
                {
                    items.AddRange(item.GetItems(areaName, controllerContext));
                }
            }

            return items;
        }


        #endregion
    }
    public class Menu
    {
        public string Name { get; set; }
        public Menu()
            : this("")
        {

        }
        public Menu(string name)
        {
            this.Name = name;
            Items = new List<MenuItem>();
        }
        public IList<MenuItem> Items { get; set; }

        public void Initialize(ControllerContext controllerContext)
        {
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    item.Initialize(controllerContext);
                }
            }
        }
    }
}
