#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Models;
using Ionic.Zip;
using System.Diagnostics;
using System.Web;
using Kooboo.CMS.Caching;
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Sites.Providers.SqlServer.PageProvider
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IPageProvider), Order = 100)]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IProvider<Page>), Order = 100)]
    public class PageProvider : IPageProvider
    {
        //#region Version
        //public class PageVersionLogger : Kooboo.CMS.Sites.Versioning.IVersionLogger<Page>
        //{
        //    public void LogVersion(Page o)
        //    {
        //        //todo:
        //    }

        //    public IEnumerable<Versioning.VersionInfo> AllVersions(Page o)
        //    {
        //        return new Versioning.VersionInfo[0];
        //        //todo:
        //    }

        //    public Page GetVersion(Page o, int version)
        //    {
        //        return null;
        //        //todo:
        //    }

        //    public void Revert(Page o, int version)
        //    {
        //        //todo:
        //    }
        //}
        //#endregion

        #region .ctor
        static PageProvider()
        {
            ClearCache();
        }
        #endregion

        #region 缓存Page表

        static string cacheKey = "SqlServer:PageProvider:PagesTable";
        /// <summary>
        /// 为什么要缓存Page表呢？
        /// 1. 因为在实际使用过程中发现，如果页面数量相对比较多的时候，第一次查找页面的过程中会调用很多次ChildPages，导致请求的效率很低，时间花费很高。
        /// 2. 目前的设计已经是假设页面数量不多的前提了。所以缓该表的数据量是可以接受的。
        /// </summary>
        private static List<PageEntity> GetCachedPageList()
        {
            var cacheObject = CacheManagerFactory.DefaultCacheManager.GlobalObjectCache();
            return cacheObject.GetCache<List<PageEntity>>(cacheKey, () =>
             {
                 return SiteDbContext.CreateDbContext().Pages.ToArray().Select(it => { it.PageObject = PageEntityHelper.ToPage(it); return it; })
                        .ToList();
             });
        }
        private static void ClearCache()
        {
            CacheManagerFactory.DefaultCacheManager.GlobalObjectCache().Remove(cacheKey);
        }
        #endregion

        #region Query
        public IEnumerable<Page> All()
        {
            throw new NotSupportedException();
        }
        public IEnumerable<Page> All(Models.Site site)
        {
            List<Page> results = new List<Page>();

            while (site != null)
            {
                var tempResults = QueryBySite(site);
                if (results.Count == 0)
                {
                    results.AddRange(tempResults);
                }
                else
                {
                    foreach (var item in tempResults)
                    {
                        if (!results.Any(it => it.Name.Equals(item.Name, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            results.Add(item);
                        }
                    }
                }
                site = site.Parent;
            }
            return results;
        }
        private IEnumerable<Page> QueryBySite(Site site)
        {
            return GetCachedPageList()
                    .Where(it => it.SiteName.Equals(site.FullName, StringComparison.InvariantCultureIgnoreCase) && string.IsNullOrEmpty(it.ParentPage))
                   .ToArray()
                   .Select(it => it.PageObject);
        }

        public IEnumerable<Models.Page> ChildPages(Models.Page parentPage)
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            var siteName = parentPage.Site.FullName;
            var pageName = parentPage.FullName;
            var data = GetCachedPageList()
                .Where(it => it.SiteName.Equals(siteName, StringComparison.InvariantCultureIgnoreCase)
                    && it.ParentPage.Equals(pageName, StringComparison.InvariantCultureIgnoreCase))
                .ToArray()
                .Select(it => it.PageObject)
                .AsQueryable();

            //stopwatch.Stop();
            //HttpContext.Current.Response.Write(string.Format("ChildPages, {0}ms.</br>", stopwatch.ElapsedMilliseconds));


            return data;


        }
        #endregion

        #region relation
        private IEnumerable<Page> AllPagesNested(Site site)
        {
            return GetCachedPageList()
                .Where(it => it.SiteName.Equals(site.FullName, StringComparison.InvariantCultureIgnoreCase))
                .ToArray()
                .Select(it => it.PageObject);
        }
        public IEnumerable<Models.Page> ByLayout(Models.Layout layout)
        {
            return AllPagesNested(layout.Site)
                 .Select(it => it.AsActual())
                 .Where(it => it.Layout.EqualsOrNullEmpty(layout.Name, StringComparison.CurrentCultureIgnoreCase))
                 .ToArray();
        }

        public IEnumerable<Models.Page> ByView(Models.View view)
        {
            return AllPagesNested(view.Site).Select(it => it.AsActual())
                .Where(it => it.PagePositions != null &&
                    it.PagePositions.Any(p => p is ViewPosition && ((ViewPosition)p).ViewName.EqualsOrNullEmpty(view.Name, StringComparison.CurrentCultureIgnoreCase)))
                    .ToArray();
        }

        public IEnumerable<Models.Page> ByModule(Models.Site site, string moduleName)
        {
            return AllPagesNested(site).Select(it => it.AsActual())
                 .Where(it => it.PagePositions != null &&
                     it.PagePositions.Any(p => p is ModulePosition && ((ModulePosition)p).ModuleName.EqualsOrNullEmpty(moduleName, StringComparison.CurrentCultureIgnoreCase)))
                     .ToArray();
        }

        public IEnumerable<Models.Page> ByHtmlBlock(HtmlBlock htmlBlock)
        {
            return AllPagesNested(htmlBlock.Site).Select(it => it.AsActual())
                .Where(it => it.PagePositions != null &&
                    it.PagePositions.Any(p => p is HtmlBlockPosition && ((HtmlBlockPosition)p).BlockName.EqualsOrNullEmpty(htmlBlock.Name, StringComparison.CurrentCultureIgnoreCase)))
                    .ToArray();
        }
        #endregion

        #region Draft
        public Models.Page GetDraft(Models.Page page)
        {
            var entity = SiteDbContext.CreateDbContext().PageDrafts
               .Where(it => it.SiteName == page.Site.FullName && it.FullName == page.FullName)
               .FirstOrDefault();
            return PageEntityHelper.ToPage(entity);
        }

        public void SaveAsDraft(Models.Page page)
        {
            var dbContext = SiteDbContext.CreateDbContext();

            var entity = dbContext.PageDrafts
                .Where(it => it.SiteName == page.Site.FullName && it.FullName == page.FullName)
                .FirstOrDefault();

            if (entity != null)
            {
                PageEntityHelper.ToPageEntity(page, entity);
            }
            else
            {
                dbContext.PageDrafts.Add(PageEntityHelper.ToPageEntity<PageDraftEntity>(page));
            }

            dbContext.SaveChanges();
        }

        public void RemoveDraft(Models.Page page)
        {
            var dbContext = SiteDbContext.CreateDbContext();
            var entity = dbContext.PageDrafts
                 .Where(it => it.SiteName == page.Site.FullName && it.FullName == page.FullName)
                 .FirstOrDefault();
            if (entity != null)
            {
                dbContext.PageDrafts.Remove(entity);
                dbContext.SaveChanges();
            }
        }
        #endregion

        #region Manage
        public Models.Page Get(Models.Page dummy)
        {
            //因为FullName是动态拼接，所以会有一点的性能问题
            var siteName = dummy.Site.FullName;
            var pageName = dummy.FullName;
            var entity = GetCachedPageList()
                .Where(it => it.SiteName.Equals(siteName, StringComparison.InvariantCultureIgnoreCase)
                    && it.FullName.Equals(pageName, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();
            return entity == null ? null : entity.PageObject;
        }

        public void Add(Models.Page item)
        {
            InsertOrUpdate(item, item);
        }

        public void Update(Models.Page @new, Models.Page old)
        {
            InsertOrUpdate(@new, old);
        }

        private void InsertOrUpdate(Models.Page @new, Models.Page old)
        {
            @new.OnSaving();

            var dbContext = SiteDbContext.CreateDbContext();

            var entity = dbContext.Pages
                 .Where(it => it.SiteName == old.Site.FullName && it.FullName == old.FullName)
                .FirstOrDefault();

            if (entity != null)
            {
                PageEntityHelper.ToPageEntity(@new, entity);
            }
            else
            {
                dbContext.Pages.Add(PageEntityHelper.ToPageEntity<PageEntity>(@new));
            }
            dbContext.SaveChanges();
            ClearCache();
        }

        #region Remove
        public void Remove(Models.Page item)
        {
            var dbContext = SiteDbContext.CreateDbContext();
            var entity = dbContext.Pages
                   .Where(it => it.SiteName == item.Site.FullName && it.FullName == item.FullName)
                   .FirstOrDefault();
            if (entity != null)
            {
                RemovePageWithChildPages(dbContext, entity);

                dbContext.SaveChanges();

                ClearCache();
            }
        }
        private void RemovePageWithChildPages(SiteDbContext dbContext, PageEntity entity)
        {
            dbContext.Pages.Remove(entity);

            var children = dbContext.Pages
              .Where(it => it.SiteName == entity.SiteName && it.ParentPage == entity.FullName)
              .ToArray();

            foreach (var item in children)
            {
                RemovePageWithChildPages(dbContext, item);
            }
        }
        #endregion

        #region Localize
        public void Localize(Models.Page o, Models.Site targetSite)
        {
            var dbContext = SiteDbContext.CreateDbContext();
            LocalizeWithChildPages(dbContext, o, targetSite);
            dbContext.SaveChanges();

            ClearCache();
        }
        private void LocalizeWithChildPages(SiteDbContext dbContext, Page page, Site targetSite)
        {
            var entity = dbContext.Pages
                .Where(it => it.SiteName == targetSite.FullName && it.FullName == page.FullName)
                .FirstOrDefault();
            if (entity == null)
            {
                page = Get(page);

                entity = PageEntityHelper.ToPageEntity<PageEntity>(page);
                entity.SiteName = targetSite.FullName;

                dbContext.Pages.Add(entity);

                foreach (var item in ChildPages(page))
                {
                    LocalizeWithChildPages(dbContext, item, targetSite);
                }
            }

        }
        #endregion

        #region copy
        public Models.Page Copy(Models.Site site, string sourcePageFullName, string newPageFullName)
        {
            var page = Get(new Page(site, sourcePageFullName));
            var newPage = new Page(site, newPageFullName);
            var entity = PageEntityHelper.ToPageEntity<PageEntity>(page);
            entity.FullName = newPageFullName;
            if (newPage.Parent != null)
            {
                entity.ParentPage = newPage.Parent.FullName;
            }

            var dbContext = SiteDbContext.CreateDbContext();
            dbContext.Pages.Add(entity);
            dbContext.SaveChanges();

            ClearCache();

            return PageEntityHelper.ToPage(entity);
        }
        #endregion

        #region Move
        public void Move(Models.Site site, string pageFullName, string newParent)
        {
            var dbContext = SiteDbContext.CreateDbContext();

            MovePageRecursively(site, pageFullName, newParent, dbContext);

            dbContext.SaveChanges();

            ClearCache();
        }

        private void MovePageRecursively(Models.Site site, string pageFullName, string newParent, SiteDbContext dbContext)
        {
            var oldPage = Get(new Page(site, pageFullName));
            var entity = PageEntityHelper.ToPageEntity<PageEntity>(oldPage);
            if (!string.IsNullOrEmpty(newParent))
            {
                var newPage = new Page(new Page(site, newParent), oldPage.Name);
                entity.FullName = newPage.FullName;
                entity.ParentPage = newPage.Parent.FullName;
            }
            else
            {
                entity.FullName = oldPage.Name;
                entity.ParentPage = "";
            }

            dbContext.Pages.Add(entity);

            foreach (var item in ChildPages(oldPage))
            {
                MovePageRecursively(site, item.FullName, entity.FullName, dbContext);
            }

            var oldEntity = dbContext.Pages
                   .Where(it => it.SiteName == oldPage.Site.FullName && it.FullName == oldPage.FullName)
                   .FirstOrDefault();
            if (oldEntity != null)
            {
                dbContext.Pages.Remove(oldEntity);
            }
        }
        #endregion
        #endregion

        #region Export
        public void Export(Site site, IEnumerable<Models.Page> sources, System.IO.Stream outputStream)
        {
            IPageProvider filePageProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.PageProvider();
            foreach (var item in sources)
            {
                ExportAsFileCascading(filePageProvider, item);
            }
            filePageProvider.Export(site, sources, outputStream);
        }
        private void ExportAsFileCascading(IPageProvider filePageProvider, Page page)
        {
            page = Get(page);
            filePageProvider.Add(page);
            foreach (var item in ChildPages(page))
            {
                ExportAsFileCascading(filePageProvider, item);
            }
        }

        public void Import(Models.Site site, Page parent, System.IO.Stream zipStream, bool @override)
        {
            List<string> pages = new List<string>();
            using (ZipFile zipFile = ZipFile.Read(zipStream))
            {
                foreach (ZipEntry item in zipFile)
                {
                    if (item.IsDirectory)
                    {
                        pages.Add(item.FileName);
                    }
                    else
                    {
                    }
                }
            }
            zipStream.Position = 0;

            IPageProvider filePageProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.PageProvider();

            filePageProvider.Import(site, parent, zipStream, @override);


            foreach (var item in pages)
            {
                var page = filePageProvider.Get(GetPage(site, parent, item));
                this.Add(page);
            }
        }
        private Page GetPage(Site site, Page parent, string pageName)
        {
            string fullName = pageName;
            if (parent != null)
            {
                fullName = PageHelper.CombineFullName(new[] { parent.FullName, pageName });
            }

            return new Page(site, fullName);
        }


        #endregion

        #region Clear
        public void Clear(Site site)
        {
            var dbContext = SiteDbContext.CreateDbContext();
            var entities = dbContext.Pages
                   .Where(it => it.SiteName == site.FullName);
            foreach (var item in entities)
            {
                dbContext.Pages.Remove(item);
            }

            var drafts = dbContext.PageDrafts
                   .Where(it => it.SiteName == site.FullName);
            foreach (var item in drafts)
            {
                dbContext.PageDrafts.Remove(item);
            }
            dbContext.SaveChanges();

            ClearCache();
        }

        #endregion

        #region ISiteElementProvider InitializeToDB/ExportToDisk
        private void InitializePageCascading(IPageProvider filePageProvider, Page page)
        {
            this.Add(filePageProvider.Get(page));
            foreach (var item in filePageProvider.ChildPages(page))
            {
                InitializePageCascading(filePageProvider, item);
            }
        }
        public void InitializeToDB(Models.Site site)
        {
            IPageProvider filePageProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.PageProvider();
            foreach (var item in filePageProvider.All(site))
            {
                if (item.Site == site)
                {
                    InitializePageCascading(filePageProvider, item);
                }
            }
        }

        public void ExportToDisk(Models.Site site)
        {
            IPageProvider filePageProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.PageProvider();

            //remove the GetCachedPageList() folder to clear all old GetCachedPageList().
            var dummy = new Page(site, "Dummy");
            Kooboo.Common.IO.IOUtility.DeleteDirectory(dummy.BasePhysicalPath, true);

            foreach (var item in QueryBySite(site))
            {
                ExportPageCascading(filePageProvider, item);
            }
        }
        private void ExportPageCascading(IPageProvider filePageProvider, Page page)
        {
            filePageProvider.Add(page);
            foreach (var item in this.ChildPages(page))
            {
                ExportPageCascading(filePageProvider, item);
            }
        }
        #endregion
    }
}
