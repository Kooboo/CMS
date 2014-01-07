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
namespace Kooboo.CMS.Sites.Persistence.Couchbase
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IPageProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Page>), Order = 100)]
    public class PageProvider : IPageProvider
    {
        Func<Site, string, Page> createModel = (Site site, string key) =>
                {
                    return new Page(site, key);
                };
        #region .ctor

        public PageProvider()
        {
        }
        #endregion

        #region Version
        public class PageVersionLogger : Kooboo.CMS.Sites.Versioning.IVersionLogger<Page>
        {
            public void LogVersion(Page o)
            {
                //todo:
            }

            public IEnumerable<Versioning.VersionInfo> AllVersions(Page o)
            {
                return new Versioning.VersionInfo[0];
                //todo:
            }

            public Page GetVersion(Page o, int version)
            {
                return null;
                //todo:
            }

            public void Revert(Page o, int version)
            {
                //todo:
            }
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
            return DataHelper.QueryList<Page>(site, ModelExtensions.GetQueryView(ModelExtensions.PageDataType), createModel)
                .Where(it => it.Parent == null);
        }

        public IEnumerable<Page> ChildPages(Models.Page parentPage)
        {
            return DataHelper.QueryList<Page>(parentPage.Site, ModelExtensions.GetQueryView(ModelExtensions.PageDataType), createModel)
                 .Where(it => it.Parent == parentPage);
        }
        #endregion

        #region relation
        private IEnumerable<Page> AllPagesNested(Site site)
        {
            return DataHelper.QueryList<Page>(site, ModelExtensions.GetQueryView(ModelExtensions.PageDataType), createModel);
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
        public Page GetDraft(Page page)
        {
            var bucketDocumentKey = ModelExtensions.GetBucketDocumentKey(ModelExtensions.PageDraftDataType, page.FullName);

            return DataHelper.QueryByKey<Page>(page.Site, bucketDocumentKey, createModel);
        }

        public void SaveAsDraft(Page page)
        {
            DataHelper.StoreObject(page, page.UUID, ModelExtensions.PageDraftDataType);
        }

        public void RemoveDraft(Page page)
        {
            DataHelper.DeleteItemByKey(page.Site, ModelExtensions.GetBucketDocumentKey(ModelExtensions.PageDraftDataType, page.FullName));
        }
        #endregion

        #region Manage
        public Models.Page Get(Models.Page dummy)
        {
            var bucketDocumentKey = ModelExtensions.GetBucketDocumentKey(ModelExtensions.PageDataType, dummy.FullName);

            return DataHelper.QueryByKey<Page>(dummy.Site, bucketDocumentKey, createModel);
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

            DataHelper.StoreObject(@new, @new.UUID, ModelExtensions.PageDataType);

            @new.OnSaved();
        }

        #region Remove
        public void Remove(Models.Page item)
        {
            RemovePageWithChildPages(item);
        }
        private void RemovePageWithChildPages(Page page)
        {
            DataHelper.DeleteItemByKey(page.Site, ModelExtensions.GetBucketDocumentKey(ModelExtensions.PageDataType, page.FullName));

            var childPages = ChildPages(page);
            foreach (var item in childPages)
            {
                RemovePageWithChildPages(item);
            }
        }
        #endregion

        #region Localize
        public void Localize(Models.Page o, Models.Site targetSite)
        {
            LocalizeWithChildPages(o, targetSite);
        }
        private void LocalizeWithChildPages(Page page, Site targetSite)
        {
            page = Get(page);

            page.Site = targetSite;

            InsertOrUpdate(page, page);

        }
        #endregion

        #region copy
        public Page Copy(Models.Site site, string sourcePageFullName, string newPageFullName)
        {
            var page = Get(new Page(site, sourcePageFullName));
            page.FullName = newPageFullName;
            InsertOrUpdate(page, page);
            return page;
        }
        #endregion

        #region Move
        public void Move(Models.Site site, string pageFullName, string newParent)
        {
            MovePageRecursively(site, pageFullName, newParent);
        }

        private void MovePageRecursively(Models.Site site, string pageFullName, string newParent)
        {
            var oldPage = new Page(site, pageFullName);
            var newPage = Get(oldPage);

            if (!string.IsNullOrEmpty(newParent))
            {
                newPage.FullName = new Page(new Page(site, newParent), oldPage.Name).FullName;
            }
            else
            {
                newPage.FullName = oldPage.Name;
            }

            InsertOrUpdate(newPage, newPage);
            Remove(oldPage);
            foreach (var item in ChildPages(oldPage))
            {
                MovePageRecursively(site, item.FullName, newPage.FullName);
            }

        }
        #endregion
        #endregion

        #region Export
        public void Export(IEnumerable<Models.Page> sources, System.IO.Stream outputStream)
        {
            IPageProvider filePageProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.PageProvider();
            foreach (var item in sources)
            {
                ExportAsFileCascading(filePageProvider, item);
            }
            filePageProvider.Export(sources, outputStream);
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
        public void InitializePages(Site site)
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
        private void InitializePageCascading(IPageProvider filePageProvider, Page page)
        {
            this.Add(filePageProvider.Get(page));
            foreach (var item in filePageProvider.ChildPages(page))
            {
                InitializePageCascading(filePageProvider, item);
            }
        }
        public void ExportPagesToDisk(Site site)
        {
            IPageProvider filePageProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.PageProvider();

            //remove the GetCachedPageList() folder to clear all old GetCachedPageList().
            var dummy = new Page(site, "Dummy");
            Kooboo.IO.IOUtility.DeleteDirectory(dummy.BasePhysicalPath, true);

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

        #region Clear
        public void Clear(Site site)
        {
            //var entities = _dbContext.Pages
            //       .Where(it => it.SiteName == site.FullName);
            //foreach (var item in entities)
            //{
            //    _dbContext.Pages.Remove(item);
            //}

            //var drafts = _dbContext.PageDrafts
            //       .Where(it => it.SiteName == site.FullName);
            //foreach (var item in drafts)
            //{
            //    _dbContext.PageDrafts.Remove(item);
            //}
            //_dbContext.SaveChanges();

            //ClearCache();
        }

        #endregion

    }
}
