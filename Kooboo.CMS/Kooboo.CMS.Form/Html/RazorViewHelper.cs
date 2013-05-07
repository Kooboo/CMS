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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Kooboo.CMS.Form.Html
{
    internal class DummyView : IView, IViewDataContainer
    {
        public void Render(ViewContext viewContext, System.IO.TextWriter writer)
        {

        }

        public ViewDataDictionary ViewData
        {
            get;
            set;
        }
    }
    public static class RazorViewHelper
    {
        public static string RenderView(string viewPath, ControllerContext controllerContext, ViewDataDictionary viewData)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            var dummyView = new DummyView() { ViewData = viewData };
            var viewContext = new ViewContext(controllerContext, dummyView, viewData, new TempDataDictionary(), writer);

            var view = CreateView(controllerContext, viewPath, null);
            view.Render(viewContext, writer);

            return sb.ToString();
        }

        private static IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new RazorView(controllerContext, viewPath, masterPath, false, null);
        }
    }
}
