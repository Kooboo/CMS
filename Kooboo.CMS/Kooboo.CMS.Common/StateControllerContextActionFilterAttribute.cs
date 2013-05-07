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

namespace Kooboo.CMS.Common
{
    public class StateControllerContextActionFilterAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Items["ControllerContext"] = filterContext.Controller.ControllerContext;
            base.OnActionExecuting(filterContext);
        }
    }
}
