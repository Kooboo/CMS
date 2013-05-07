#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension
{
    public interface IPageRequestModule
    {
        void OnResolvingSite(HttpContextBase httpContext);
        void OnResolvedSite(HttpContextBase httpContext);
        void OnResolvingPage(ControllerContext controllerContext);
        void OnResolvedPage(ControllerContext controllerContext, PageRequestContext pageRequestContext);
    }

    public abstract class PageRequestModuleBase : IPageRequestModule
    {
        public virtual void OnResolvingSite(HttpContextBase httpContext) { }
        public virtual void OnResolvedSite(HttpContextBase httpContext) { }
        public virtual void OnResolvingPage(ControllerContext controllerContext) { }
        public virtual void OnResolvedPage(ControllerContext controllerContext, PageRequestContext pageRequestContext) { }
    }
}
