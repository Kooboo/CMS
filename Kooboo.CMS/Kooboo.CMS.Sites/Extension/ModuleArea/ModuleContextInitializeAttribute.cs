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

using Kooboo.Common.Globalization;
using Kooboo.CMS.Sites.Models;


namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public class ModuleContextInitializeAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            if (ModuleContext.Current == null)
            {
                var moduleName = filterContext.Controller.ControllerContext.GetModuleName();
                if (string.IsNullOrEmpty(moduleName))
                {
                    throw new ArgumentException("Module must be created as MVC area.".Localize());
                }
                Site site = Site.Current;
                if (site == null)
                {
                    var siteName = filterContext.RequestContext.GetRequestValue("siteName");
                    if (!string.IsNullOrEmpty(siteName))
                    {
                        site = new Site(siteName);
                    }
                }
                ModuleContext.Current = ModuleContext.Create(moduleName, site);
            }
        }
    }
}
