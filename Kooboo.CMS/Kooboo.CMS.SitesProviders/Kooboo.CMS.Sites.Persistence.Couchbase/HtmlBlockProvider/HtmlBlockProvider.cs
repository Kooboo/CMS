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

using Kooboo.CMS.Sites.Models;
using Ionic.Zip;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Persistence.Couchbase.HtmlBlockProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IHtmlBlockProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<HtmlBlock>), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISiteExportableProvider), Order = 100, Key = "HtmlBlockProvider")]
    public class HtmlBlockProvider : IHtmlBlockProvider
    {
        Func<Site, string, HtmlBlock> createModel = (Site site, string key) =>
        {
            return new HtmlBlock(site, key);
        };
        #region .ctor
        public HtmlBlockProvider()
        {
        }
        #endregion

        //#region version
        //public class HtmlBlockVersionLogger : Kooboo.CMS.Sites.Versioning.IVersionLogger<HtmlBlock>
        //{
        //    public void LogVersion(HtmlBlock o)
        //    {
        //        //todo:
        //    }

        //    public IEnumerable<Versioning.VersionInfo> AllVersions(HtmlBlock o)
        //    {
        //        return new Versioning.VersionInfo[0];
        //        //todo:
        //    }

        //    public HtmlBlock GetVersion(HtmlBlock o, int version)
        //    {
        //        return null;
        //        //todo:
        //    }

        //    public void Revert(HtmlBlock o, int version)
        //    {
        //        //todo:
        //    }
        //}
        //#endregion

        #region general
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
            return DataHelper.QueryList<HtmlBlock>(site, ModelExtensions.GetQueryViewName(ModelExtensions.HtmlBlockDataType), createModel);
        }

        public HtmlBlock Get(Models.HtmlBlock dummy)
        {
            var bucketDocumentKey = ModelExtensions.GetBucketDocumentKey(ModelExtensions.HtmlBlockDataType, dummy.Name);

            return DataHelper.QueryByKey<HtmlBlock>(dummy.Site, bucketDocumentKey, createModel);
        }

        public void Add(Models.HtmlBlock item)
        {
            InsertOrUpdate(item, item);
        }
        private void InsertOrUpdate(HtmlBlock @new, HtmlBlock old)
        {
            ((IPersistable)@new).OnSaving();

            DataHelper.StoreObject(@new, @new.UUID, ModelExtensions.HtmlBlockDataType);

            ((IPersistable)@new).OnSaved();
        }
        public void Update(HtmlBlock @new, HtmlBlock old)
        {
            InsertOrUpdate(@new, old);
        }

        public void Remove(HtmlBlock item)
        {
            DataHelper.DeleteItemByKey(item.Site, ModelExtensions.GetBucketDocumentKey(ModelExtensions.HtmlBlockDataType, item.Name));
        }

        public void Localize(HtmlBlock o, Site targetSite)
        {
            var htmlBlock = Get(o);
            htmlBlock.Site = targetSite;
            InsertOrUpdate(htmlBlock, htmlBlock);
        }
        #endregion

        #region Export
        public void Export(Site site, IEnumerable<Models.HtmlBlock> sources, System.IO.Stream outputStream)
        {
            var fileProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.HtmlBlockProvider();
            foreach (var item in sources)
            {
                var layout = Get(item);
                fileProvider.Add(layout);
            }
            fileProvider.Export(site, sources, outputStream);
        }
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

        #region Clear
        public void Clear(Site site)
        {


        }
        #endregion

        #region All
        public IEnumerable<HtmlBlock> All()
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
