using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel.DataAnnotations;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Web.Models;
using System.Web.Script.Serialization;
using System.IO;
using System.Security.Permissions;
using System.Security;

namespace Kooboo.CMS.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Allow the area to use the global shared view .
        /// </summary>
        private class CustomWebFormViewEngine : WebFormViewEngine
        {
            public CustomWebFormViewEngine()
                : base()
            {
                base.AreaMasterLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.master", "~/Areas/{2}/Views/Shared/{0}.master", "~/Views/Shared/{0}.master" };
                base.AreaViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.aspx", "~/Areas/{2}/Views/{1}/{0}.ascx", "~/Areas/{2}/Views/Shared/{0}.aspx", "~/Areas/{2}/Views/Shared/{0}.ascx", "~/Views/Shared/{0}.ascx" };
                base.AreaPartialViewLocationFormats = base.AreaViewLocationFormats;

            }
        }
        /// <summary>
        /// Allow the area to use the global shared view .
        /// </summary>
        private class CustomRazorViewEngine : RazorViewEngine
        {
            public CustomRazorViewEngine()
                : base()
            {
                base.AreaMasterLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/{1}/{0}.vbhtml", "~/Areas/{2}/Views/Shared/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.vbhtml", "~/Views/Shared/{0}.cshtml", "~/Views/Shared/{0}.vbhtml" };

                base.AreaViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/{1}/{0}.vbhtml", "~/Areas/{2}/Views/Shared/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.vbhtml", "~/Views/Shared/{0}.cshtml", "~/Views/Shared/{0}.vbhtml" };

                base.AreaPartialViewLocationFormats = base.AreaViewLocationFormats;
            }
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            Kooboo.Web.Mvc.Routing.RouteTableRegister.RegisterRoutes(routes);

            ModelMetadataProviders.Current = new KoobooDataAnnotationsModelMetadataProvider();

            ModelValidatorProviders.Providers.RemoveAt(0);
            ModelValidatorProviders.Providers.Insert(0, new KoobooDataAnnotationsModelValidatorProvider());

            //Job.Jobs.Instance.AttachJob("test", new Kooboo.Job.TestJob(), 1000, null, true);
        }

        protected void Application_Start()
        {
            //execute the initializer method.
            ApplicationInitialization.Execute();        

            #region mono
#if MONO
			Kooboo.HealthMonitoring.Log.Logger = (e)=>{
				
				string msgFormat= @"
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
				args[0]= e.Message;
                args[1] = DateTime.Now.ToString(System.Globalization.CultureInfo.InstalledUICulture);
                args[2] = DateTime.UtcNow.ToString(System.Globalization.CultureInfo.InstalledUICulture);
				args[3]=e.GetType().ToString ();
				if (System.Web.HttpContext.Current!=null) {
					var request = HttpContext.Current.Request;
					args[4]= request.RawUrl;
						args[5]= request.UserHostAddress;
					args[6]= HttpContext.Current.User.Identity.Name;
					args[7]= HttpContext.Current.User.Identity.IsAuthenticated.ToString ();					
				}
				args[8]=System.Threading.Thread.CurrentThread.ManagedThreadId.ToString ();				
				args[9] =e.StackTrace;
				
				Kooboo.CMS.Web.HealthMonitoring.TextFileLogger.Log(string.Format(msgFormat,args));
			};
#endif
            #endregion

            //
            ControllerBuilder.Current.SetControllerFactory(new Kooboo.CMS.Sites.CMSControllerFactory());

            //ViewEngine for module.            
            ViewEngines.Engines.Insert(0, new Kooboo.CMS.Sites.Extension.ModuleArea.ModuleRazorViewEngine());
            ViewEngines.Engines.Insert(1, new Kooboo.CMS.Sites.Extension.ModuleArea.ModuleWebFormViewEngine());
            ViewEngines.Engines.Insert(2, new CustomRazorViewEngine());
            ViewEngines.Engines.Insert(3, new CustomWebFormViewEngine());


            AreaRegistration.RegisterAllAreas();

            ModelBinders.Binders.Add(typeof(Dynamic.DynamicDictionary), new DynamicDictionaryBinder());
            //ModelBinders.Binders.Add(typeof(Dictionary<string, string>), new StringDictionaryBinder());

            RegisterRoutes(RouteTable.Routes);

            Kooboo.CMS.Content.Persistence.Providers.RepositoryProvider.TestDbConnection();
        }
    }
}