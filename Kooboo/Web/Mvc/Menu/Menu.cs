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

        static IDictionary<string, MenuTemplate> areasMenu = new Dictionary<string, MenuTemplate>(StringComparer.OrdinalIgnoreCase);


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

        public static void RegisterAreaMenu(string area, string menuFileName)
        {
            lock (areasMenu)
            {
                Configuration.MenuSection menuSection = Configuration.MenuSection.GetSection(menuFileName);
                if (menuSection != null)
                {
                    MenuTemplate areaMenu = new MenuTemplate();
                    areaMenu.ItemContainers = CreateItems(menuSection.Items, new List<IMenuItemContainer>());
                    areasMenu.Add(area, areaMenu);
                }
            }
        }
        public static bool ContainsAreaMenu(string area)
        {
            return areasMenu.ContainsKey(area);
        }
        public static MenuTemplate GetMenuTemplate(string area)
        {
            if (ContainsAreaMenu(area))
            {
                return areasMenu[area];
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
            return BuildMenu(controllerContext, areaName, true);
        }
        public static Menu BuildMenu(ControllerContext controllerContext, string areaName, bool initialize)
        {
            Menu menu = new Menu();

            MenuTemplate menuTemplate = new MenuTemplate();
            if (!string.IsNullOrEmpty(areaName) && areasMenu.ContainsKey(areaName))
            {
                menuTemplate = areasMenu[areaName];
            }
            else
            {
                menuTemplate = defaultMenu;
            }

            menu.Items = GetItems(areaName, menuTemplate.ItemContainers, controllerContext);

            if (initialize)
            {
                menu.Initialize(controllerContext);
            }

            return menu;
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
        public Menu()
        {
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
