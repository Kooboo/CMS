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
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.TemplateEngines.TAL.MvcViewEngine
{
    public class TALViewEngine : VirtualPathProviderViewEngine, IViewEngine
    {
        public TALViewEngine()
        {
            base.MasterLocationFormats = new string[] { "~/Views/{1}/{0}.tal", "~/Views/Shared/{0}.tal" };
            base.AreaMasterLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.tal", "~/Areas/{2}/Views/Shared/{0}.tal" };
            base.ViewLocationFormats = new string[] { "~/Views/{1}/{0}.tal", "~/Views/Shared/{0}.tal" };
            base.AreaViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.tal", "~/Areas/{2}/Views/Shared/{0}.tal" };
            base.PartialViewLocationFormats = base.ViewLocationFormats;
            base.AreaPartialViewLocationFormats = base.AreaViewLocationFormats;
            base.FileExtensions = new string[] { "tal" };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new TALView(controllerContext, partialPath, null);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new TALView(controllerContext, viewPath, masterPath);
        }
    }
}
