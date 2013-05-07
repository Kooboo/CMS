#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web.Authorizations;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [RequiredLogOn]
    public class HomeController : Kooboo.CMS.Sites.AreaControllerBase
    {
        public virtual ActionResult Index(string search, int? page, int? pageSize)
        {
            var sites = ServiceFactory.SiteManager.SiteTrees(User.Identity.Name);
            if (!string.IsNullOrEmpty(search))
            {
                sites = sites.Where(it => it.Root.Site.Name.ToLower().Contains(search.ToLower())
                    || (!string.IsNullOrEmpty(it.Root.Site.AsActual().DisplayName) && it.Root.Site.AsActual().DisplayName.ToLower().Contains(search.ToLower())));
            }

            var model = sites.ToPagedList<SiteTree>(page ?? 1, pageSize ?? 20);
            return View(model);
        }
        public virtual ActionResult SiteMap()
        {
            var siteName = ControllerContext.RequestContext.GetRequestValue("siteName");
            if (string.IsNullOrEmpty(siteName))
            {
                return RedirectToAction("Index", "Home");
            }
            var site = SiteHelper.Parse(siteName).AsActual();
            if (site == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (site.ShowSitemap.HasValue && site.ShowSitemap.Value == false)
            {
                if (ServiceFactory.UserManager.Authorize(site, User.Identity.Name, Kooboo.CMS.Account.Models.Permission.Sites_Page_EditPermission))
                {
                    return RedirectToAction("Index", this.ControllerContext.RequestContext.AllRouteValues().Merge("Controller", "Page"));
                }
                else
                {
                    return RedirectToAction("Index", this.ControllerContext.RequestContext.AllRouteValues().Merge("Controller", "Home").Merge("Area", "Contents"));
                }
            }

            Site.Current = site;

            var siteMap = ServiceFactory.PageManager.GetSiteMap(site);

            var layoutList = ServiceFactory.LayoutManager.All(site, "");

            ViewData["LayoutList"] = layoutList;

            return View(siteMap);
        }
    }
}
