#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
//# define Page_Trace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Diagnostics;

namespace Kooboo.CMS.Sites.View
{
    public class FrontViewResult : ViewResult
    {
        private IViewEngine viewEngine;
        public FrontViewResult(ControllerContext controllerContext, string fileExtension, string viewVirtualPath)
            : this(controllerContext, fileExtension, viewVirtualPath, "")
        {
        }
        public FrontViewResult(ControllerContext controllerContext, string fileExtension, string viewVirtualPath, string masterVirtualPath)
        {
            var templateEngine = TemplateEngines.GetEngineByFileExtension(fileExtension);
            this.View = templateEngine.CreateView(controllerContext, viewVirtualPath, masterVirtualPath);
            this.viewEngine = templateEngine.GetViewEngine();
        }
        public override void ExecuteResult(ControllerContext context)
        {
#if Page_Trace
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            base.ExecuteResult(context);
#if Page_Trace
            stopwatch.Stop();
            context.HttpContext.Response.Write(string.Format("ExecuteResult, {0}ms.</br>", stopwatch.ElapsedMilliseconds));
#endif
        }
        protected override ViewEngineResult FindView(ControllerContext context)
        {
            return new ViewEngineResult(View, viewEngine);
        }
    }
}
