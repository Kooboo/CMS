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

namespace Kooboo.CMS.Sites.Extension.UI.TopToolbar
{
    public class ToolbarButtons
    {
        public static IEnumerable<ToolbarGroupButtons> GetToolbarButtons(RequestContext requestContext)
        {
            var tabProviders = Kooboo.CMS.Common.Runtime.EngineContext.Current.ResolveAll<IToolbarProvider>();

            var matchedProviders = MatchProviders(tabProviders, requestContext).ToArray();

            var groups = matchedProviders.SelectMany(it => it.GetGroups(requestContext)).OrderBy(it => it.Order)
                .Distinct(new ToolbarGroupEqualityComparer())
                .Select(it => new ToolbarGroupButtons() { Group = it, Buttons = new List<ToolbarButton>() }).ToArray();

            var buttons = matchedProviders.SelectMany(it => it.GetButtons(requestContext)).OrderBy(it => it.Order).ToArray();

            var nonGroup = new ToolbarGroupButtons() { Group = null, Buttons = new List<ToolbarButton>() };

            foreach (var button in buttons)
            {
                if (string.IsNullOrEmpty(button.GroupName))
                {
                    nonGroup.Buttons.Add(button);
                }
                else
                {
                    var group = groups.Where(it => it.Group.GroupName.EqualsOrNullEmpty(button.GroupName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (group != null)
                    {
                        group.Buttons.Add(button);
                    }
                    else
                    {
                        nonGroup.Buttons.Add(button);
                    }
                }
            }

            nonGroup.Buttons = nonGroup.Buttons.OrderBy(it => it.Order).ToList();

            foreach (var group in groups)
            {
                group.Buttons = group.Buttons.OrderBy(it => it.Order).ToList();
            }

            return new[] { nonGroup }.Concat(groups);
        }
        private static IEnumerable<IToolbarProvider> MatchProviders(IEnumerable<IToolbarProvider> tabProviders, RequestContext requestContext)
        {

            return tabProviders.Where(it => it.ApplyTo == null
                            || it.ApplyTo.Any(at => MatchProvider(at, requestContext)));
        }
        private static bool MatchProvider(MvcRoute applyTo, RequestContext requestContext)
        {
            var area = AreaHelpers.GetAreaName(requestContext.RouteData);
            var controller = requestContext.GetRequestValue("controller");
            var action = requestContext.GetRequestValue("action");


            var matched = applyTo.Area.EqualsOrNullEmpty(area, StringComparison.OrdinalIgnoreCase)
                 && applyTo.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase)
                 && applyTo.Action.EqualsOrNullEmpty(action, StringComparison.OrdinalIgnoreCase);

            if (matched && applyTo.RouteValues != null)
            {
                foreach (var item in applyTo.RouteValues)
                {
                    if (item.Value == null)
                    {
                        continue;
                    }
                    var routeValue = requestContext.GetRequestValue(item.Key);
                    if (routeValue == null)
                    {
                        matched = false;
                        break;
                    }
                    if (!item.Value.ToString().EqualsOrNullEmpty(routeValue.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        matched = false;
                        break;
                    }
                }
            }

            return matched;
        }

    }
}
