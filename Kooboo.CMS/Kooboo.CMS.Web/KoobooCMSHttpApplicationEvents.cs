#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Web.Models;
using Kooboo.Collections;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web
{
    [Dependency(typeof(Kooboo.CMS.Common.IHttpApplicationEvents), Key = "KooboCMSHttpApplicationEvents")]
    public class KoobooCMSHttpApplicationEvents : Kooboo.CMS.Common.HttpApplicationEvents
    {
        #region CustomRazorViewEngine
        /// <summary>
        /// Allow the area to use the global shared view .
        /// </summary>
        private class CustomRazorViewEngine : RazorViewEngine
        {
            public CustomRazorViewEngine()
                : base()
            {
                var baseDir = EngineContext.Current.Resolve<Kooboo.CMS.Common.IBaseDir>();

                base.AreaMasterLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/" + baseDir.Cms_DataPathName + "/Views/Shared/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml", "~/Views/Shared/{0}.cshtml" }; //add: "~/Views/Shared/{0}.cshtml" 

                base.AreaViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/" + baseDir.Cms_DataPathName + "/Views/Shared/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml", "~/Views/Shared/{0}.cshtml" };//add: "~/Views/Shared/{0}.cshtml"

                base.AreaPartialViewLocationFormats = base.AreaViewLocationFormats;
            }
        }
        #endregion

        #region RegisterRoutes
        public static void RegisterRoutes(RouteCollection routes)
        {
            Kooboo.Web.Mvc.Routing.RouteTableRegister.RegisterRoutes(routes);

            ModelMetadataProviders.Current = new KoobooDataAnnotationsModelMetadataProvider();

            ModelValidatorProviders.Providers.RemoveAt(0);
            ModelValidatorProviders.Providers.Insert(0, new KoobooDataAnnotationsModelValidatorProvider());

            //Job.Jobs.Instance.AttachJob("test", new Kooboo.Job.TestJob(), 1000, null, true);
        }
        #endregion

        #region Application_Start
        public override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);

            //execute the initializer method.
            ApplicationInitialization.Execute();

            #region mono
#if MONO
            Kooboo.HealthMonitoring.Log.Logger = (exception) =>
            {

                string msgFormat = @"
Event message: {0}
Event time: {1}
Event time {2}
Exception information:
    Exception type: {3}
    Exception message: {0}

Request information:
    Request URL: {4}
    User host address: {5}
    User: {6}
    Is authenticated: {7}
Thread information:
    Thread ID: {8}    
    Stack trace: {9}
";
                string[] args = new string[13];
                args[0] = exception.Message;
                args[1] = DateTime.Now.ToString(System.Globalization.CultureInfo.InstalledUICulture);
                args[2] = DateTime.UtcNow.ToString(System.Globalization.CultureInfo.InstalledUICulture);
                args[3] = e.GetType().ToString();
                if (System.Web.HttpContext.Current != null)
                {
                    var request = HttpContext.Current.Request;
                    args[4] = request.RawUrl;
                    args[5] = request.UserHostAddress;
                    args[6] = HttpContext.Current.User.Identity.Name;
                    args[7] = HttpContext.Current.User.Identity.IsAuthenticated.ToString();
                }
                args[8] = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
                args[9] = exception.StackTrace;

                Kooboo.CMS.Web.HealthMonitoring.TextFileLogger.Log(string.Format(msgFormat, args));
            };
#endif
            #endregion

            //
            ControllerBuilder.Current.SetControllerFactory(new Kooboo.CMS.Sites.CMSControllerFactory());

            #region MVC Inject
            DependencyResolver.SetResolver(new Kooboo.CMS.Common.DependencyResolver(EngineContext.Current, DependencyResolver.Current));
            #endregion

            //ViewEngine for module.            
            ViewEngines.Engines.Insert(0, new Kooboo.CMS.Sites.Extension.ModuleArea.ModuleRazorViewEngine());
            ViewEngines.Engines.Insert(1, new Kooboo.CMS.Sites.Extension.ModuleArea.ModuleWebFormViewEngine());
            ViewEngines.Engines.Insert(2, new CustomRazorViewEngine());


            AreaRegistration.RegisterAllAreas();

            #region Binders
            ModelBinders.Binders.DefaultBinder = new JsonModelBinder();

            ModelBinders.Binders.Add(typeof(DynamicDictionary), new DynamicDictionaryBinder());
            ModelBinders.Binders.Add(typeof(Kooboo.CMS.Sites.DataRule.IDataRule), new Kooboo.CMS.Web.Areas.Sites.ModelBinders.DataRuleBinder());
            ModelBinders.Binders.Add(typeof(Kooboo.CMS.Sites.DataRule.DataRuleBase), new Kooboo.CMS.Web.Areas.Sites.ModelBinders.DataRuleBinder());
            ModelBinders.Binders.Add(typeof(Kooboo.CMS.Sites.Models.PagePosition), new Kooboo.CMS.Web.Areas.Sites.ModelBinders.PagePositionBinder());
            ModelBinders.Binders.Add(typeof(Kooboo.CMS.Sites.Models.Parameter), new Kooboo.CMS.Web.Areas.Sites.ModelBinders.ParameterBinder());
            #endregion

            RegisterRoutes(RouteTable.Routes);


            Kooboo.Web.Mvc.Menu.MenuFactory.RegisterAreaMenu("AreasMenu", Path.Combine(Settings.BaseDirectory, "Menu.config"));

            Kooboo.CMS.Content.Persistence.Providers.RepositoryProvider.TestDbConnection();
        }
        #endregion
    }
}
