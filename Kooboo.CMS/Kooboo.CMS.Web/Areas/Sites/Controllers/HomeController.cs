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
        #region .ctor
        Kooboo.CMS.Content.Services.RepositoryManager _repositoryManager = null;
        Kooboo.CMS.Member.Services.MembershipManager _membershipManager = null;
        Kooboo.CMS.Sites.Services.SiteManager _siteManager = null;

        public HomeController(Kooboo.CMS.Content.Services.RepositoryManager repositoryManager, Kooboo.CMS.Member.Services.MembershipManager membershipManager, Kooboo.CMS.Sites.Services.SiteManager siteManager)
        {
            this._repositoryManager = repositoryManager;
            this._membershipManager = membershipManager;
            this._siteManager = siteManager;
        }
        #endregion

        #region Index
        public ActionResult Index()
        {
            var databases = _repositoryManager.All().Select(it => it).ToArray();
            var memberships = _membershipManager.All(null).Select(it => it).ToArray();
            var sites = _siteManager.All().Select(it => it.AsActual()).ToArray();

            var index = 0;
            ViewBag.Sites = sites.Select(it => new
            {
                num = index++,
                name = it.FullName,
                text = it.FriendlyName,
                content = FindIndex(databases, db => db.Name.EqualsOrNullEmpty(it.Repository, StringComparison.OrdinalIgnoreCase)),
                membership = FindIndex(memberships, mb => mb.Name.EqualsOrNullEmpty(it.Membership, StringComparison.OrdinalIgnoreCase)),
                url = Url.Action("SiteMap", new { siteName = it.FullName })
            }).ToArray();

            ViewBag.Databases = databases.Select(it => new
            {
                name = it.Name,
                text = string.IsNullOrEmpty(it.DisplayName) ? it.Name : it.DisplayName,
                url = Url.Action("Guide", new { controller = "Repository", area = "Contents", repositoryName = it.Name })
            }).ToArray();

            ViewBag.Memberships = memberships.Select(it => new
            {
                name = it.Name,
                text = it.Name,
                url = Url.Action("Go", new { controller = "Membership", area = "Membership", membershipName = it.Name })
            }).ToArray();
            return View();
        }
        public static int FindIndex<T>(IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }
        #endregion

        #region Cluster
        public virtual ActionResult Cluster(string search, int? page, int? pageSize)
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
        #endregion

        #region SiteMap
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
        #endregion
    }
}
