#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Kooboo.CMS.Web.Models
{
    public class RemoteExAttribute : RemoteAttribute
    {
        public RemoteExAttribute(string routeName)
            : base(routeName)
        {
        }
        public RemoteExAttribute(string action, string controller)
            : this(action, controller, null)
        {
        }
        public RemoteExAttribute(string action, string controller, string areaName)
            : base(action, string.IsNullOrEmpty(controller) ? "*" : controller, areaName)
        {
            if (string.IsNullOrEmpty(controller) || controller == "*")
            {
                variableController = true;
            }
            this.HttpMethod = "POST";
        }
        private bool variableController = false;
        private string routeFields;
        private string[] routeFieldsSplit = new string[0];
        public string RouteFields
        {
            get
            {
                return routeFields;
            }
            set
            {
                routeFields = value;
                if (!string.IsNullOrEmpty(routeFields))
                {
                    routeFieldsSplit = RouteFields.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    routeFieldsSplit = new string[0];
                }

            }
        }

        protected override string GetUrl(ControllerContext controllerContext)
        {
            var routeData = new RouteValueDictionary(this.RouteData);
            if (variableController)
            {
                routeData["controller"] = controllerContext.RouteData.Values["controller"];
            }
            //merge the route data
            if (routeFieldsSplit.Length > 0)
            {
                foreach (var field in routeFieldsSplit)
                {
                    routeData[field] = controllerContext.RequestContext.GetRequestValue(field);
                }
            }
            VirtualPathData data = this.Routes.GetVirtualPathForArea(controllerContext.RequestContext, this.RouteName, routeData);
            if (data == null)
            {
                throw new InvalidOperationException(SR.GetString("RemoteAttribute_NoUrlFound"));
            }
            return data.VirtualPath;

            //return base.GetUrl(controllerContext);
        }
    }
}