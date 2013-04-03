using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Menu;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.Globalization;
namespace Kooboo.CMS.Web.Areas.Sites.Menus
{
    public class PageMenuItem : MenuItem
    {
        public PageMenuItem(Page page)
        {
            base.Action = "Index";
            base.Controller = "Page";
            base.Area = "Sites";
            base.Text = page.Name;
            if (page.Navigation != null && !string.IsNullOrEmpty(page.Navigation.DisplayText))
            {
                base.Text = page.Navigation.DisplayText;
            }

            base.Visible = true;
            RouteValues = new System.Web.Routing.RouteValueDictionary();
            RouteValues.Add("fullName", page.FullName);

            HtmlAttributes = new System.Web.Routing.RouteValueDictionary();
            if (page.IsDefault)
            {
                HtmlAttributes.Add("class", "home");
            }

            //this.Page = page;
            Initializer = new PageMenuItemInitializer();
        }
        public override bool Localizable
        {
            get
            {
                return false;
            }
            set
            {

            }
        }

        //protected override bool DefaultActive(ControllerContext controllContext)
        //{
        //    return false;
        //}
    }


    public class PageMenuItems : IMenuItemContainer
    {

        private MenuItem CreatePageMenuItem(Page page)
        {

            MenuItem item = new PageMenuItem(page);

            List<MenuItem> subItems = new List<MenuItem>();
            item.Items = subItems;

            var subPages = ServiceFactory.PageManager.ChildPages(page.Site, page.FullName, "");
            if (subPages != null)
            {
                foreach (var subPage in subPages)
                {
                    subItems.Add(CreatePageMenuItem(subPage));
                }
            }
            return item;
        }


        #region IMenuItemContainer Members

        public IEnumerable<MenuItem> GetItems(string areaName, ControllerContext controllerContext)
        {
            var siteName = controllerContext.RequestContext.GetRequestValue("siteName");
            List<MenuItem> items = new List<MenuItem>();
            if (!string.IsNullOrEmpty(siteName))
            {
                var site = new Site(SiteHelper.SplitFullName(siteName));

                var pageManager = ServiceFactory.PageManager;

                var pages = pageManager.All(site, "", "");
                foreach (var page in pages)
                {
                    items.Add(CreatePageMenuItem(page));
                }

            }
            return items;
        }

        #endregion
    }
}