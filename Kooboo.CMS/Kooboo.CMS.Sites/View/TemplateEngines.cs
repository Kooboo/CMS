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
using System.Collections;
using System.Web.Mvc;
using Kooboo.CMS.Sites.View.CodeSnippet;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.View
{
    public enum CodeType
    {
        List,
        Detail
    }
    public interface ITemplateEngine
    {
        #region Properties
        string Name { get; }
        int Order { get; }
        string LayoutExtension { get; }
        string ViewExtension { get; }
        string EditorVirtualPath { get; }
        #endregion

        #region CreateView
        IViewEngine GetViewEngine();
        IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath);
        #endregion

        #region CodeHelper
        IDataRuleCodeSnippet GetDataRuleCodeSnippet(TakeOperation takeOperation);
        ILayoutPositionParser GetLayoutPositionParser();
        ICodeHelper GetCodeHelper();
        #endregion
    }

    public static class TemplateEngines
    {
        //private static List<ITemplateEngine> engines = new List<ITemplateEngine>();
        //static TemplateEngines()
        //{
        //    //RegisterEngine(new ASPXTemplateEngine());
        //    //RegisterEngine(new RazorTemplateEngine());
        //}
        public static IEnumerable<ITemplateEngine> Engines
        {
            get
            {
                return Kooboo.Common.ObjectContainer.EngineContext.Current.ResolveAll<ITemplateEngine>();
            }
        }
        //public static void RegisterEngine(ITemplateEngine engine)
        //{
        //    lock (engines)
        //    {
        //        engines.Add(engine);
        //    }
        //}

        public static ITemplateEngine GetEngineByFileExtension(string fileExtension)
        {
            var engine = Engines.Where(it => new[] { it.LayoutExtension, it.ViewExtension }.Contains(fileExtension, StringComparer.OrdinalIgnoreCase)).FirstOrDefault();
            if (engine == null)
            {
                throw new NotSupportedException(string.Format("Not supported engine for '{0}'", fileExtension));
            }
            return engine;
        }
        public static ITemplateEngine GetEngineByName(string name)
        {
            var engine = Engines.Where(it => it.Name.EqualsOrNullEmpty(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (engine == null)
            {
                throw new NotSupportedException(string.Format("Not found the engine. Name:'{0}'", name));
            }
            return engine;
        }
    }
}
