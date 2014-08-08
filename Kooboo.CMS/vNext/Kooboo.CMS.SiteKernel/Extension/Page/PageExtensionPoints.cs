#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Extension.Page
{
    public static class PageExtensionPoints
    {
        //Site map页面
        public static MvcRoute SiteMap = new MvcRoute() { Area = "Sites", Controller = "Home", Action = "SiteMap" };
        //Site 列表页面
        public static MvcRoute Page_Grid = new MvcRoute() { Area = "Sites", Controller = "Page", Action = "Index" };
        public static MvcRoute Page_Create = new MvcRoute() { Area = "Sites", Controller = "Page", Action = "Create" };
        public static MvcRoute Page_Edit = new MvcRoute() { Area = "Sites", Controller = "Page", Action = "Edit" };
        public static MvcRoute Page_Draft = new MvcRoute() { Area = "Sites", Controller = "Page", Action = "Draft" };

        public static string SiteMapNodeButton = "SiteMapNode";
    }
}
