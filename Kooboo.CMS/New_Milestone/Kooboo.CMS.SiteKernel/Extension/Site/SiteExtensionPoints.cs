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

namespace Kooboo.CMS.SiteKernel.Extension.Site
{
    public static class SiteExtensionPoints
    {
        //Site cluster页面
        public static MvcRoute SiteCluster = new MvcRoute() { Area = "Sites", Controller = "Home", Action = "Cluster" };
        //Site setting页面
        public static MvcRoute SiteSetting = new MvcRoute() { Area = "Sites", Controller = "Site", Action = "Settings" };
    }
}
