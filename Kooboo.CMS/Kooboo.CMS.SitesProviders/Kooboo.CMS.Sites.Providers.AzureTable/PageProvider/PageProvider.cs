#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Ionic.Zip;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
namespace Kooboo.CMS.Sites.Providers.AzureTable.PageProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IPageProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Page>), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISiteExportableProvider), Order = 100)]
    public class PageProvider : IPageProvider
    {
        //#region version
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

        #region Static
        static string PageTable = "Pages";
        static string PageDraftTable = "PageDrafts";
        private class DummyEntity : TableServiceEntity { }
        static PageProvider()
        {
            CloudTableClient tableClient = CloudTableHelper.GetTableClient();

            tableClient.CreateTableIfNotExist<PageEntity>(PageTable);

            tableClient.CreateTableIfNotExist<PageEntity>(PageDraftTable);

        }

        #endregion

        #region Query
        public IEnumerable<Page> All()
        {
            throw new NotImplementedException();
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
            return results.AsQueryable();
        }
        private IEnumerable<Page> QueryBySite(Site site)
        {
            return CloudTableHelper.GetTableServiceContext().CreateQuery<PageEntity>(PageTable)
                   .Where(it => it.PartitionKey == site.FullName && it.ParentPage == "")
                   .ToArray()
                   .Select(it => PageEntityHelper.ToPage(it));
        }

        public IEnumerable<Models.Page> ChildPages(Models.Page parentPage)
        {
            return CloudTableHelper.GetTableServiceContext().CreateQuery<PageEntity>(PageTable)
                .Where(it => it.PartitionKey == parentPage.Site.FullName && it.ParentPage == parentPage.FullName)
                .ToArray()
                .Select(it => PageEntityHelper.ToPage(it))
                .AsQueryable();

        }
        #endregion

        #region Front-end
        public Models.Page GetDefaultPage(Models.Site site)
        {
            var entity = CloudTableHelper.GetTableServiceContext().CreateQuery<PageEntity>(PageTable)
                .Where(it => it.PartitionKey == site.FullName)
                .Where(it => it.IsDefault == true)
                .FirstOrDefault();
            return PageEntityHelper.ToPage(entity);
        }
        #endregion

        #region relation
        private IEnumerable<Page> AllPagesNested(Site site)
        {
            return CloudTableHelper.GetTableServiceContext().CreateQuery<PageEntity>(PageTable)
                .Where(it => it.SiteName == site.FullName)
                .ToArray()
                .Select(it => PageEntityHelper.ToPage(it))
                .AsQueryable();
        }
        public IEnumerable<Models.Page> ByLayout(Models.Layout layout)
        {
            return AllPagesNested(layout.Site)
                 .Select(it => it.AsActual())
                 .Where(it => it.Layout.EqualsOrNullEmpty(layout.Name, StringComparison.CurrentCultureIgnoreCase))
                 .ToArray()
                 .AsQueryable();
        }

        public IEnumerable<Models.Page> ByView(Models.View view)
        {
            return AllPagesNested(view.Site).Select(it => it.AsActual())
                .Where(it => it.PagePositions != null &&
                    it.PagePositions.Any(p => p is ViewPosition && ((ViewPosition)p).ViewName.EqualsOrNullEmpty(view.Name, StringComparison.CurrentCultureIgnoreCase)))
                    .ToArray()
                    .AsQueryable();
        }

        public IEnumerable<Models.Page> ByModule(Models.Site site, string moduleName)
        {
            return AllPagesNested(site).Select(it => it.AsActual())
                 .Where(it => it.PagePositions != null &&
                     it.PagePositions.Any(p => p is ModulePosition && ((ModulePosition)p).ModuleName.EqualsOrNullEmpty(moduleName, StringComparison.CurrentCultureIgnoreCase)))
                     .ToArray()
                     .AsQueryable();
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
            var entity = CloudTableHelper.GetTableServiceContext().CreateQuery<PageEntity>(PageDraftTable)
               .Where(it => it.PartitionKey == page.Site.FullName && it.RowKey == page.FullName)
               .FirstOrDefault();
            return PageEntityHelper.ToPage(entity);
        }

        public void SaveAsDraft(Models.Page page)
        {
            var entity = PageEntityHelper.ToPageEntity(page);
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            if (GetDraft(page) != null)
            {
                serviceContext.AttachTo(PageDraftTable, entity);
                serviceContext.UpdateObject(entity);
            }
            else
            {
                serviceContext.AddObject(PageDraftTable, entity);
            }

            serviceContext.SaveChangesWithRetries();
        }

        public void RemoveDraft(Models.Page page)
        {
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            var entity = serviceContext.CreateQuery<PageEntity>(PageDraftTable)
                 .Where(it => it.PartitionKey == page.Site.FullName && it.RowKey == page.FullName)
                 .Select(it => PageEntityHelper.ToPage(it)).FirstOrDefault();
            if (entity != null)
            {
                serviceContext.DeleteObject(entity);
                serviceContext.SaveChangesWithRetries();
            }
        }
        #endregion

        #region Manage
        public Models.Page Get(Models.Page dummy)
        {
            var entity = CloudTableHelper.GetTableServiceContext().CreateQuery<PageEntity>(PageTable)
                .Where(it => it.PartitionKey == dummy.Site.FullName && it.RowKey == dummy.FullName)
                .FirstOrDefault();
            return PageEntityHelper.ToPage(entity);
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

            var entity = PageEntityHelper.ToPageEntity(@new);
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            if (Get(old) == null)
            {
                serviceContext.AddObject(PageTable, entity);
            }
            else
            {
                serviceContext.AttachTo(PageTable, entity, "*");
                serviceContext.UpdateObject(entity);
            }
            serviceContext.SaveChangesWithRetries();
        }

        public void Remove(Models.Page item)
        {
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            var entity = serviceContext.CreateQuery<PageEntity>(PageTable)
                   .Where(it => it.PartitionKey == item.Site.FullName && it.RowKey == item.FullName)
                   .FirstOrDefault();
            if (entity != null)
            {
                RemovePageWithChildPages(serviceContext, entity);

                serviceContext.SaveChangesWithRetries();
            }
        }
        private void RemovePageWithChildPages(TableServiceContext serviceContext, PageEntity entity)
        {
            serviceContext.DeleteObject(entity);

            var children = serviceContext.CreateQuery<PageEntity>(PageTable)
              .Where(it => it.PartitionKey == entity.SiteName && it.ParentPage == entity.FullName)
              .ToArray();

            foreach (var item in children)
            {
                RemovePageWithChildPages(serviceContext, item);
            }
        }

        public void Localize(Models.Page o, Models.Site targetSite)
        {
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            LocalizeWithChildPages(serviceContext, o, targetSite);
            serviceContext.SaveChangesWithRetries();

        }
        private void LocalizeWithChildPages(TableServiceContext serviceContext, Page page, Site targetSite)
        {
            var entity = CloudTableHelper.GetTableServiceContext().CreateQuery<PageEntity>(PageTable)
               .Where(it => it.PartitionKey == targetSite.FullName && it.RowKey == page.FullName)
               .FirstOrDefault();
            if (entity == null)
            {
                page = Get(page);
                entity = PageEntityHelper.ToPageEntity(page);
                entity.SiteName = targetSite.FullName;
                serviceContext.AddObject(PageTable, entity);

                foreach (var item in ChildPages(page))
                {
                    LocalizeWithChildPages(serviceContext, item, targetSite);
                }
            }

        }
        public Models.Page Copy(Models.Site site, string sourcePageFullName, string newPageFullName)
        {
            var page = Get(new Page(site, sourcePageFullName));
            var newPage = new Page(site, newPageFullName);
            var entity = PageEntityHelper.ToPageEntity(page);
            entity.FullName = newPageFullName;
            if (newPage.Parent != null)
            {
                entity.ParentPage = newPage.Parent.FullName;
            }

            var serviceContext = CloudTableHelper.GetTableServiceContext();
            serviceContext.AddObject(PageTable, entity);
            serviceContext.SaveChangesWithRetries();

            return PageEntityHelper.ToPage(entity);
        }

        public void Move(Models.Site site, string pageFullName, string newParent)
        {
            var serviceContext = CloudTableHelper.GetTableServiceContext();

            MovePageRecursively(site, pageFullName, newParent, serviceContext);

            serviceContext.SaveChangesWithRetries();
        }

        private void MovePageRecursively(Models.Site site, string pageFullName, string newParent, TableServiceContext serviceContext)
        {
            var oldPage = Get(new Page(site, pageFullName));
            var entity = PageEntityHelper.ToPageEntity(oldPage);
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

            foreach (var item in ChildPages(oldPage))
            {
                MovePageRecursively(site, item.FullName, entity.FullName, serviceContext);
            }

            serviceContext.AddObject(PageTable, entity);
            var oldEntity = PageEntityHelper.ToPageEntity(oldPage);
            serviceContext.AttachTo(PageTable, oldEntity, "*");
            serviceContext.DeleteObject(oldEntity);
        }
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
       
        private void InitializePageCascading(IPageProvider filePageProvider, Page page)
        {
            this.Add(filePageProvider.Get(page));
            foreach (var item in filePageProvider.ChildPages(page))
            {
                InitializePageCascading(filePageProvider, item);
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
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            var entities = serviceContext.CreateQuery<PageEntity>(PageTable)
                   .Where(it => it.PartitionKey == site.FullName);
            foreach (var item in entities)
            {
                serviceContext.DeleteObject(item);
            }

            var drafts = serviceContext.CreateQuery<PageEntity>(PageDraftTable)
                   .Where(it => it.PartitionKey == site.FullName);
            foreach (var item in drafts)
            {
                serviceContext.DeleteObject(item);
            }

            serviceContext.SaveChangesWithRetries();

        }
        #endregion

        #region ISiteElementProvider InitializeToDB/ExportToDisk
        public void InitializeToDB(Site site)
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

        public void ExportToDisk(Site site)
        {
            IPageProvider filePageProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.PageProvider();

            //remove the pages folder to clear all old pages.
            var dummy = new Page(site, "Dummy");
            Kooboo.IO.IOUtility.DeleteDirectory(dummy.BasePhysicalPath, true);

            foreach (var item in QueryBySite(site))
            {
                ExportPageCascading(filePageProvider, item);
            }          
        }
        #endregion
    }
}
