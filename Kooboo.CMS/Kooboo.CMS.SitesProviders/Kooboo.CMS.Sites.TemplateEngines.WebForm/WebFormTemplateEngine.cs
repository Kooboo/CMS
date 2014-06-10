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
using Kooboo.CMS.Sites.View.CodeSnippet;
using Kooboo.CMS.Sites.Models;
using System.Web.Mvc;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.CMS.Sites.TemplateEngines.WebForm
{
    [Dependency(typeof(ITemplateEngine), Key = "TalTemplateEngine")]
    public class WebFormTemplateEngine : ITemplateEngine
    {
        #region CreateView
        public IViewEngine GetViewEngine()
        {
            return new WebFormViewEngine();
        }
        public IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new WebFormView(controllerContext, viewPath, masterPath);
        }

        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return "WebForm";
            }
        }

        public string LayoutExtension
        {
            get { return ".aspx"; }
        }

        public string ViewExtension
        {
            get { return ".ascx"; }
        }
        public string EditorVirtualPath
        {
            //default editor
            get { return null; }
        }
        public int Order
        {
            get { return 2; }
        }
        #endregion

        #region CodeHelper

        public IDataRuleCodeSnippet GetDataRuleCodeSnippet(TakeOperation takeOperation)
        {
            switch (takeOperation)
            {
                case TakeOperation.List:
                    return new WebFormListCodeSnippet();

                case TakeOperation.First:
                    return new WebFormDetailCodeSnippet();
                default:
                    return null;
            }
        }

        public ILayoutPositionParser GetLayoutPositionParser()
        {
            return new WebFormLayoutPositionParser();
        }

        public ICodeHelper GetCodeHelper()
        {
            return new WebFormCodeHelper();
        }
        #endregion
    }

}
