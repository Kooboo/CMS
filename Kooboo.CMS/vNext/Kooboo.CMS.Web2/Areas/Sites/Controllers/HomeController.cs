#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.CMS.SiteKernel.Services;
using Kooboo.CMS.Web2.Authorizations;
using Kooboo.CMS.Web2.Button;
using Kooboo.Common.Data;
using Kooboo.Common.Web.Button;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Kooboo.CMS.Web2.Areas.Sites.Controllers
{
    [RequiredLogOn]
    [ResolveTopButtonsFilter]
    public class HomeController : Kooboo.CMS.Sites.AreaControllerBase
    {
        #region .ctor
        SiteService siteService;
        PageService pageService;
        IButtonPluginExecutor buttonPluginExecutor;

        public HomeController(SiteService siteService, PageService pageService, IButtonPluginExecutor buttonPluginExecutor)
        {
            this.siteService = siteService;
            this.pageService = pageService;
            this.buttonPluginExecutor = buttonPluginExecutor;
        }
        #endregion

        #region Index
        public ActionResult Index()
        {
            return RedirectToAction("Cluster");
        }
        #endregion

        #region Cluster

        public virtual ActionResult Cluster(string search, int? page, int? pageSize)
        {
            var sites = siteService.SiteTrees(User.Identity.Name);
            if (!string.IsNullOrEmpty(search))
            {
                sites = sites.Where(it => it.Site.Name.ToLower().Contains(search.ToLower())
                    || (!string.IsNullOrEmpty(it.Site.DisplayName) && it.Site.AsActual().DisplayName.ToLower().Contains(search.ToLower())));
            }
            var model = sites.ToPagedList<SiteNode>(page ?? 1, pageSize ?? 20);


            ViewBag.SiteNodeButtons = buttonPluginExecutor.LoadButtons(this.ControllerContext, Kooboo.CMS.SiteKernel.Extension.Site.SiteExtensionPoints.SiteNodeButton);

            return View(model);
        }
        #endregion

        #region SiteMap
        public virtual ActionResult SiteMap()
        {
            var siteName = ControllerContext.RequestContext.GetRequestValue("siteName");
            if (string.IsNullOrEmpty(siteName))
            {
                return RedirectToAction("Index", "Home");
            }
            var site = siteService.Get(new Site(siteName));
            if (site == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Site.Current = site;

            var siteMap = pageService.GetSiteMap(site);

            //var layoutList = ServiceFactory.LayoutManager.All(site, "");

            //ViewData["LayoutList"] = layoutList;

            return View(siteMap);
        }
        #endregion
    }
}
