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

namespace Kooboo.Web.Mvc.Constraints
{
    public class IgnoreConstraint : IRouteConstraint
    {
        private string[] _values;

        public IgnoreConstraint(params string[] ignoreValues)
        {
            _values = ignoreValues;
        }

        public bool Match(System.Web.HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var value = values[parameterName].ToString();
            return !_values.Contains(value, StringComparer.OrdinalIgnoreCase);
        }
    }
}
