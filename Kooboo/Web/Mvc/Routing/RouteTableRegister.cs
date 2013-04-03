/*
Kooboo is a content management system based on ASP.NET MVC framework. Copyright 2009 Yardi Technology Limited.

This program is free software: you can redistribute it and/or modify it under the terms of the
GNU General Public License version 3 as published by the Free Software Foundation.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.
If not, see http://www.kooboo.com/gpl3/.
*/
using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;
using System.Configuration;

namespace Kooboo.Web.Mvc.Routing
{
    public class RouteTableRegister
    {
        private sealed class IgnoreRouteInternal : Route
        {
            // Methods
            public IgnoreRouteInternal(string url)
                : base(url, new StopRoutingHandler())
            {
            }

            public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary routeValues)
            {
                return null;
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            RegisterRoutes(routes, string.Empty);
        }
        public static void RegisterRoutes(RouteCollection routes, string routeFile)
        {
            lock (routes)
            {
                RouteTableSection routesTableSection = GetRouteTableSection(routeFile);
                if (routesTableSection != null)
                {
                    //ignore route
                    if (routesTableSection.Ignores.Count > 0)
                    {
                        foreach (ConfigurationElement item in routesTableSection.Ignores)
                        {
                            var ignore = new IgnoreRouteInternal(((IgnoreRouteElement)item).Url)
                            {
                                Constraints = GetRouteValueDictionary(((IgnoreRouteElement)item).Constraints.Attributes)
                            };
                            routes.Add(ignore);
                        }
                    }

                    if (routesTableSection.Routes.Count > 0)
                    {
                        for (int routeIndex = 0; routeIndex < routesTableSection.Routes.Count; routeIndex++)
                        {
                            RouteConfigElement route = routesTableSection.Routes[routeIndex] as RouteConfigElement;
                            if (routes[route.Name] == null)
                            {
                                if (string.IsNullOrEmpty(route.RouteType))
                                {
                                    routes.Add(route.Name, new Route(
                                                        route.Url,
                                                        GetDefaults(route),
                                                        GetConstraints(route),
                                                        GetDataTokens(route),
                                                        GetInstanceOfRouteHandler(route)));
                                }
                                else
                                {
                                    var customRoute = (RouteBase)Activator.CreateInstance(Type.GetType(route.RouteType),
                                                            route.Url,
                                                            GetDefaults(route),
                                                            GetConstraints(route),
                                                            GetDataTokens(route),
                                                            GetInstanceOfRouteHandler(route));
                                    routes.Add(route.Name, customRoute);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static RouteTableSection GetRouteTableSection(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return (RouteTableSection)ConfigurationManager.GetSection("routeTable");
            }
            else
            {
                var section = (RouteTableSection)Activator.CreateInstance(typeof(RouteTableSection));

                section.DeserializeSection(IO.IOUtility.ReadAsString(file));

                return (RouteTableSection)section;
            }
        }
        /// <summary>
        /// Gets the instance of route handler.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private static IRouteHandler GetInstanceOfRouteHandler(RouteConfigElement route)
        {
            IRouteHandler routeHandler;

            if (string.IsNullOrEmpty(route.RouteHandlerType))
                routeHandler = new MvcRouteHandler();
            else
            {
                try
                {
                    Type routeHandlerType = Type.GetType(route.RouteHandlerType);
                    routeHandler = Activator.CreateInstance(routeHandlerType) as IRouteHandler;
                }
                catch (Exception e)
                {
                    throw new ApplicationException(
                                 string.Format("Can't create an instance of IRouteHandler {0}", route.RouteHandlerType),
                                 e);
                }

            }

            return routeHandler;
        }


        /// <summary>
        /// Gets the constraints.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private static RouteValueDictionary GetConstraints(RouteConfigElement route)
        {
            return GetRouteValueDictionary(route.Constraints.Attributes);
        }


        /// <summary>
        /// Gets the defaults.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private static RouteValueDictionary GetDefaults(RouteConfigElement route)
        {
            return GetRouteValueDictionary(route.Defaults.Attributes);
        }


        /// <summary>
        /// Gets the data tokens.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns></returns>
        private static RouteValueDictionary GetDataTokens(RouteConfigElement route)
        {
            return GetRouteValueDictionary(route.DataTokens.Attributes);
        }


        /// <summary>
        /// Gets the dictionary from attributes.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        private static RouteValueDictionary GetRouteValueDictionary(Dictionary<string, string> attributes)
        {
            RouteValueDictionary dataTokensDictionary = new RouteValueDictionary();

            foreach (var dataTokens in attributes)
            {
                //ref : DefaultControllerFactory.GetControllerType
                if (dataTokens.Key == "Namespaces")
                {
                    dataTokensDictionary.Add(dataTokens.Key, dataTokens.Value.Split(','));
                }
                else
                {
                    dataTokensDictionary.Add(dataTokens.Key, dataTokens.Value);
                }
            }

            return dataTokensDictionary;

        }
    }
}

