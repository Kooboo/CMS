using Kooboo.CMS.Sites.Extension.ModuleArea.Runtime;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public static class ModuleControllerContextExtensions
    {
        #region GetModuleName
        /// <summary>
        /// Get the module name from controller context.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        public static string GetModuleName(this ControllerContext controllerContext)
        {
            return Kooboo.Web.Mvc.AreaHelpers.GetAreaName(controllerContext.RouteData);
        }
        #endregion

        #region GetModuleContext
        public static ModuleContext GetModuleContext(this ControllerContext controllerContext)
        {
            CheckFrontendController(controllerContext);
            return ((ModuleRequestContext)(controllerContext.RequestContext)).ModuleContext;
        }

        private static void CheckFrontendController(ControllerContext controllerContext)
        {
            if (!(controllerContext.RequestContext is ModuleRequestContext))
            {
                throw new InvalidOperationException("The GetModuleContext only can be invoked from the module frontend controller.");
            }
        }
        #endregion     

        #region ShareData
        public static void ShareData(this ControllerContext controllerContext, string key, object data)
        {
            CheckFrontendController(controllerContext);
            Page_Context.Current.ControllerContext.Controller.ViewData[key] = data;
        }
        public static object GetSharedData(this ControllerContext controllerContext, string key)
        {
            CheckFrontendController(controllerContext);

            if (Page_Context.Current.ControllerContext.Controller.ViewData.ContainsKey(key))
            {
                return Page_Context.Current.ControllerContext.Controller.ViewData[key];
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
