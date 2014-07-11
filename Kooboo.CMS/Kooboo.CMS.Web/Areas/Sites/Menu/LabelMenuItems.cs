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
using Kooboo.Common.Web.Menu;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites.Menu
{
    public class LabelMenuItem : MenuItem
    {
        private class LabelMenuItemInitializer : DefaultMenuItemInitializer
        {
            protected override bool GetIsActive(MenuItem menuItem, ControllerContext controllerContext)
            {
                string category = controllerContext.RequestContext.GetRequestValue("category");
                return string.Compare(category, menuItem.RouteValues["category"].ToString(), true) == 0;
            }
        }
        public LabelMenuItem(string name, string fullName, string culture)
        {
            base.Action = "index";
            base.Controller = "label";
            base.Area = "Sites";
            base.Text = name;
            base.Visible = true;
            RouteValues = new System.Web.Routing.RouteValueDictionary();
            RouteValues.Add("category", fullName);
            RouteValues.Add("culture", culture);

            this.Initializer = new LabelMenuItemInitializer();
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
    public class LabelMenuItems : IMenuItemContainer
    {

        private void CreateMenuItem(Site site, string category, ref IDictionary<string, MenuItem> items)
        {
            if (!string.IsNullOrEmpty(category))
            {
                string parentName = "";
                foreach (var name in category.Split('.'))
                {
                    var fullName = string.IsNullOrEmpty(parentName) ? name : string.Join(".", parentName, name);
                    if (!items.ContainsKey(fullName))
                    {
                        var item = new LabelMenuItem(name, fullName, "");
                        //CreateCultureItems(site, name, fullName, item);
                        if (!string.IsNullOrEmpty(parentName))
                        {
                            var parent = items[parentName];
                            ((IList<MenuItem>)parent.Items).Add(item);
                        }

                        items.Add(fullName, item);
                    }
                    parentName = fullName;
                }
            }
        }


        #region IMenuItemContainer Members

        public IEnumerable<MenuItem> GetItems(string areaName, ControllerContext controllerContext)
        {
            var site = Site.Current;

            IDictionary<string, MenuItem> items = new Dictionary<string, MenuItem>(StringComparer.CurrentCultureIgnoreCase);
            if (site != null)
            {
                var labelManager = ServiceFactory.LabelManager;

                var labelCategories = labelManager.GetCategories(site);
                foreach (var category in labelCategories)
                {
                    CreateMenuItem(site, category, ref items);
                }
            }
            return items.Where(it => it.Key.IndexOf(".") == -1).Select(it => it.Value);
        }

        #endregion
    }
}