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
using System.Text;
using Kooboo.CMS.Sites.Persistence;
using Microsoft.WindowsAzure.StorageClient;
using Kooboo.CMS.Sites.Models;
using Ionic.Zip;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Providers.AzureTable.HtmlBlockProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IHtmlBlockProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<HtmlBlock>), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISiteExportableProvider), Order = 100)]
    public class HtmlBlockProvider : IHtmlBlockProvider
    {
        #region HtmlBlockVersionLogger
        public class HtmlBlockVersionLogger : Kooboo.CMS.Sites.Versioning.IVersionLogger<HtmlBlock>
        {
            public void LogVersion(HtmlBlock o)
            {
                //todo:
            }

            public IEnumerable<Versioning.VersionInfo> AllVersions(HtmlBlock o)
            {
                return new Versioning.VersionInfo[0];
                //todo:
            }

            public HtmlBlock GetVersion(HtmlBlock o, int version)
            {
                return null;
                //todo:
            }

            public void Revert(HtmlBlock o, int version)
            {
                //todo:
            }
        }
        #endregion

        #region Static
        static string HtmlBlockTable = "HtmlBlocks";
        static HtmlBlockProvider()
        {
            CloudTableClient tableClient = CloudTableHelper.GetTableClient();

            tableClient.CreateTableIfNotExist<HtmlBlockEntity>(HtmlBlockTable);

        }
        #endregion

        #region All/GET/Add/Update/Remove/Localize
        public IEnumerable<HtmlBlock> All()
        {
            throw new NotSupportedException();
        }
        public IEnumerable<Models.HtmlBlock> All(Models.Site site)
        {
            List<HtmlBlock> results = new List<HtmlBlock>();

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
        private IEnumerable<HtmlBlock> QueryBySite(Site site)
        {
            return CloudTableHelper.GetTableServiceContext().CreateQuery<HtmlBlockEntity>(HtmlBlockTable)
                   .Where(it => it.PartitionKey == site.FullName)
                   .ToArray()
                   .Select(it => it.ToHtmlBlock());
        }

        public Models.HtmlBlock Get(Models.HtmlBlock dummy)
        {
            var entity = CloudTableHelper.GetTableServiceContext().CreateQuery<HtmlBlockEntity>(HtmlBlockTable)
                 .Where(it => it.PartitionKey == dummy.Site.FullName && it.RowKey == dummy.Name)
                 .FirstOrDefault();
            return entity == null ? null : entity.ToHtmlBlock();
        }

        public void Add(Models.HtmlBlock item)
        {
            InsertOrUpdate(item, item);
        }
        private void InsertOrUpdate(Models.HtmlBlock @new, Models.HtmlBlock old)
        {
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            var entity = new HtmlBlockEntity(@new);
            if (Get(old) == null)
            {
                serviceContext.AddObject(HtmlBlockTable, entity);
            }
            else
            {
                serviceContext.AttachTo(HtmlBlockTable, entity, "*");
                serviceContext.UpdateObject(entity);
            }
            serviceContext.SaveChangesWithRetries();
        }
        public void Update(Models.HtmlBlock @new, Models.HtmlBlock old)
        {
            InsertOrUpdate(@new, old);
        }

        public void Remove(Models.HtmlBlock item)
        {
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            var entity = serviceContext.CreateQuery<HtmlBlockEntity>(HtmlBlockTable)
                   .Where(it => it.PartitionKey == item.Site.FullName && it.RowKey == item.Name)
                   .FirstOrDefault();
            if (entity != null)
            {
                serviceContext.DeleteObject(entity);
                serviceContext.SaveChangesWithRetries();
            }
        }

        public void Localize(Models.HtmlBlock o, Models.Site targetSite)
        {
            var htmlBlock = Get(o);
            var entity = new HtmlBlockEntity(htmlBlock);
            entity.SiteName = targetSite.FullName;

            var serviceContext = CloudTableHelper.GetTableServiceContext();
            serviceContext.AddObject(HtmlBlockTable, entity);
            serviceContext.SaveChangesWithRetries();
        }
        #endregion

        #region Export
        public void Export(IEnumerable<Models.HtmlBlock> sources, System.IO.Stream outputStream)
        {
            var fileProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.HtmlBlockProvider();
            foreach (var item in sources)
            {
                var layout = Get(item);
                fileProvider.Add(layout);
            }
            fileProvider.Export(sources, outputStream);
        }

        #endregion

        #region Import
        public void Import(Models.Site site, System.IO.Stream zipStream, bool @override)
        {
            List<string> htmlBlocks = new List<string>();
            using (ZipFile zipFile = ZipFile.Read(zipStream))
            {
                foreach (ZipEntry item in zipFile)
                {
                    if (item.IsDirectory)
                    {
                        htmlBlocks.Add(item.FileName.Trim('/'));
                    }
                    else
                    {
                    }
                }
            }
            zipStream.Position = 0;

            var fileProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.HtmlBlockProvider();

            fileProvider.Import(site, zipStream, @override);


            foreach (var item in htmlBlocks)
            {
                var htmlBlock = fileProvider.Get(new HtmlBlock(site, item));
                this.Add(htmlBlock);
            }
        }
        #endregion

        #region Clear
        public void Clear(Site site)
        {
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            var entities = serviceContext.CreateQuery<HtmlBlockEntity>(HtmlBlockTable)
                   .Where(it => it.PartitionKey == site.FullName);
            foreach (var item in entities)
            {
                serviceContext.DeleteObject(item);
            }
            serviceContext.SaveChangesWithRetries();
        }
        #endregion

        #region ISiteElementProvider InitializeToDB/ExportToDisk
        public void InitializeToDB(Site site)
        {
            IHtmlBlockProvider fileHtmlProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.HtmlBlockProvider();
            foreach (var item in fileHtmlProvider.All(site))
            {
                if (item.Site == site)
                {
                    this.Add(fileHtmlProvider.Get(item));
                }
            }
        }

        public void ExportToDisk(Site site)
        {
            IHtmlBlockProvider fileHtmlProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.HtmlBlockProvider();

            //remove the pages folder to clear all old pages.
            var dummy = new HtmlBlock(site, "Dummy");
            Kooboo.IO.IOUtility.DeleteDirectory(dummy.BasePhysicalPath, true);

            foreach (var item in QueryBySite(site))
            {
                fileHtmlProvider.Add(item);
            }
        }
        #endregion
    }
}
