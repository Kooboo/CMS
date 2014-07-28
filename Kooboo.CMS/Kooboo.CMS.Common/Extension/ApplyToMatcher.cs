#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer.Dependency;
using Kooboo.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Common.Extension
{
    [Dependency(typeof(IApplyToMatcher))]
    public class ApplyToMatcher : IApplyToMatcher
    {
        public IEnumerable<T> Match<T>(IEnumerable<T> applyToItems, System.Web.Routing.RouteData route)
            where T : IApplyTo
        {
            var area = AreaHelpers.GetAreaName(route);
            var controller = route.Values["controller"].ToString();
            var action = route.Values["action"].ToString();

            return applyToItems.Where(it => it.ApplyTo == null
                || it.ApplyTo.Any(at => at.Area.EqualsOrNullEmpty(area, StringComparison.OrdinalIgnoreCase)
                && at.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase)
                && at.Action.EqualsOrNullEmpty(action, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
