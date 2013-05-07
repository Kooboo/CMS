using Kooboo.CMS.Sites.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension
{
    internal static class PageRequestModuleExecutor
    {
        private static void ExecuteModules(Action<IPageRequestModule> action)
        {
            var modules = Kooboo.CMS.Common.Runtime.EngineContext.Current.ResolveAll<IPageRequestModule>();
            foreach (var item in modules)
            {
                action(item);
            }
        }
        internal static void OnResolvingSite(HttpContextBase httpContext)
        {
            ExecuteModules((module) => module.OnResolvingSite(httpContext));
        }
        internal static void OnResolvedSite(HttpContextBase httpContext)
        {
            ExecuteModules((module) => module.OnResolvedSite(httpContext));
        }
        internal static void OnResolvingPage(ControllerContext controllerContext)
        {
            ExecuteModules((module) => module.OnResolvingPage(controllerContext));
        }
        internal static void OnResolvedPage(ControllerContext controllerContext, PageRequestContext pageRequestContext)
        {
            ExecuteModules((module) => module.OnResolvedPage(controllerContext, pageRequestContext));
        }
    }
}
