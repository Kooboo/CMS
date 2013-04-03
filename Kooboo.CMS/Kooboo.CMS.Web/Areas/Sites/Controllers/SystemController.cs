using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web.Models;
using Kooboo.Globalization;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Content.Caching;
using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Caching;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "Settings", Order = 1)]
    public class SystemController : AdminControllerBase
    {
        #region Index
        public virtual ActionResult Index()
        {
            return View(Site.AsActual());
        }
        [HttpPost]
        public virtual ActionResult Index(Site model)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                if (ModelState.IsValid)
                {
                    var site = Site.AsActual();

                    //if (isNew)
                    //{
                    //    CreateRepository(model.Repository);
                    //}

                    site.DisplayName = model.DisplayName;
                    site.Culture = model.Culture;

                    site.Domains = model.Domains;
                    site.SitePath = model.SitePath;
                    site.Mode = model.Mode;
                    site.Repository = model.Repository;
                    site.Theme = model.Theme;
                    site.Version = model.Version;
                    site.InlineEditing = model.InlineEditing;
                    site.ShowSitemap = model.ShowSitemap;
                    site.Smtp = model.Smtp;
                    site.Security = model.Security;
                    site.ResourceDomain = model.ResourceDomain;
                    if (site.Smtp != null && site.Smtp.To != null)
                    {
                        site.Smtp.To = site.Smtp.To.Where(it => !string.IsNullOrEmpty(it)).ToArray();
                    }
                    site.CustomFields = model.CustomFields;
                    site.EnableVersioning = model.EnableVersioning;
                    site.EnableJquery = model.EnableJquery;
                    site.EnableStyleEdting = model.EnableStyleEdting;

                    ServiceFactory.SiteManager.Update(site);
                    resultEntry.AddMessage("Site setting has been changed.".Localize());
                }

            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry);
        }
        #endregion

        #region ClearCache
        [HttpPost]
        public virtual ActionResult ClearCache()
        {
            JsonResultEntry resultEntry = new JsonResultEntry();

            try
            {
                Site.ClearCache();
                var repository = Site.GetRepository();
                if (repository != null)
                {
                    repository.ClearCache();
                }
                CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();

                resultEntry.AddMessage("The site cache has been cleared.");
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }

            return Json(resultEntry);
        }
        #endregion


    }
}
