#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.IO;
using SharpTAL;
using SharpTAL.TemplateCache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.TemplateEngines.TAL.MvcViewEngine
{
    public class TALView : IViewDataContainer, IView
    {
        #region ctor
        private ControllerContext _controllerContext;
        private readonly string _masterTemplate;
        private readonly string _viewTemplate;
        private static FileSystemTemplateCache templateCache;

        static TALView()
        {
            string cacheFolder = Path.Combine(Path.GetDirectoryName(
               Assembly.GetExecutingAssembly().Location), "Template Cache");

            Kooboo.IO.IOUtility.EnsureDirectoryExists(cacheFolder);

            // Create the template cache.
            // We want to clear the cache folder on startup, setting the clearCache parameter to true,
            // and using customized file name pattern.
            templateCache = new FileSystemTemplateCache(cacheFolder, true);
        }

        public TALView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            _controllerContext = controllerContext;
            this._masterTemplate = masterPath;
            this._viewTemplate = viewPath;
        }
        #endregion

        public virtual void Render(ViewContext viewContext, System.IO.TextWriter writer)
        {
            this.ViewData = viewContext.ViewData;

            bool hasLayout = _masterTemplate != null;

            if (hasLayout)
            {
                //有master模板的情况没有处理
            }
            else
            {
                var viewPhysicalPath = Kooboo.Web.Url.UrlUtility.MapPath(_viewTemplate);
                if (File.Exists(viewPhysicalPath))
                {
                    var body = IOUtility.ReadAsString(viewPhysicalPath);

                    Dictionary<string, object> globals = new Dictionary<string, object>(ViewData);

                    var types = GetReferencedTypes(globals);
                    var assemblies = GetReferencedAssemblies(types);
                    Template template = new Template(body, types, assemblies);
                    template.TemplateCache = templateCache;
                    // Global variables used in template

                    writer.Write(template.Render(globals));
                }

            }
        }
        protected virtual Dictionary<string, Type> GetReferencedTypes(IDictionary<string, object> globals)
        {
            var globalsTypes = new Dictionary<string, Type>();
            foreach (var kw in globals)
            {
                globalsTypes.Add(kw.Key, kw.Value.GetType());
            }
            globalsTypes.Add("ContentBase", typeof(Kooboo.CMS.Content.Models.ContentBase));
            globalsTypes.Add("DynamicDictionary", typeof(Kooboo.Collections.DynamicDictionary));
            return globalsTypes;
        }
        protected virtual List<Assembly> GetReferencedAssemblies(IDictionary<string, Type> globalTypes)
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(it => !it.IsDynamic && !it.FullName.Contains("Mono.Cecil")).ToList();

            //List<Assembly> assemblies = new List<Assembly>();
            //foreach (var type in globalTypes.Values)
            //{
            //    assemblies.Add(type.Assembly);
            //}
            //assemblies.Add(Assembly.Load("System.Dynamic, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"));
            //assemblies.Add(Assembly.Load("Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"));
            //return assemblies;
        }

        #region IViewDataContainer.ViewData
        private ViewDataDictionary _viewData;
        public ViewDataDictionary ViewData
        {
            get
            {
                if (_viewData == null)
                {
                    return _controllerContext.Controller.ViewData;
                }
                return _viewData;
            }
            set
            {
                _viewData = value;
            }
        }
        #endregion
    }
}
