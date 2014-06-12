#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.TemplateEngines.TAL.MvcViewEngine;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.View.CodeSnippet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.TemplateEngines.TAL
{
    [Dependency(typeof(ITemplateEngine), Key = "TalTemplateEngine")]
    public class TalTemplateEngine : ITemplateEngine
    {
        #region Properties
        public string Name
        {
            get { return "TAL"; }
        }

        public int Order
        {
            get { return 4; }
        }

        public string LayoutExtension
        {
            get { return ".tal"; }
        }

        public string ViewExtension
        {
            get { return ".tal"; }
        }
        public string EditorVirtualPath
        {
            get { return "_TALEditor"; }
        }
        #endregion

        #region GetViewEngine
        public System.Web.Mvc.IViewEngine GetViewEngine()
        {
            return new TALViewEngine();
        }
        #endregion

        #region CreateView
        public System.Web.Mvc.IView CreateView(System.Web.Mvc.ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new TALView(controllerContext, viewPath, masterPath);
        }
        #endregion

        #region GetDataRuleCodeSnippet
        public IDataRuleCodeSnippet GetDataRuleCodeSnippet(Models.TakeOperation takeOperation)
        {
            return new DataRuleCodeSnippet();
        }
        #endregion

        #region GetLayoutPositionParser
        public ILayoutPositionParser GetLayoutPositionParser()
        {
            return new LayoutPositionParser();
        }
        #endregion

        #region GetCodeHelper
        public ICodeHelper GetCodeHelper()
        {
            return new CodeHelper();
        }
        #endregion
    }
}
