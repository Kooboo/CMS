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
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public static class ViewUrlExtensions
    {
        #region ViewUrl
        public static IHtmlString ViewUrl(this FrontUrlHelper frontUrl, string viewName)
        {
            return ViewUrl(frontUrl, viewName, null);
        }

        public static IHtmlString ViewUrl(this FrontUrlHelper frontUrl, string viewName, object values)
        {
            RouteValueDictionary routeValues = RouteValuesHelper.GetRouteValues(values);
            routeValues["ViewName"] = viewName;
            return frontUrl.WrapperUrl(frontUrl.Url.Action("ViewEntry", "Page", routeValues));
        }

        #endregion
    }
}
