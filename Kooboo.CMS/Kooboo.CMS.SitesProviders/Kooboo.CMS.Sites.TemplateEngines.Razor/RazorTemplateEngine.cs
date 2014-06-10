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
using Kooboo.CMS.Sites.View;
using System.Web.Mvc;
using Kooboo.CMS.Sites.View.CodeSnippet;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.CMS.Sites.TemplateEngines.Razor
{
    [Dependency(typeof(ITemplateEngine), Key = "RazorTemplateEngine")]
    public class RazorTemplateEngine : ITemplateEngine
    {
        #region Properties
        public string Name
        {
            get { return "Razor"; }
        }

        public string LayoutExtension
        {
            get { return ".cshtml"; }
        }

        public string ViewExtension
        {
            get { return ".cshtml"; }
        }

        public int Order
        {
            get { return 1; }
        }

        public string EditorVirtualPath
        {
            //default editor
            get { return null; }
        }
        #endregion

        #region CreateView
        public IViewEngine GetViewEngine()
        {
            return new RazorViewEngine();
        }

        public IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new RazorView(controllerContext, viewPath, masterPath, false, null);
        }

        #endregion

        #region CodeHelper
        public IDataRuleCodeSnippet GetDataRuleCodeSnippet(TakeOperation takeOperation)
        {
            switch (takeOperation)
            {
                case TakeOperation.List:
                    return new RazorListCodeSnippet();
                case TakeOperation.First:
                    return new RazorDetailCodeSnippet();
                case TakeOperation.Count:
                default:
                    return null;
            }
        }

        public ILayoutPositionParser GetLayoutPositionParser()
        {
            return new RazorLayoutPositionParser();
        }

        public ICodeHelper GetCodeHelper()
        {
            return new RazorCodeHelper();
        }
        #endregion





    }
}
