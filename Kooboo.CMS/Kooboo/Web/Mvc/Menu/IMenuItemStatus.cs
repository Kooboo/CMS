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
    public interface IMenuItemStatus
    {
        bool IsVisible(MenuItem item, ControllerContext conrollerContext);
        bool IsActive(MenuItem item, ControllerContext controllerContext);
    }

    public class DefaultMenuItemStatus : IMenuItemStatus
    {

        #region IMenuStatus Members

        public virtual bool IsVisible(MenuItem item, ControllerContext conrollerContext)
        {
            return true;
        }

        public virtual bool IsActive(MenuItem item, ControllerContext controllerContext)
        {
            if (!string.IsNullOrEmpty(item.Area))
            {
                if (string.Compare(item.Area, AreaHelpers.GetAreaName(controllerContext.RouteData)) != 0)
                {
                    return false;
                }
            }
            return string.Compare(controllerContext.RouteData.Values["controller"].ToString(), item.Controller, true) == 0;

        }

        #endregion

    }
}
