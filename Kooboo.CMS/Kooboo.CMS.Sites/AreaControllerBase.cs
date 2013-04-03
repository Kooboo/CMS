using System;
using System.Web.Mvc;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.Models;
using Kooboo.Extensions;
using Kooboo.Web.Mvc;
using System.Globalization;
using System.Threading;
using Kooboo.CMS.Sites;
namespace Kooboo.CMS.Sites
{
    [ValidateInput(false)]
    public class AreaControllerBase : Controller
    {
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
                    Site.Current = Kooboo.CMS.Sites.Models.IPersistableExtensions.AsActual(SiteHelper.Parse(siteName));
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
                    Repository.Current = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(new Repository(name));
                }
            }

            SetUICulture();
        }
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
                var result = new JsonResultEntry();
                result.AddException(filterContext.Exception);
                filterContext.Result = Json(result, JsonRequestBehavior.AllowGet);
                filterContext.ExceptionHandled = true;
            }
        }
    }
}