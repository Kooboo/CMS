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
            return RedirectToAction("Cluster");
        }
        #endregion
        #region Relation
        public ActionResult Relation(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                if (!SearchSite(search))
                {
                    if (!SearchDatabase(search))
                    {
                        if (!SearchMembership(search))
                        {
                            FillData(new Kooboo.CMS.Content.Models.Repository[0],
                                new Kooboo.CMS.Member.Models.Membership[0],
                                new Kooboo.CMS.Sites.Models.Site[0]);
                        }
                    }
                }
            }
            else
            {
                var databases = _repositoryManager.All().Select(it => it).ToArray();
                var memberships = _membershipManager.All(null).Select(it => it).ToArray();
                var sites = _siteManager.All().Select(it => it.AsActual()).ToArray();
                FillData(databases, memberships, sites);
            }
            return View();
        }
        protected virtual bool SearchSite(string search)
        {
            var sites = _siteManager.All().Where(it => it.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)).Select(it => it.AsActual()).ToArray();
            if (sites.Length == 0)
            {
                return false;
            }
            var databases = sites.Select(it => it.GetRepository()).Select(it => it.AsActual()).Where(it => it != null).ToArray();
            var memberships = sites.Select(it => it.GetMembership()).Select(it => it.AsActual()).Where(it => it != null).ToArray();
            FillData(databases, memberships, sites);
            return true;
        }
        protected virtual bool SearchDatabase(string search)
        {
            var databases = _repositoryManager.All().Where(it => it.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).Select(it => it.AsActual()).ToArray();
            if (databases.Length == 0)
            {
                return false;
            }
            var allSites = _siteManager.All();
            var sites = allSites.Where(it => databases.Any(db => db.Name.EqualsOrNullEmpty(it.Repository, StringComparison.OrdinalIgnoreCase))).Select(it => it.AsActual()).Where(it => it != null).ToArray();
            var memberships = sites.Select(it => it.GetMembership()).Where(it => it != null).ToArray();
            FillData(databases, memberships, sites);
            return true;
        }
        protected virtual bool SearchMembership(string search)
        {
            var memberships = _membershipManager.All(search).Select(it => it.AsActual()).ToArray();
            if (memberships.Length == 0)
            {
                return false;
            }
            var allSites = _siteManager.All();
            var sites = allSites.Where(it => memberships.Any(mb => mb.Name.EqualsOrNullEmpty(it.Membership, StringComparison.OrdinalIgnoreCase))).Select(it => it.AsActual()).Where(it => it != null).ToArray();
            var databases = sites.Select(it => it.GetRepository()).Select(it => it.AsActual()).Where(it => it != null).ToArray();
            FillData(databases, memberships, sites);
            return true;
        }
        protected virtual void FillData(CMS.Content.Models.Repository[] databases, Member.Models.Membership[] memberships, CMS.Sites.Models.Site[] sites)
        {
            var index = 0;

            ViewBag.Sites = sites.OrderBy(it => it.FullName).Select(it => new
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
