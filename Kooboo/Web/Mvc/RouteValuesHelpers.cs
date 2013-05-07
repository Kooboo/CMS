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
using System.Web;
using System.Web.Routing;
using System.Collections.Specialized;

namespace Kooboo.Web.Mvc
{
    public static class RouteValuesHelpers
    {
        // Methods
        public static RouteValueDictionary GetRouteValues(RouteValueDictionary routeValues)
        {
            if (routeValues == null)
            {
                return new RouteValueDictionary();
            }
            return new RouteValueDictionary(routeValues);
        }

        public static RouteValueDictionary MergeRouteValues(string actionName, string controllerName, RouteValueDictionary implicitRouteValues, RouteValueDictionary routeValues, bool includeImplicitMvcValues)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();
            if (includeImplicitMvcValues)
            {
                object obj2;
                if ((implicitRouteValues != null) && implicitRouteValues.TryGetValue("action", out obj2))
                {
                    dictionary["action"] = obj2;
                }
                if ((implicitRouteValues != null) && implicitRouteValues.TryGetValue("controller", out obj2))
                {
                    dictionary["controller"] = obj2;
                }
            }
            if (routeValues != null)
            {
                foreach (KeyValuePair<string, object> pair in GetRouteValues(routeValues))
                {
                    dictionary[pair.Key] = pair.Value;
                }
            }
            if (actionName != null)
            {
                dictionary["action"] = actionName;
            }
            if (controllerName != null)
            {
                dictionary["controller"] = controllerName;
            }
            return dictionary;
        }

        public static RouteValueDictionary Merge(this RouteValueDictionary routeValues, string key, object value)
        {
            routeValues[key] = value;
            return routeValues;
        }

        public static RouteValueDictionary Merge(this RouteValueDictionary routeValues, RouteValueDictionary newRouteValues)
        {
            foreach (var each in newRouteValues)
            {
                routeValues[each.Key] = each.Value;
            }
            return routeValues;
        }

        public static RouteValueDictionary Merge(this RouteValueDictionary routeValues, object newRouteValues)
        {
            return Merge(routeValues, new RouteValueDictionary(newRouteValues));
        }

        public static RouteValueDictionary Copy(this RouteValueDictionary routeValues)
        {
            return new RouteValueDictionary(routeValues);
        }

        public static RouteValueDictionary ToRouteValues(this NameValueCollection values)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary();

            foreach (var item in values.AllKeys)
            {
                routeValues[item] = values[item];
            }

            return routeValues;
        }

        public static RouteData GetRouteDataByUrl(string url)
        {
            return RouteTable.Routes.GetRouteData(new RewritedHttpContextBase(url));
        }

        private class RewritedHttpContextBase : HttpContextBase
        {
            private readonly HttpRequestBase mockHttpRequestBase;

            public RewritedHttpContextBase(string appRelativeUrl)
            {
                this.mockHttpRequestBase = new MockHttpRequestBase(appRelativeUrl);
            }

            public override HttpRequestBase Request
            {
                get
                {
                    return mockHttpRequestBase;
                }
            }

            private class MockHttpRequestBase : HttpRequestBase
            {
                private readonly string appRelativeUrl;

                public MockHttpRequestBase(string appRelativeUrl)
                {
                    this.appRelativeUrl = appRelativeUrl;
                }

                public override string AppRelativeCurrentExecutionFilePath
                {
                    get { return appRelativeUrl; }
                }

                public override string PathInfo
                {
                    get { return String.Empty; }
                }
            }
        }
    }
}
