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
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.View.CodeSnippet;
using Kooboo.CMS.Content.Models;
using System.Text.RegularExpressions;
using System.Web;
using Kooboo.CMS.Sites.TemplateEngines.NVelocity.MvcViewEngine;
using Kooboo.Common.ObjectContainer.Dependency;

namespace Kooboo.CMS.Sites.TemplateEngines.NVelocity
{
    [Dependency(typeof(ITemplateEngine), Key = "NVelocityTemplateEngine")]
    public class NVelocityTemplateEngine : ITemplateEngine
    {

        #region Properties
        public string Name
        {
            get { return "NVelocity"; }
        }

        public string LayoutExtension
        {
            get { return ".vm"; }
        }

        public string ViewExtension
        {
            get { return ".vm"; }
        }
        public string EditorVirtualPath
        {
            //default editor
            get { return null; }
        }
        public int Order
        {
            get { return 3; }
        }
        #endregion

        #region CreateView
        public System.Web.Mvc.IViewEngine GetViewEngine()
        {
            return NVelocityViewEngine.Default;
        }

        public System.Web.Mvc.IView CreateView(System.Web.Mvc.ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new NVelocityView(controllerContext, viewPath, masterPath);
        }
        #endregion

        #region CodeHelper
        public Kooboo.CMS.Sites.View.CodeSnippet.IDataRuleCodeSnippet GetDataRuleCodeSnippet(TakeOperation takeOperation)
        {
            switch (takeOperation)
            {
                case TakeOperation.List:
                    return new NVelocityListCodeSnippet();
                case TakeOperation.First:
                    return new NVelocityDetailCodeSnippet();
                case TakeOperation.Count:
                default:
                    return null;
            }
        }


        public View.CodeSnippet.ILayoutPositionParser GetLayoutPositionParser()
        {
            return new NVelocityLayoutPositionParser();
        }


        public View.CodeSnippet.ICodeHelper GetCodeHelper()
        {
            return new NVelocityCodeHelper();
        }
        #endregion
    }
}
