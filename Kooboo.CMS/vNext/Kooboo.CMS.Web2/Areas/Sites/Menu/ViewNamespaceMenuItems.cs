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
using System.Web.Routing;

using Kooboo.Common.Web.Menu;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Web.Areas.Sites.Menu
{

    public class ViewNamespaceMenuItems : IMenuItemContainer
    {
        private class ViewNamespaceItemInitializer : ViewMenuItemInitializer
        {
            protected override bool GetIsActive(MenuItem item, System.Web.Mvc.ControllerContext controllerContext)
            {
                string ns = controllerContext.RequestContext.GetRequestValue("ns");
                var nsItem = item.RouteValues["ns"] == null ? "" : item.RouteValues["ns"].ToString();
                if (controllerContext.RouteData.Values["controller"].ToString().ToLower() == "datarule")
                {
                    if (controllerContext.RequestContext.GetRequestValue("from").ToLower() == item.Controller.ToLower()
                        && string.Compare(ns, nsItem, true) == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (controllerContext.RouteData.Values["controller"].ToString().ToLower() == "view")
                {
                    return string.Compare(ns, nsItem, true) == 0;
                }
                return false;
            }
        }
        private class ViewNamespaceItem : MenuItem
        {
            public ViewNamespaceItem(Namespace<Kooboo.CMS.Sites.Models.View> ns)
            {
                base.Action = "index";
                base.Controller = "view";
                base.Area = "Sites";
                base.Text = ns.Name;

                base.Visible = true;
                RouteValues = new System.Web.Routing.RouteValueDictionary();
                RouteValues.Add("ns", ns.FullName);

                Initializer = new ViewNamespaceItemInitializer();

                if (ns.ChildNamespaces != null)
                {
                    this.Items = ns.ChildNamespaces.Select(it => new ViewNamespaceItem(it)).ToArray();
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


                var root = ServiceFactory.ViewManager.GetNamespace(site);
                foreach (var ns in root.ChildNamespaces)
                {
                    items.Add(new ViewNamespaceItem(ns));
                }
            }
            return items;
        }
    }
}