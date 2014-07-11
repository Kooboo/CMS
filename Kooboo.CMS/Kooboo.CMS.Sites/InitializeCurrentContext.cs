#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.Models;

using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Routing;

using Kooboo.Common.Misc;

namespace Kooboo.CMS.Sites
{
    public class InitializeCurrentContext : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            Site.Current = null;
            Kooboo.CMS.Content.Models.Repository.Current = null;

            var siteName = filterContext.RequestContext.GetRequestValue("siteName");
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


            var repository = filterContext.RequestContext.GetRequestValue("repositoryName");
            if (!string.IsNullOrEmpty(repository))
            {
                var name = repository.ToString();
                if (!string.IsNullOrEmpty(name))
                {
                    Repository.Current = (new Repository(name)).AsActual();
                }
            }
            if (filterContext.HttpContext.User.Identity.IsAuthenticated == true)
            {
                SetUICulture(filterContext);
            }
        }
        #region Culture
        private void SetThreadCulture(Site site)
        {
            if (!string.IsNullOrEmpty(site.Culture))
            {
                var culture = CultureInfoHelper.CreateCultureInfo(site.Culture);
                Thread.CurrentThread.CurrentCulture = culture;
            }
        }

        private void SetUICulture(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            var user = Kooboo.CMS.Account.Services.ServiceFactory.UserManager.Get(filterContext.HttpContext.User.Identity.Name);
            var clientCulture = "en-us";
            if (filterContext.HttpContext.Request.UserLanguages != null && filterContext.HttpContext.Request.UserLanguages.Length > 0)
            {
                clientCulture = filterContext.HttpContext.Request.UserLanguages[0];
            }
            var uiCulture = CultureInfoHelper.CreateCultureInfo(clientCulture);
            if (user != null && !string.IsNullOrEmpty(user.UICulture))
            {
                uiCulture = CultureInfoHelper.CreateCultureInfo(user.UICulture);

            }
            Thread.CurrentThread.CurrentUICulture = uiCulture;
        }
        #endregion
    }
}
