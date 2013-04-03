using System;
using System.Collections;
using System.IO;
using System.Web.Mvc;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System.Web;
using NVelocity.Runtime.Resource.Loader;

namespace Kooboo.CMS.Sites.TemplateEngines.NVelocity.MvcViewEngine
{
    public class FileResourceLoaderEx : FileResourceLoader
    {
        public FileResourceLoaderEx() : base() { }
        private Stream FindTemplate(string filePath)
        {
            try
            {
                FileInfo file = new FileInfo(filePath);
                return new BufferedStream(file.OpenRead());
            }
            catch (Exception exception)
            {
                base.runtimeServices.Debug(string.Format("FileResourceLoader : {0}", exception.Message));
                return null;
            }
        }


        public override long GetLastModified(global::NVelocity.Runtime.Resource.Resource resource)
        {
            if (File.Exists(resource.Name))
            {
                FileInfo file = new FileInfo(resource.Name);
                return file.LastWriteTime.Ticks;
            }
            return base.GetLastModified(resource);
        }
        public override Stream GetResourceStream(string templateName)
        {
            if (File.Exists(templateName))
            {
                return FindTemplate(templateName);
            }
            return base.GetResourceStream(templateName);
        }
        public override bool IsSourceModified(global::NVelocity.Runtime.Resource.Resource resource)
        {
            if (File.Exists(resource.Name))
            {
                FileInfo file = new FileInfo(resource.Name);
                return (!file.Exists || (file.LastWriteTime.Ticks != resource.LastModified));
            }
            return base.IsSourceModified(resource);
        }
    }
    public class NVelocityViewEngine : VirtualPathProviderViewEngine, IViewEngine
    {
        public static NVelocityViewEngine Default = null;

        private static readonly IDictionary DEFAULT_PROPERTIES = new Hashtable();
        private readonly VelocityEngine _engine;

        static NVelocityViewEngine()
        {
            string targetViewFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "views");
            //DEFAULT_PROPERTIES.Add(RuntimeConstants.RESOURCE_LOADER, "file");
            DEFAULT_PROPERTIES.Add(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, targetViewFolder);
            DEFAULT_PROPERTIES.Add("file.resource.loader.class", "Kooboo.CMS.Sites.TemplateEngines.NVelocity.MvcViewEngine.FileResourceLoaderEx\\,Kooboo.CMS.Sites.TemplateEngines.NVelocity");


            Default = new NVelocityViewEngine();
        }

        public NVelocityViewEngine()
            : this(DEFAULT_PROPERTIES)
        {
        }

        public NVelocityViewEngine(IDictionary properties)
        {
            base.MasterLocationFormats = new string[] { "~/Views/{1}/{0}.vm", "~/Views/Shared/{0}.vm" };
            base.AreaMasterLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.vm", "~/Areas/{2}/Views/Shared/{0}.vm" };
            base.ViewLocationFormats = new string[] { "~/Views/{1}/{0}.vm", "~/Views/Shared/{0}.vm" };
            base.AreaViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.vm", "~/Areas/{2}/Views/Shared/{0}.vm" };
            base.PartialViewLocationFormats = base.ViewLocationFormats;
            base.AreaPartialViewLocationFormats = base.AreaViewLocationFormats;
            base.FileExtensions = new string[] { "vm" };


            if (properties == null) properties = DEFAULT_PROPERTIES;

            ExtendedProperties props = new ExtendedProperties();
            foreach (string key in properties.Keys)
            {
                props.AddProperty(key, properties[key]);
            }

            _engine = new VelocityEngine();
            _engine.Init(props);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            Template viewTemplate = _engine.GetTemplate(viewPath);
            Template masterTemplate = _engine.GetTemplate(masterPath);
            NVelocityView view = new NVelocityView(controllerContext, viewTemplate, masterTemplate);
            return view;
        }
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            Template viewTemplate = _engine.GetTemplate(partialPath);
            NVelocityView view = new NVelocityView(controllerContext, viewTemplate, null);
            return view;
        }
        public Template GetTemplate(string viewPath)
        {
            if (string.IsNullOrEmpty(viewPath))
            {
                return null;
            }
            return _engine.GetTemplate(Kooboo.Web.Url.UrlUtility.MapPath(viewPath));
        }
        //public void RenderView(ViewContext viewContext)
        //{
        //    CreateView(viewContext).RenderView();
        //}
        //#region Resole masterpage/view
        //public NVelocityView CreateView(ViewContext viewContext)
        //{
        //    string controllerName = (string)viewContext.RouteData.Values["controller"];
        //    string controllerFolder = controllerName;

        //    Template viewTemplate = ResolveView(viewContext);
        //    Template masterTemplate = ResolveMaster(viewContext);
        //    NVelocityView view = new NVelocityView(viewTemplate, masterTemplate, viewContext);
        //    return view;
        //}
        //private Template ResolveMaster(ViewContext viewContext)
        //{
        //    Template masterTemplate = null;
        //    if (!string.IsNullOrEmpty(viewContext.MasterName))
        //    {
        //        string masterPath = ViewLocator.GetMasterLocation(viewContext, viewContext.MasterName);

        //        if (!_engine.TemplateExists(masterPath))
        //            throw new InvalidOperationException("Could not find view for master template named " + viewContext.MasterName +
        //                                                ". I searched for '" + masterPath + "' file. Maybe the file doesn't exist?");
        //        masterTemplate = _engine.GetTemplate(masterPath);
        //    }
        //    return masterTemplate;
        //}

        //private Template ResolveView(ViewContext viewContext)
        //{
        //    string viewPath = ViewLocator.GetViewLocation(viewContext, viewContext.ViewName);
        //    if (!_engine.TemplateExists(viewPath))
        //        throw new InvalidOperationException("Could not find view " + viewContext.ViewName +
        //                                            ". I searched for '" + viewPath + "' file. Maybe the file doesn't exist?");
        //    return _engine.GetTemplate(viewPath);
        //}
        //#endregion


        //#region ViewLocator
        //IViewLocator _viewLocator = null;
        //public IViewLocator ViewLocator
        //{
        //    get
        //    {
        //        if (this._viewLocator == null)
        //        {
        //            this._viewLocator = new NVelocityViewLocator();
        //        }
        //        return this._viewLocator;
        //    }
        //    set
        //    {
        //        this._viewLocator = value;
        //    }
        //}
        //#endregion
    }
}
