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
using System.Web;
using Kooboo.Web.Mvc.Menu;
using Kooboo.CMS.Sites.Services;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.Extensions;
namespace Kooboo.CMS.Web.Areas.Sites.Menu
{

    public class HtmlBlockNamespaceMenuItems : IMenuItemContainer
    {
        private class ViewNamespaceItemInitializer : ViewMenuItemInitializer
        {
            protected override bool GetIsActive(MenuItem item, System.Web.Mvc.ControllerContext controllerContext)
            {
                string ns = controllerContext.RequestContext.GetRequestValue("ns");
                var nsItem = item.RouteValues["ns"] == null ? "" : item.RouteValues["ns"].ToString();
                return string.Compare(ns, nsItem, true) == 0;
            }
        }
        private class HtmlBlockNamespaceItem : MenuItem
        {
            public HtmlBlockNamespaceItem(Namespace<HtmlBlock> ns)
            {
                base.Action = "index";
                base.Controller = "HtmlBlock";
                base.Area = "Sites";
                base.Text = ns.Name;

                base.Visible = true;
                RouteValues = new System.Web.Routing.RouteValueDictionary();
                RouteValues.Add("ns", ns.FullName);

                Initializer = new ViewNamespaceItemInitializer();

                if (ns.ChildNamespaces != null)
                {
                    this.Items = ns.ChildNamespaces.Select(it => new HtmlBlockNamespaceItem(it)).ToArray();
                }
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
        public IEnumerable<MenuItem> GetItems(string areaName, System.Web.Mvc.ControllerContext controllerContext)
        {
            var siteName = controllerContext.RequestContext.GetRequestValue("siteName");
            List<MenuItem> items = new List<MenuItem>();
            if (!string.IsNullOrEmpty(siteName))
            {
                var site = new Site(SiteHelper.SplitFullName(siteName));


                var root = ServiceFactory.HtmlBlockManager.GetNamespace(site);
                foreach (var ns in root.ChildNamespaces)
                {
                    items.Add(new HtmlBlockNamespaceItem(ns));
                }
            }
            return items;
        }
    }
}