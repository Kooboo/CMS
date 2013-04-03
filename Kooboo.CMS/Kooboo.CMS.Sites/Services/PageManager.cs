using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.Extensions;
using System.IO;
using Kooboo.Globalization;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.CMS.Search;
using Kooboo.CMS.Search.Models;
using System.Collections.Specialized;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Caching;
using Kooboo.CMS.Sites.Caching;
namespace Kooboo.CMS.Sites.Services
{
    #region PageConverter
    public class PageConverter : IObjectConverter
    {
        public KeyValuePair<string, string> GetKeyField(object o)
        {
            Page page = (Page)o;
            return new KeyValuePair<string, string>("PageName", page.FullName);
        }

        public Search.Models.IndexObject GetIndexObject(object o)
        {
            IndexObject indexObject = null;

            Page page = (Page)o;
            NameValueCollection storeFields = new NameValueCollection();
            NameValueCollection sysFields = new NameValueCollection();

            sysFields.Add("SiteName", page.Site.FullName);
            sysFields.Add("PageName", page.FullName);

            string title = "";
            StringBuilder body = new StringBuilder();

            if (page.HtmlMeta != null && !string.IsNullOrEmpty(page.HtmlMeta.HtmlTitle))
            {
                title = page.HtmlMeta.HtmlTitle;
            }
            if (!string.IsNullOrEmpty(page.ContentTitle))
            {
                body.AppendFormat(title);

                title = page.ContentTitle;
            }

            if (page.PagePositions != null)
            {
                foreach (var item in page.PagePositions.Where(it => (it is HtmlBlockPosition) || (it is HtmlPosition)))
                {
                    if (item is HtmlBlockPosition)
                    {
                        HtmlBlockPosition htmlBlockPosition = (HtmlBlockPosition)item;
                        var htmlBlock = new HtmlBlock(page.Site, htmlBlockPosition.BlockName).LastVersion().AsActual();
                        if (htmlBlock != null)
                        {
                            body.Append(" " + Kooboo.Extensions.StringExtensions.StripAllTags(htmlBlock.Body));
                        }
                    }
                    else
                    {
                        HtmlPosition htmlPosition = (HtmlPosition)item;
                        body.Append(" " + Kooboo.Extensions.StringExtensions.StripAllTags(htmlPosition.Html));
                    }
                }
            }


            indexObject = new IndexObject()
            {
                Title = title,
                Body = body.ToString(),
                StoreFields = storeFields,
                SystemFields = sysFields,
                NativeType = typeof(Page).AssemblyQualifiedNameWithoutVersion()
            };

            return indexObject;
        }

        public object GetNativeObject(System.Collections.Specialized.NameValueCollection fields)
        {
            var siteName = fields["SiteName"];
            var pageName = fields["PageName"];
            return new Page(Page_Context.Current.PageRequestContext.Site, pageName);
        }

        public string GetUrl(object nativeObject)
        {
            Page page = (Page)nativeObject;
            return Page_Context.Current.FrontUrl.PageUrl(page.FullName).ToString();
        }
    }
    #endregion

    public class PageManager : PathResourceManagerBase<Page, IPageProvider>
    {
        static PageManager()
        {
            Converters.Register(typeof(Page), new PageConverter());
        }
        #region Providers
        public IVersionLogger<Models.Page> VersiongLogger
        {
            get
            {
                return VersionManager.ResolveVersionLogger<Models.Page>();
            }
        }
        public IPageProvider PageProvider
        {
            get
            {
                return (IPageProvider)this.Provider;
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

        public override IEnumerable<Page> All(Site site, string filterName)
        {
            var pages = Provider.All(site);
            if (!string.IsNullOrEmpty(filterName))
            {
                pages = pages.Select(it => it.AsActual())
                   .Where(it => it.Name.Contains(filterName, StringComparison.CurrentCultureIgnoreCase)
                       || (it.Navigation != null && !string.IsNullOrEmpty(it.Navigation.DisplayText) && it.Navigation.DisplayText.Contains(filterName, StringComparison.CurrentCultureIgnoreCase)));
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
            var pages = ((IPageProvider)this.Provider).ChildPages(parentPage.LastVersion());

            if (!string.IsNullOrEmpty(filterName))
            {
                pages = pages.Select(it => it.AsActual())
                   .Where(it => it.Name.Contains(filterName, StringComparison.CurrentCultureIgnoreCase)
                       || (it.Navigation != null && !string.IsNullOrEmpty(it.Navigation.DisplayText) && it.Navigation.DisplayText.Contains(filterName, StringComparison.CurrentCultureIgnoreCase)));
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
            var page = this.PageProvider.Copy(site, sourcePageFullName, destPageFullName);

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
        public virtual void Localize(string fullName, Site currentSite)
        {
            var targetPage = new Page(currentSite, fullName);
            var sourcePage = targetPage.LastVersion();

            PageProvider.Localize(sourcePage, currentSite);

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
        public virtual void Export(IEnumerable<Page> pages, System.IO.Stream outputStream)
        {
            Provider.Export(pages, outputStream);
        }
        public virtual void ExportAll(Site site, System.IO.Stream outputStream)
        {
            Provider.Export(All(site, ""), outputStream);
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

        public virtual void Move(Site site, string pageFullName, string newParent, bool createRedirect)
        {
            Page page = new Page(site, pageFullName);
            if (string.IsNullOrEmpty(newParent))
            {
                if (page.Parent == null)
                {
                    throw new KoobooException("The page is a root page already.".Localize());
                }
            }

            PageProvider.Move(site, pageFullName, newParent);
            Page sourcePage = new Page(site, pageFullName);
            Page newPage = null;
            if (!string.IsNullOrEmpty(newParent))
            {
                newPage = new Page(new Page(site, newParent), sourcePage.Name);
            }
            else
            {
                newPage = new Page(site, sourcePage.Name);
            }

            if (createRedirect)
            {
                UrlRedirect redirect = new UrlRedirect()
                {
                    InputUrl = sourcePage.FriendlyName,
                    OutputUrl = newPage.FriendlyName,
                    RedirectType = RedirectType.Moved_Permanently_301,
                    Regex = false
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
                var page = new Page(site, PageHelper.CombineFullName(new[] { parentPage, orderedPageNames[i] }));
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
            var draft = ServiceFactory.PageManager.PageProvider.GetDraft(page);
            return draft != null;
        }
        public virtual void Publish(Page page, bool publishDraft, string userName)
        {
            Publish(page, false, publishDraft, false, DateTime.Now, DateTime.Now, userName);
        }
        public virtual void Publish(Page page, bool publishSchedule, bool publishDraft, bool period, DateTime publishDate, DateTime offlineDate, string userName)
        {
            if (!publishSchedule)
            {
                page = page.AsActual();
                if (publishDraft)
                {
                    page = PageProvider.GetDraft(page);
                    PageProvider.RemoveDraft(page);
                }
                page.Published = true;
                page.UserName = userName;
                PageProvider.Update(page, page);
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
            PageProvider.Update(page, page);
            VersionManager.LogVersion(page);
        }
        #endregion

        #region Front-End
        public virtual Page GetDefaultPage(Site site)
        {
            var pages = Providers.PageProvider.All(site);
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
            foreach (var page in PageProvider.All(site))
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
            if (lastPage.VirtualPath.EqualsOrNullEmpty(identifier, StringComparison.OrdinalIgnoreCase))
            {
                return lastPage;
            }
            foreach (var item in PageProvider.ChildPages(page))
            {
                var found = GetPageByUrlIdentifier(site, item, identifier);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }
        #endregion

    }
}
