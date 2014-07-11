#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Web.Mvc;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Routing;

using Kooboo.Common.Globalization;
using Kooboo.Common.Web;
using Kooboo.Common.Misc;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites
{
    [ValidateInput(false)]
    public class AreaControllerBase : Controller
    {
        #region Initialize
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Site.Current = null;
            Repository.Current = null;

            var siteName = requestContext.GetRequestValue("siteName");
            if (siteName != null)
            {
                var name = siteName.ToString();
                if (!string.IsNullOrEmpty(name))
                {
                    Site.Current = (SiteHelper.Parse(siteName)).AsActual();
                    if (Site.Current != null)
                    {
                        SetThreadCulture(Site.Current);
                        if (Site.Current.GetRepository() != null)
                        {
                            Repository.Current = Site.Current.GetRepository().AsActual();
                        }
                    }
                }
            }


            var repository = requestContext.GetRequestValue("repositoryName");
            if (!string.IsNullOrEmpty(repository))
            {
                var name = repository.ToString();
                if (!string.IsNullOrEmpty(name))
                {
                    Repository.Current = (new Repository(name)).AsActual();
                }
            }
            if (User.Identity.IsAuthenticated == true)
            {
                SetUICulture();
            }
        }
        #endregion

        #region  OnAuthorization
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var forceSSL = false;

            var forceSSLSetting = System.Configuration.ConfigurationManager.AppSettings["forceSSL_AdminPages"];
            if (!string.IsNullOrEmpty(forceSSLSetting))
            {
                forceSSL = forceSSLSetting.ToLower() == "true";
            }

            if (forceSSL && !filterContext.HttpContext.Request.IsSecureConnection)
            {
                this.HandleNonHttpsRequest(filterContext);
            }

        }
        protected virtual void HandleNonHttpsRequest(AuthorizationContext filterContext)
        {
            if (!string.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("MvcResources.RequireHttpsAttribute_MustUseSsl");
            }
            string url = "https://" + filterContext.HttpContext.Request.Url.Host + filterContext.HttpContext.Request.RawUrl;
            filterContext.Result = new RedirectResult(url);
        }


        #endregion

        #region Culture
        private void SetThreadCulture(Site site)
        {
            if (!string.IsNullOrEmpty(site.Culture))
            {
                var culture = CultureInfoHelper.CreateCultureInfo(site.Culture);
                Thread.CurrentThread.CurrentCulture = culture;
            }
        }

        private void SetUICulture()
        {
            var user = Kooboo.CMS.Account.Services.ServiceFactory.UserManager.Get(User.Identity.Name);
            var clientCulture = "en-us";
            if (Request.UserLanguages != null && Request.UserLanguages.Length > 0)
            {
                clientCulture = Request.UserLanguages[0];
            }
            var uiCulture = CultureInfoHelper.CreateCultureInfo(clientCulture);
            if (user != null && !string.IsNullOrEmpty(user.UICulture))
            {
                uiCulture = CultureInfoHelper.CreateCultureInfo(user.UICulture);

            }
            Thread.CurrentThread.CurrentUICulture = uiCulture;
        }
        #endregion

        #region Json
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                if (Request.ContentType.Contains("multipart/form-data;", StringComparison.CurrentCultureIgnoreCase))
                {
                    contentType = "text/plain";
                }
            }
            return base.Json(data, contentType, contentEncoding, behavior);
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var result = new JsonResultData();
                result.AddException(filterContext.Exception);
                filterContext.Result = Json(result, JsonRequestBehavior.AllowGet);
                filterContext.ExceptionHandled = true;
            }
        }
        #endregion

        #region Site
        public virtual Site Site
        {
            get
            {
                return Site.Current;
            }
            set
            {
                Site.Current = value;
            }
        }
        #endregion
    }
}