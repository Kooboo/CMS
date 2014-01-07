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

namespace Kooboo.Web.Mvc.Menu
{
    public interface IMenuItemInitializer
    {
        MenuItem Initialize(MenuItem menuItem, ControllerContext controllerContext);
    }

    public class DefaultMenuItemInitializer : IMenuItemInitializer
    {
        #region IMenuInitializer Members

        public virtual MenuItem Initialize(MenuItem menuItem, ControllerContext controllerContext)
        {
            var areaName = AreaHelpers.GetAreaName(controllerContext.RouteData);
            if (!string.IsNullOrEmpty(areaName) && menuItem.RouteValues != null)
            {
                menuItem.RouteValues["area"] = areaName;
            }
            if (!string.IsNullOrEmpty(menuItem.Area) && menuItem.RouteValues != null)
            {
                menuItem.RouteValues["area"] = menuItem.Area;
            }


            var isActive =
               GetIsActive(menuItem, controllerContext);
            menuItem.IsCurrent = isActive;

            foreach (var sub in menuItem.Items)
            {
                sub.Initialize(controllerContext);
                if (sub.IsActive)
                {
                    menuItem.IsCurrent = false;
                }
                isActive = isActive || sub.IsActive;
            }

            menuItem.IsActive = isActive;

            //if (!this.IsActive)
            //{
            //    this.IsActive = DefaultActive(controllerContext);
            //}


            if (menuItem.Visible != false)
            {
                var isVisible =
               GetIsVisible(menuItem, controllerContext);

                if (string.IsNullOrEmpty(menuItem.Action) && menuItem.Items.Where(it => it.Visible == true).Count() == 0)
                {
                    isVisible = false;
                }

                menuItem.Visible = isVisible;
            }


            menuItem.Initialized = true;

            return menuItem;

        }

        protected virtual bool GetIsActive(MenuItem menuItem, ControllerContext controllerContext)
        {
            if (!string.IsNullOrEmpty(menuItem.Area))
            {
                if (string.Compare(menuItem.Area, AreaHelpers.GetAreaName(controllerContext.RouteData)) != 0)
                {
                    return false;
                }
            }
            var active = string.Compare(controllerContext.RouteData.Values["controller"].ToString(), menuItem.Controller, true) == 0;
            if (active && menuItem.ReadOnlyProperties != null)
            {
                var activeByAction = menuItem.ReadOnlyProperties["activeByAction"];
                if (!string.IsNullOrEmpty(activeByAction) && activeByAction.ToLower() == "true")
                {
                    active = string.Compare(controllerContext.RouteData.Values["action"].ToString(), menuItem.Action, true) == 0;
                }
            }
            return active;
        }

        protected virtual bool GetIsVisible(MenuItem menuItem, ControllerContext controllerContext)
        {
            return true;
        }
        #endregion
    }
}
