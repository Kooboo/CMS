using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using System.IO;
using Kooboo.Web.Url;

namespace Kooboo.Web.Mvc
{
    public static class AreaHelpers
    {
        // Methods
        public static string GetAreaName(RouteBase route)
        {
            IRouteWithArea area = route as IRouteWithArea;
            if (area != null)
            {
                return area.Area;
            }
            Route route2 = route as Route;
            if ((route2 != null) && (route2.DataTokens != null))
            {
                return (route2.DataTokens["area"] as string);
            }
            return null;
        }

        public static string GetAreaName(RouteData routeData)
        {
            object obj2;
            if (routeData.DataTokens.TryGetValue("area", out obj2))
            {
                return (obj2 as string);
            }
            return GetAreaName(routeData.Route);
        }
        public static string CombineAreaFilePhysicalPath(string areaName, params string[] paths)
        {
            string basePhysicalPath = Path.Combine(Settings.BaseDirectory, "Areas", areaName);
            return Path.Combine(basePhysicalPath, Path.Combine(paths));
        }
        public static string CombineAreaFileVirtualPath(string areaName, params string[] paths)
        {
            string basePhysicalPath = UrlUtility.Combine("~/", "Areas", areaName);
            return UrlUtility.Combine(basePhysicalPath, UrlUtility.Combine(paths));
        }
    }
}
