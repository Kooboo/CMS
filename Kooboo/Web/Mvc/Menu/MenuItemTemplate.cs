using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.Web.Mvc.Menu
{
    public class MenuItemTemplate : MenuItem, IMenuItemContainer
    {
        public IEnumerable<IMenuItemContainer> ItemContainers { get; set; }

        public virtual void Initialize(MenuItem menuItem, ControllerContext controllerContext)
        {
            Initializer.Initialize(menuItem, controllerContext);
        }

        public virtual IEnumerable<MenuItem> GetItems(string areaName, ControllerContext controllerContext)
        {
            List<MenuItem> items = new List<MenuItem>();
            items.Add(CreateMenuItemByTemplate(areaName, this, controllerContext));
            return items;
        }

        private MenuItem CreateMenuItemByTemplate(string areaName, MenuItemTemplate template, ControllerContext controllerContext)
        {
            MenuItem item = new MenuItem()
            {
                Action = template.Action,
                Area = string.IsNullOrEmpty(template.Area) ? areaName : template.Area,
                Controller = template.Controller,
                HtmlAttributes = new RouteValueDictionary(template.HtmlAttributes),
                Localizable = template.Localizable,
                Name = template.Name,
                ReadOnlyProperties = template.ReadOnlyProperties,
                RouteValues = new RouteValueDictionary(template.RouteValues),
                Text = template.Text,
                Visible = template.Visible,
                Initializer = template.Initializer
            };

            List<MenuItem> items = new List<MenuItem>();
            if (template.ItemContainers != null)
            {
                foreach (var itemContainer in template.ItemContainers)
                {
                    items.AddRange(itemContainer.GetItems(areaName, controllerContext));
                }
            }

            item.Items = items;

            return item;
        }

    }
}
