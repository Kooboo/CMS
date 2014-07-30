#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.TokenTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Kooboo.CMS.SiteKernel.Models
{
    public static class PageRouteExtensions
    {
        public static IEnumerable<string> GetRouteParamters(this PageRoute pageRoute)
        {
            var routePath = pageRoute.RoutePath;

            TemplateParser parser = new TemplateParser();

            var parameters = parser.GetTokens(routePath);

            return parameters;
        }

        /// <summary>
        /// 填充默认的URL参数值，保证生产页面链接时不出错
        /// </summary>
        /// <param name="pageRoute"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        private static RouteValueDictionary MergeEmptyRouteValues(this PageRoute pageRoute, RouteValueDictionary routeValues)
        {
            var parameters = pageRoute.GetRouteParamters();

            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    if (!routeValues.ContainsKey(p))
                    {
                        routeValues[p] = "";
                    }
                }
            }

            return routeValues;
        }
        public static Route ToMvcRoute(this PageRoute pageRoute)
        {
            var routePath = GetRouteUrl(pageRoute);
            var routeValues = RouteValuesHelper.GetRouteValues(pageRoute.Defaults);
            routeValues = MergeEmptyRouteValues(pageRoute, routeValues);
            var mvcRoute = new Route(routePath, routeValues, null);
            return mvcRoute;
        }
        private static string GetRouteUrl(PageRoute pageRoute)
        {
            var routePath = pageRoute.RoutePath;
            if (!string.IsNullOrEmpty(pageRoute.TrimmedIdentifier))
            {
                if (!string.IsNullOrEmpty(routePath))
                {
                    routePath += "/" + pageRoute.TrimmedIdentifier;
                }
                else
                {
                    routePath = pageRoute.TrimmedIdentifier;
                }
            }
            return routePath;
        }

    }
}
