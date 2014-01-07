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

namespace Kooboo.CMS.Sites.TemplateEngines.Razor
{
    public class RazorTemplateEngine : ITemplateEngine
    {
        public string Name
        {
            get { return "Razor"; }
        }

        public string GetFileExtensionForLayout()
        {
            return ".cshtml";
        }

        public string GetFileExtensionForView()
        {
            return ".cshtml";
        }

        public IEnumerable<string> FileExtensions
        {
            get { return new string[] { ".cshtml" }; }
        }

        public IViewEngine GetViewEngine()
        {
            return new RazorViewEngine();
        }

        public IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new RazorView(controllerContext, viewPath, masterPath, false, null);
        }

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


        public int Order
        {
            get { return 1; }
        }
    }
}
