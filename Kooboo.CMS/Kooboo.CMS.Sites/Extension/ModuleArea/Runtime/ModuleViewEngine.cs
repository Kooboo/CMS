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
using System.Web.Mvc;
using System.Globalization;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Runtime
{
    public abstract class ModuleBuildManagerViewEngine : BuildManagerViewEngine
    {
        protected ModuleBuildManagerViewEngine() : base() { }
        protected ModuleBuildManagerViewEngine(IViewPageActivator viewPageActivator) : base(viewPageActivator) { }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (!(controllerContext.RequestContext is ModuleRequestContext))
            {
                return new ViewEngineResult(new[] { "" });
            }

            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("Value is required.", "viewName");
            }


            string[] searchedViewLocations;
            string[] searchedMasterLocations;

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            string viewPath = this.GetPath(controllerContext, this.ViewLocationFormats, viewName, controllerName, out searchedViewLocations);
            string masterPath = this.GetPath(controllerContext, this.MasterLocationFormats, masterName, controllerName, out searchedMasterLocations);

            if (!(string.IsNullOrEmpty(viewPath)) && (!(masterPath == string.Empty) || string.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(this.CreateView(controllerContext, viewPath, masterPath), this);
            }
            return new ViewEngineResult(searchedViewLocations.Union<string>(searchedMasterLocations));
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (!(controllerContext.RequestContext is ModuleRequestContext))
            {
                return new ViewEngineResult(new[] { "" });
            }
            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentException("Value is required.", partialViewName);
            }


            string[] searchedLocations;

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            string partialPath = this.GetPath(controllerContext, this.PartialViewLocationFormats, partialViewName, controllerName, out searchedLocations);

            if (string.IsNullOrEmpty(partialPath))
            {
                return new ViewEngineResult(searchedLocations);
            }
            return new ViewEngineResult(this.CreatePartialView(controllerContext, partialPath), this);
        }

        protected virtual string GetPath(ControllerContext controllerContext, string[] locations, string viewName, string controllerName, out string[] searchedLocations)
        {
            string path = null;
            searchedLocations = new string[locations.Length];

            var moduleContext = ((ModuleRequestContext)controllerContext.RequestContext).ModuleContext;

            var cacheKey = CreateCacheKey("Module", moduleContext.ModuleName, viewName, controllerName);
            path = this.ViewLocationCache.GetViewLocation(controllerContext.HttpContext, cacheKey);
            if (path != null)
            {
                return path;
            }

            var baseVirtualPath = moduleContext.ModulePath.VirtualPath;
            for (int i = 0; i < locations.Length; i++)
            {
                path = string.Format(CultureInfo.InvariantCulture, locations[i], new object[] { viewName, controllerName, baseVirtualPath });
                if (this.VirtualPathProvider.FileExists(path))
                {
                    searchedLocations = new string[0];
                    this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, path);
                    return path;
                }
                searchedLocations[i] = path;
            }
            return null;
        }
        private string CreateCacheKey(string prefix, string moduleName, string name, string controllerName)
        {
            return string.Format(CultureInfo.InvariantCulture, ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}", new object[] { base.GetType().AssemblyQualifiedName, prefix, moduleName, name, controllerName });
        }
    }
    public class ModuleWebFormViewEngine : ModuleBuildManagerViewEngine
    {

        public ModuleWebFormViewEngine() : this(null) { }
        public ModuleWebFormViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {
            base.ViewLocationFormats = new string[] {
                "{2}/Views/{1}/{0}.aspx",
                "{2}/Views/{1}/{0}.ascx",
                "{2}/Views/Shared/{0}.aspx",
                "{2}/Views/Shared/{0}.ascx"
            };

            base.MasterLocationFormats = new string[] {
                "{2}/Views/{1}/{0}.master",
                "{2}/Views/Shared/{0}.master"
            };

            base.PartialViewLocationFormats = new string[] {
                "{2}/Views/{1}/{0}.aspx",
                "{2}/Views/{1}/{0}.ascx",
                "{2}/Views/Shared/{0}.aspx",
                "{2}/Views/Shared/{0}.ascx"
            };
            base.FileExtensions = new string[] { "aspx", "ascx", "master" };
        }
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new WebFormView(controllerContext, partialPath, null, base.ViewPageActivator);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new WebFormView(controllerContext, viewPath, masterPath, base.ViewPageActivator);
        }

    }
    public class ModuleRazorViewEngine : ModuleBuildManagerViewEngine
    {
        public ModuleRazorViewEngine() : this(null) { }
        public ModuleRazorViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {
            base.ViewLocationFormats = new string[] {
                "{2}/Views/{1}/{0}.cshtml",
                "{2}/Views/{1}/{0}.vbhtml",
                "{2}/Views/Shared/{0}.cshtml",
                "{2}/Views/Shared/{0}.vbhtml"
            };

            base.MasterLocationFormats = new string[] {
                "{2}/Views/{1}/{0}.cshtml",
                "{2}/Views/Shared/{0}.vbhtml"
            };

            base.PartialViewLocationFormats = new string[] {
                "{2}/Views/{1}/{0}.cshtml",
                "{2}/Views/{1}/{0}.vbhtml",
                "{2}/Views/Shared/{0}.cshtml",
                "{2}/Views/Shared/{0}.vbhtml"
            };
            base.FileExtensions = new string[] { "cshtml", "vbhtml" };
        }
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            string layoutPath = null;
            bool runViewStartPages = false;
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, partialPath, layoutPath, runViewStartPages, fileExtensions, base.ViewPageActivator);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            string layoutPath = masterPath;
            bool runViewStartPages = true;
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, viewPath, layoutPath, runViewStartPages, fileExtensions, base.ViewPageActivator);
        }
    }
}
