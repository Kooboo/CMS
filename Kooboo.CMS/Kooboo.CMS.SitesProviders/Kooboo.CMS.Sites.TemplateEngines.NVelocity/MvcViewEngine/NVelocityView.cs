#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using NVelocity;
using Kooboo.CMS.Sites.Globalization;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Search;
using Kooboo.CMS.Search.Models;
using Kooboo.Common.Data;

namespace Kooboo.CMS.Sites.TemplateEngines.NVelocity.MvcViewEngine
{
    public class NVelocitySearchHelper
    {
        static NVelocitySearchHelper()
        {
            ServiceBuilder = new ServiceBuilder();
        }
        public static IServiceBuilder ServiceBuilder { get; set; }

        public static ISearchService OpenService(Repository repository)
        {
            return ServiceBuilder.OpenService(repository);
        }

        public static IPagedList<ResultObject> Search(Repository repository, string key, int pageIndex, int pageSize)
        {
            return OpenService(repository).Search(key, pageIndex, pageSize);
        }
    }
    /// <summary>
    /// NVelocity的ViewPage
    /// 
    /// </summary>
    public class NVelocityView : IViewDataContainer, IView
    {
        private ControllerContext _controllerContext;
        private readonly Template _masterTemplate;
        private readonly Template _viewTemplate;

        public NVelocityView(ControllerContext controllerContext, string viewPath, string masterPath)
            : this(controllerContext, NVelocityViewEngine.Default.GetTemplate(viewPath), NVelocityViewEngine.Default.GetTemplate(masterPath))
        {

        }
        public NVelocityView(ControllerContext controllerContext, Template viewTemplate, Template masterTemplate)
        {
            _controllerContext = controllerContext;
            _viewTemplate = viewTemplate;
            _masterTemplate = masterTemplate;
        }

        public Template ViewTemplate
        {
            get { return _viewTemplate; }
        }

        public Template MasterTemplate
        {
            get { return _masterTemplate; }
        }

        private VelocityContext CreateContext(ViewContext context)
        {
            Hashtable entries = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
            if (context.ViewData != null)
            {
                foreach (var pair in context.ViewData)
                {
                    entries[pair.Key] = GetDuckObject(pair.Value);
                }
            }
            entries["ViewBag"] = context.Controller.ViewBag;
            entries["viewdata"] = context.ViewData;
            entries["tempdata"] = context.TempData;
            entries["routedata"] = context.RouteData;
            entries["controller"] = context.Controller;
            entries["httpcontext"] = context.HttpContext;
            entries["viewbag"] = context.ViewData;
            CreateAndAddHelpers(entries, context);
            AddCMSHelpers(entries);

            return new VelocityContext(entries);
        }
        private object GetDuckObject(object o)
        {
            if (o is TextContent)
            {
                return new TextContentExtensionDuck((TextContent)o);
            }
            if (o is IEnumerable<TextContent>)
            {
                return ((IEnumerable<TextContent>)o).Select(it => new TextContentExtensionDuck(it));
            }
            return o;
        }
        private void AddCMSHelpers(Hashtable entries)
        {
            entries["ViewHelper"] = new ViewHelper();
            entries["MenuHelper"] = new MenuHelper();
            entries["SubmissionHelper"] = new SubmissionHelper();
            entries["ServiceFactory"] = new Kooboo.CMS.Sites.Services.ServiceFactory();
            entries["Page_Context"] = Page_Context.Current;
            entries["ContentHelper"] = new ContentHelper();
            entries["Repository"] = Repository.Current;
            entries["SearchHelper"] = new NVelocitySearchHelper();
        }
        private void CreateAndAddHelpers(Hashtable entries, ViewContext context)
        {
            entries["html"] = entries["htmlhelper"] = new HtmlExtensionDuck(context, this);
            entries["url"] = entries["urlhelper"] = new UrlExtensionDuck(context.RequestContext);
            entries["ajax"] = entries["ajaxhelper"] = new AjaxHelper(context, this);
        }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            this.ViewData = viewContext.ViewData;

            bool hasLayout = _masterTemplate != null;

            VelocityContext context = CreateContext(viewContext);

            if (hasLayout)
            {
                StringWriter sw = new StringWriter();
                _viewTemplate.Merge(context, sw);

                context.Put("childContent", sw.GetStringBuilder().ToString());

                _masterTemplate.Merge(context, writer);
            }
            else
            {
                _viewTemplate.Merge(context, writer);
            }
        }

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
    }
}