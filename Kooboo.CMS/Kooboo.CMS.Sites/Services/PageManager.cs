#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Caching;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Search;
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.CMS.Sites.View;
using Kooboo.Extensions;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
namespace Kooboo.CMS.Sites.Services
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(PageManager))]
    public class PageManager : PathResourceManagerBase<Page, IPageProvider>
    {
        public PageManager(IPageProvider provider)
            : base(provider)
        {

            if (!(provider is Kooboo.CMS.Sites.Persistence.Caching.PageProvider))
            {
                throw new Exception("Expect caching provider");
            }
        }

        #region Providers
        public IVersionLogger<Page> VersiongLogger
        {
            get
            {
                return VersionManager.ResolveVersionLogger<Models.Page>();
            }
        }

        public IPagePublishingQueueProvider PagePublishingProvider
        {
            get
            {
                return Providers.PagePublishingProvider;
            }
        }
        #endregion

        #region Page

        /// <summary>
        /// Query the all pages including the sub pages.
        /// </summary>
        /// <param name="site"></param>
        /// <param name="filterName"></param>
        /// <returns></returns>
        public override IEnumerable<Page> All(Site site, string filterName)
        {
            var pages = Provider.All(site);
            if (!string.IsNullOrEmpty(filterName))
            {
                pages = pages.Select(it => it.AsActual())
                   .Where(it => it.Name.Contains(filterName, StringComparison.CurrentCultureIgnoreCase)
                       || (it.Navigation != null && !string.IsNullOrEmpty(it.Navigation.DisplayText) && it.Navigation.DisplayText.Contains(filterName, StringComparison.CurrentCultureIgnoreCase))
                       || it.VirtualPath.Contains(filterName, StringComparison.OrdinalIgnoreCase));
            }

            return pages.Select(it => it.AsActual()).OrderBy(it => it.Navigation.Order).ToArray();
        }
        public virtual IEnumerable<Page> All(Site site, string parentPageName, string filterName)
        {
            IEnumerable<Page> pages;
            if (string.IsNullOrEmpty(parentPageName))
            {
                pages = All(site, filterName);
            }
            else
            {
                pages = ChildPages(site, parentPageName, filterName);
            }
            return pages;
        }
        public virtual IEnumerable<Page> ChildPages(Site site, string parentPageName, string filterName)
        {
            List<Page> list = new List<Page>();
            var parentPage = new Page(site, PageHelper.SplitFullName(parentPageName).ToArray());
            var pages = ((IPageProvider)this.Provider).ChildPages(parentPage.LastVersion(site)).Select(it => it.LastVersion(site));

            if (!string.IsNullOrEmpty(filterName))
            {
                pages = pages.Select(it => it.AsActual())
                   .Where(it => it.Name.Contains(filterName, StringComparison.CurrentCultureIgnoreCase)
                       || (it.Navigation != null && !string.IsNullOrEmpty(it.Navigation.DisplayText) && it.Navigation.DisplayText.Contains(filterName, StringComparison.CurrentCultureIgnoreCase))
                       || it.VirtualPath.Contains(filterName, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var page in pages)
            {
                var o = Provider.Get(page);
                if (o != null)
                {
                    list.Add(o);
                }
            }
            return list.OrderBy(it => it.Navigation.Order);
        }
        public override Page Get(Site site, string fullName)
        {
            var page = Provider.Get((new Page(site, fullName)).LastVersion());

            return page;
        }
        public virtual Page Get(Site site, string name, string parent)
        {
            string fullName = name;
            if (!string.IsNullOrEmpty(parent))
            {
                fullName = PageHelper.CombineFullName(new string[] { parent, fullName });
            }
            return Get(site, fullName);
        }

        public virtual void Add(Site site, string parentPageName, Page page)
        {
            page.Site = site;
            if (!string.IsNullOrEmpty(parentPageName))
            {
                var parentPage = new Page(site, PageHelper.SplitFullName(parentPageName).ToArray());
                page.Parent = parentPage;
            }

            if (page.Exists() && page.IsLocalized(site))
            {
                throw new ItemAlreadyExistsException();
            }
            if (page.IsDefault == true)
            {
                ResetDefaultPage(site, page);
            }
            if (page.Navigation == null)
            {
                page.Navigation = new Navigation();
            }
            if (page.Navigation.Order == 0)
            {
                var count = All(site, parentPageName, "").Count();
                page.Navigation.Order = count + 1;
            }

            Provider.Add(page);

            if (page.Searchable)
            {
                SearchHelper.OpenService(site.GetRepository()).Add(page);
            }

            VersionManager.LogVersion(page);
        }

        public virtual SiteMap GetSiteMap(Site site)
        {
            var rootPage = GetDefaultPage(site);

            if (rootPage == null)
            {
                return new SiteMap();
            }

            var rootNode = new SiteMapNode() { Page = rootPage.AsActual() };

            rootNode.Children = All(site, "")
                .Select(it => GetPageNode(site, it))
                .OrderBy(it => it.Page.Navigation.Order);

            return new SiteMap() { Root = rootNode };
        }

        private SiteMapNode GetPageNode(Site site, Page page)
        {
            SiteMapNode node = new SiteMapNode() { Page = page.AsActual() };

            node.Children = ChildPages(site, page.FullName, "").Select(it => GetPageNode(site, it)).OrderBy(it => it.Page.Navigation.Order);

            return node;
        }

        public virtual Page Copy(Site site, string sourcePageFullName, string destPageFullName)
        {
            var destPage = PageHelper.Parse(site, destPageFullName);
            if (destPage.Exists())
            {
                throw new KoobooException("The page already exists.".Localize());
            }
            var page = this.Provider.Copy(site, sourcePageFullName, destPageFullName);

            page = page.AsActual();

            // Reset the display text.
            if (page.Navigation == null)
            {
                page.Navigation = new Navigation();
            }
            page.Navigation.DisplayText = "";
            page.IsDefault = false;

            return page;
        }

        public override void Update(Site site, Page @new, Page old)
        {
            @new.Site = site;
            base.Update(site, @new, old);
            if (@new.IsDefault == true)
            {
                ResetDefaultPage(site, @new);
            }

            if (@new.Searchable)
            {
                SearchHelper.OpenService(site.GetRepository()).Update(@new);
            }
            else
            {
                SearchHelper.OpenService(site.GetRepository()).Delete(@new);
            }
            VersionManager.LogVersion(@new);
        }

        private void ResetDefaultPage(Site site, Page currentDefaultPage)
        {
            foreach (var page in All(site, ""))
            {
                if (page.IsLocalized(site))
                {
                    var actualPage = page.AsActual();
                    if (actualPage != currentDefaultPage && actualPage.IsDefault == true)
                    {
                        actualPage.IsDefault = false;
                        base.Update(site, actualPage, actualPage);
                    }
                }
            }
        }


        public override void Remove(Site site, Page o)
        {
            base.Remove(site, o);

            SearchHelper.OpenService(site.GetRepository()).Delete(o);
        }
        #endregion

        #region Localize
        public virtual void Localize(string fullName, Site currentSite, string userName = null)
        {
            var targetPage = new Page(currentSite, fullName);
            var sourcePage = targetPage.LastVersion();

            if (targetPage.Site != sourcePage.Site)
            {
                Provider.Localize(sourcePage, currentSite);

                try
                {
                    UpdatePageCascading(targetPage, userName);
                }
                catch (Exception e)
                {
                    Kooboo.HealthMonitoring.Log.LogException(e);
                }
            }
        }
        private void UpdatePageCascading(Page page, string userName)
        {
            page = page.AsActual();
            if (page != null)
            {
                page.UserName = userName;
                Update(page.Site, page, page);
                foreach (var item in ChildPages(page.Site, page.FullName, null))
                {
                    UpdatePageCascading(item, userName);
                }
            }

        }
        public virtual void Unlocalize(Page page)
        {
            Remove(page.Site, page);
        }
        #endregion

        #region Import/export
        public virtual void Import(Site site, string name, Stream zipStream, bool @override)
        {
            Page parent = null;
            if (!string.IsNullOrEmpty(name))
            {
                parent = new Page(site, name);
            }
            Provider.Import(site, parent, zipStream, @override);
        }
        public virtual void Export(Site site, IEnumerable<Page> pages, System.IO.Stream outputStream)
        {
            Provider.Export(site, pages, outputStream);
        }
        #endregion

        #region IsStaticPage
        public virtual bool IsStaticPage(Site site, Page page)
        {
            page = page.AsActual();
            if (page.PageType == PageType.Default)
            {
                foreach (var item in AggregateDataRules(site, page))
                {
                    if (item.DataRule.HasAnyParameters())
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (page.PageType == PageType.Dynamic)
                {
                    return false;
                }
            }
            return true;
        }
        private IEnumerable<DataRuleSetting> AggregateDataRules(Site site, Page page)
        {
            page = page.AsActual();
            IEnumerable<DataRuleSetting> datarules = page.DataRules ?? new List<DataRuleSetting>();
            if (page.PagePositions != null)
            {
                var viewPositions = page.PagePositions.Where(it => it is ViewPosition).OrderBy(it => it.Order);
                foreach (ViewPosition viewPosition in viewPositions)
                {
                    var view = new Models.View(site, viewPosition.ViewName).LastVersion();

                    if (view.Exists())
                    {
                        datarules = datarules.Concat(view.AsActual().DataRules ?? new List<DataRuleSetting>());
                    }
                }

            }

            return datarules;
        }
        #endregion

        #region Move

        public virtual void Move(Site site, string pageFullName, string newParent, bool createRedirect, string user = "")
        {
            Page page = new Page(site, pageFullName);
            if (string.IsNullOrEmpty(newParent))
            {
                if (page.Parent == null)
                {
                    throw new KoobooException("The page is a root page already.".Localize());
                }
            }
            //backup the source page.
            Page sourcePage = new Page(site, pageFullName).AsActual();

            Provider.Move(site, pageFullName, newParent);

            Page newPage = null;
            if (!string.IsNullOrEmpty(newParent))
            {
                newPage = new Page(new Page(site, newParent), sourcePage.Name).AsActual();
            }
            else
            {
                newPage = new Page(site, sourcePage.Name).AsActual();
            }

            if (createRedirect)
            {
                UrlRedirect redirect = new UrlRedirect()
                {
                    InputUrl = sourcePage.VirtualPath,
                    OutputUrl = newPage.VirtualPath,
                    RedirectType = RedirectType.Moved_Permanently_301,
                    Regex = false,
                    UtcCreationDate = DateTime.UtcNow,
                    LastestEditor = user
                };
                ServiceFactory.UrlRedirectManager.Add(site, redirect);
            }
        }
        #endregion

        #region sort pages
        public virtual void SortPages(Site site, string parentPage, string[] orderedPageNames)
        {
            for (int i = 0; i < orderedPageNames.Length; i++)
            {
                var page = new Page(site, orderedPageNames[i]);
                page = page.AsActual();
                if (page != null)
                {
                    if (page.Navigation == null)
                    {
                        page.Navigation = new Navigation();
                    }
                    page.Navigation.Order = i;
                    Provider.Update(page, page);
                }
            }
        }
        #endregion

        #region Publish
        public virtual bool HasDraft(string pageName)
        {
            var page = new Page(Site.Current, pageName);
            return HasDraft(page);
        }
        public virtual bool HasDraft(Page page)
        {
            var draft = ServiceFactory.PageManager.Provider.GetDraft(page);
            return draft != null;
        }
        public virtual void Publish(Page page, bool publishDraft, string userName)
        {
            page = page.AsActual();
            if (page != null)
            {
                if (publishDraft)
                {
                    page = Provider.GetDraft(page);
                    Provider.RemoveDraft(page);
                }
                page.Published = true;
                page.UserName = userName;
                Provider.Update(page, page);
                VersionManager.LogVersion(page);
            }
        }
        public virtual void Publish(Page page, bool publishSchedule, bool publishDraft, bool period, DateTime publishDate, DateTime offlineDate, string userName)
        {
            if (!publishSchedule)
            {
                page = page.AsActual();
                if (publishDraft)
                {
                    page = Provider.GetDraft(page);
                    Provider.RemoveDraft(page);
                }
                page.Published = true;
                page.UserName = userName;
                Provider.Update(page, page);
                VersionManager.LogVersion(page);
            }
            else
            {
                PagePublishingQueueItem publishingItem = new PagePublishingQueueItem()
                {
                    Site = page.Site,
                    PageName = page.FullName,
                    PublishDraft = publishDraft,
                    CreationUtcDate = DateTime.UtcNow,
                    UtcDateToPublish = publishDate.ToUniversalTime(),
                    Period = period,
                    UtcDateToOffline = offlineDate.ToUniversalTime(),
                    UserName = userName
                };
                PagePublishingProvider.Add(publishingItem);
            }
        }
        public virtual void Unpublish(Page page, string userName)
        {
            page = page.AsActual();
            page.Published = false;
            page.UserName = userName;
            Provider.Update(page, page);
            VersionManager.LogVersion(page);
        }
        #endregion

        #region Front-End
        public virtual Page GetDefaultPage(Site site)
        {
            var pages = Provider.All(site);
            var defaultPage = pages.Select(it => it.AsActual()).Where(it => it.IsDefault == true).FirstOrDefault();
            if (defaultPage == null)
            {
                defaultPage = pages.FirstOrDefault();
            }
            return defaultPage;
        }

        /// <summary>
        /// 根据传入的URL来查找页面：
        /// 1. 标准查找
        /// 2. 在标准查找之后，再去做一遍查找
        ///  2.1 如果路径等于页面的绝对路径就不再继续查找
        ///  2.2 反之，根据剩下的路径长度，决定查询几层的别名为*的子页面，如果能一级一级找到，则返回子页面。如果找不到，则返回标准找到的结果。
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public virtual Page GetPageByUrl(Site site, string url)
        {
            if (string.IsNullOrEmpty(url) || url == "/")
            {
                return GetDefaultPage(site);
            }

            string[] paths = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var page = GetPage(site, paths);
            if (page != null)
            {
                var pagePaths = page.VirtualPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (pagePaths.Length >= paths.Length)
                {
                    return page;
                }
                var level = paths.Length - pagePaths.Length;
                var foundGeneric = false;
                var i = 1;
                while (level > 0)
                {
                    string remainsUrl = string.Join("/", paths.Skip(pagePaths.Length).Take(i));
                    var childPages = ChildPages(site, page.FullName, "").Select(it => it.LastVersion(site).AsActual()).ToArray();
                    var genericPage = childPages.Where(it => it.Route != null)
                        .Where(it => !string.IsNullOrEmpty(it.Route.Identifier))
                        .Where(it => it.Route.Identifier.StartsWith("#"))
                        .Where(it => it.Route.ToParsedRoute().Match(remainsUrl, it.Route.ToMvcRoute().Defaults) != null)
                        .FirstOrDefault();
                    if (genericPage == null)
                    {
                        genericPage = childPages
                        .Where(it => it.Route != null)
                        .Where(it => it.Route.Identifier == "*").FirstOrDefault();
                    }
                    if (genericPage == null)
                    {
                        break;
                    }
                    else
                    {
                        page = genericPage;
                        foundGeneric = true;
                    }
                    level--;
                    i++;
                }
                if (foundGeneric == false)
                {
                    //如果找不到通配页面（也就是找不到子页面用来适配URL参数的话），而且页面的没有设置任何路由的话，则认为该页面找到的是上一级的页面
                    //应该设置为空（未找到）
                    if (page.Route == null || string.IsNullOrEmpty(page.Route.RoutePath))
                    {
                        page = null;
                    }
                }
            }
            return page;
        }

        private Page GetPage(Site site, string[] pagePaths)
        {
            Page page = null;
            Page previousPage = null;
            for (int i = 1; i <= pagePaths.Length; i++)
            {
                var paths = pagePaths.Take(i).ToArray();

                page = GetPageByPathsNoRecursion(site, paths);

                if (page == null)
                {
                    if (previousPage != null)
                    {
                        break;
                    }
                }
                else
                {
                    previousPage = page;
                }
            }
            if (previousPage != null)
            {
                previousPage = previousPage.AsActual();
            }
            return previousPage;
        }
        private Page GetPageByPathsNoRecursion(Site site, string[] pagePaths)
        {
            string urlPath = Kooboo.Web.Url.UrlUtility.UrlSeparatorChar + Kooboo.Web.Url.UrlUtility.Combine(pagePaths);

            string cachedKey = "GetPageByPathsNoRecursion:" + urlPath.ToLower();

            var page = site.ObjectCache().Get(cachedKey) as Page;

            if (page == null)
            {
                // performance leaks, one path will take 18ms...
                page = GetPageByUrlIdentifier(site, urlPath);
                //if (page == null)
                //{
                //    page = new Page(site, pagePaths);
                //    var last = page.LastVersion();
                //    if (!last.Exists())
                //    {
                //        page = null;
                //    }
                //}
                if (page != null)
                {
                    site.ObjectCache().Add(cachedKey, page, new System.Runtime.Caching.CacheItemPolicy() { SlidingExpiration = TimeSpan.Parse("00:30:00") });
                }
            }

            return page;
        }

        public virtual Page GetPageByUrlIdentifier(Site site, string identifier)
        {
            foreach (var page in Provider.All(site))
            {
                var found = GetPageByUrlIdentifier(site, page, identifier);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }
        private Page GetPageByUrlIdentifier(Site site, Page page, string identifier)
        {
            var lastPage = page.LastVersion(site).AsActual();
            if (lastPage != null)
            {
                if (lastPage.VirtualPath.EqualsOrNullEmpty(identifier, StringComparison.OrdinalIgnoreCase))
                {
                    return lastPage;
                }
                foreach (var item in Provider.ChildPages(page))
                {
                    var found = GetPageByUrlIdentifier(site, item, identifier);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }

            return null;
        }
        #endregion

        #region GetUnsyncedSubPage
        public IEnumerable<Page> GetUnsyncedSubPages(Site site, string fullPageName)
        {
            if (site.Parent != null)
            {
                var page = new Page(site, fullPageName).LastVersion(site);
                if (page.IsLocalized(site))
                {
                    var parent = site.Parent;
                    List<Page> pagesInParentSites = new List<Page>();

                    while (parent != null)
                    {
                        var parentPage = new Page(parent, fullPageName).LastVersion(parent);
                        if (parentPage != null && parentPage.IsLocalized(parent))
                        {
                            pagesInParentSites.AddRange(this.ChildPages(parent, fullPageName, null));
                        }
                        parent = parent.Parent;
                    }

                    var childPages = this.ChildPages(site, fullPageName, null);

                    return pagesInParentSites.Where(it => !childPages.Any(cp => it.FullName.EqualsOrNullEmpty(cp.FullName, StringComparison.OrdinalIgnoreCase)));
                }
            }

            return new Page[0];
        }
        #endregion

        #region AllPagesFlattened
        public virtual IEnumerable<Page> AllPagesFlattened(Site site)
        {
            List<Page> folders = new List<Page>();

            foreach (var item in Provider.All(site))
            {
                AggregateFolderRecurisively(item, ref folders);
            }

            return folders;
        }
        private void AggregateFolderRecurisively(Page page, ref List<Page> list)
        {
            list.Add(page);
            foreach (var item in Provider.ChildPages(page))
            {
                AggregateFolderRecurisively(item, ref list);
            }
        }
        #endregion
    }
}
