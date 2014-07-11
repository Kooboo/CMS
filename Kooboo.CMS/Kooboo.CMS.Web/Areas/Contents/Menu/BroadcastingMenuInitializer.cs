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

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Web.Authorizations;
namespace Kooboo.CMS.Web.Areas.Contents.Menu
{
    public class BroadcastingMenuInitializer : AuthorizeMenuItemInitializer
    {
        protected override bool GetIsVisible(MenuItem menuItem, System.Web.Mvc.ControllerContext controllerContext)
        {
            var visible = false;
            if (Repository.Current != null)
            {
                visible = Repository.Current.EnableBroadcasting;
            }
            if (visible)
            {
                visible = base.GetIsVisible(menuItem, controllerContext);
            }
            return visible;
        }
    }
}