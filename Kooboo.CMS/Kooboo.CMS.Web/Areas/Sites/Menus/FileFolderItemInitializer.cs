#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Web.Mvc.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Sites.Menus
{
    public class FileFolderItemInitializer : SiteAuthorizeMenuItemInitializer
    {
        protected override bool GetIsActive(MenuItem item, System.Web.Mvc.ControllerContext controllerContext)
        {
            string folderPath = (controllerContext.RequestContext.GetRequestValue("folderPath") ?? "").ToLower();

            string type = (controllerContext.RequestContext.GetRequestValue("type") ?? "").ToLower();

            string route_FolderPath = (item.RouteValues["folderPath"] ?? "").ToString().ToLower();
            string route_Type = item.RouteValues["type"].ToString().ToLower();
            return (route_FolderPath == folderPath)
                && route_Type == type;
        }

    }
}