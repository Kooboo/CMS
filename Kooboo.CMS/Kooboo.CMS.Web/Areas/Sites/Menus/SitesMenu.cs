using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Menu;
using Kooboo.CMS.Sites.Models;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Sites.Menus
{
    public class SiteMenuItem : MenuItem
    {
        private class SiteMenuItemInitializer : DefaultMenuItemInitializer
        {
            protected override bool GetIsActive(MenuItem menuItem, ControllerContext controllerContext)
            {
                string siteName = controllerContext.RequestContext.GetRequestValue("siteName");
                //string page = controllerContext.RequestContext.GetRequestValue("name");
                return string.Compare(siteName, menuItem.RouteValues["siteName"].ToString(), true) == 0;
                //|| string.Compare(page, this.Page.Name) == 0;
            }
        }
        public SiteMenuItem(Site site)
        {
            base.Action = "index";
            base.Controller = "home";
            base.Text = site.Name;
            if (!string.IsNullOrEmpty(site.DisplayName))
            {
                base.Text = site.DisplayName;
            }

            base.Visible = true;
            RouteValues = new System.Web.Routing.RouteValueDictionary();
            RouteValues["siteName"] = site.FullName;

            this.Initializer = new SiteMenuItemInitializer();
        }
        public override void Initialize(ControllerContext controllerContext)
        {
            this.Controller = controllerContext.RequestContext.GetRequestValue("controller");

            base.Initialize(controllerContext);
        }
        //protected override bool DefaultActive(ControllerContext controllContext)
        //{
        //    return false;
        //}
    }
    public static class SitesMenu
    {
        public static Menu BuildMenu(ControllerContext controllerContext)
        {
            Menu menu = new Menu();
            var sites = Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.AllRootSites();
            List<MenuItem> items = new List<MenuItem>();
            menu.Items = items;
            foreach (var site in sites)
            {
                items.Add(CreateSiteMenuItem(site));
            }

            menu.Initialize(controllerContext);
            return menu;
        }
        private static MenuItem CreateSiteMenuItem(Site site)
        {
            MenuItem menuItem = new SiteMenuItem(site);
            List<MenuItem> items = new List<MenuItem>();
            menuItem.Items = items;
            foreach (var child in Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.ChildSites(site))
            {
                items.Add(CreateSiteMenuItem(child));
            }
            return menuItem;
        }
    }
}