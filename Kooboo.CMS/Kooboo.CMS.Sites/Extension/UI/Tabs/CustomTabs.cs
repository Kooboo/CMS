#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Kooboo.CMS.Sites.Extension.UI.Tabs
{
    public class CustomTabs
    {
        public static IEnumerable<TabInfo> GetTabs(RequestContext requestContext)
        {
            var tabProviders = Kooboo.CMS.Common.Runtime.EngineContext.Current.ResolveAll<ITabProvider>();

            var matchedProviders = MatchProviders(tabProviders, requestContext.RouteData);

            return matchedProviders.SelectMany(it => it.GetTabs(requestContext)).OrderBy(it => it.Order);
        }
        private static IEnumerable<ITabProvider> MatchProviders(IEnumerable<ITabProvider> tabProviders, RouteData route)
        {
            var area = AreaHelpers.GetAreaName(route);
            var controller = route.Values["controller"].ToString();
            var action = route.Values["action"].ToString();

            return tabProviders.Where(it => it.ApplyTo == null
                || it.ApplyTo.Any(at => at.Area.EqualsOrNullEmpty(area, StringComparison.OrdinalIgnoreCase)
                && at.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase)
                && at.Action.EqualsOrNullEmpty(action, StringComparison.OrdinalIgnoreCase)));

        }
    }
}
