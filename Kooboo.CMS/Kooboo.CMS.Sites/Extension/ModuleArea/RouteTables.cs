using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.IO;
using Kooboo.Web.Mvc.Routing;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
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

        public static ModuleEntryPath GetRoutesFilePath(string moduleName)
        {
            return new ModuleEntryPath(moduleName, RouteFile);
        }
    }
}
