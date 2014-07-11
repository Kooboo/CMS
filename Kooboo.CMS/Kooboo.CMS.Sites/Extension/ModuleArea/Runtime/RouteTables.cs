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
using System.Web.Routing;
using System.IO;
using Kooboo.Common.Web.Routing;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Runtime
{
    public static class RouteTables
    {
        public static string RouteFile = "routes.config";

        static IDictionary<string, RouteCollection> routeTables = new Dictionary<string, RouteCollection>(StringComparer.CurrentCultureIgnoreCase);
        public static RouteCollection GetRouteTable(string moduleName)
        {
            if (!routeTables.ContainsKey(moduleName))
            {
                lock (routeTables)
                {
                    if (!routeTables.ContainsKey(moduleName))
                    {
                        var routeFile = GetRoutesFilePath(moduleName).PhysicalPath;
                        RouteCollection routeTable = new RouteCollection();
                        RouteTableRegister.RegisterRoutes(routeTable, routeFile);
                        routeTables[moduleName] = routeTable;
                    }
                }
            }
            return routeTables[moduleName];
        }

        public static ModuleItemPath GetRoutesFilePath(string moduleName)
        {
            return new ModuleItemPath(moduleName, RouteFile);
        }
    }
}
