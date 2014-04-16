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
using Kooboo.CMS.Common.Persistence.Relational;

namespace Kooboo.CMS.Sites.Providers.SqlServer.HtmlBlockProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IHtmlBlockProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<HtmlBlock>), Order = 100)]
    public class HtmlBlockProvider : IHtmlBlockProvider
    {
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
            return SiteDbContext.CreateDbContext().HtmlBlocks
                   .Where(it => it.SiteName == site.FullName)
                   .ToArray()
                   .Select(it => it.ToHtmlBlock());
        }

        public Models.HtmlBlock Get(Models.HtmlBlock dummy)
        {
            var entity = SiteDbContext.CreateDbContext().HtmlBlocks
                 .Where(it => it.SiteName == dummy.Site.FullName && it.Name == dummy.Name)
                 .FirstOrDefault();
            return entity == null ? null : entity.ToHtmlBlock();
        }

        public void Add(Models.HtmlBlock item)
        {
            InsertOrUpdate(item, item);
        }
        private void InsertOrUpdate(Models.HtmlBlock @new, Models.HtmlBlock old)
        {
            var dbContext = SiteDbContext.CreateDbContext();
            var entity = dbContext.HtmlBlocks
                  .Where(it => it.SiteName == @new.Site.FullName && it.Name == @new.Name)
                  .FirstOrDefault();
            if (entity != null)
            {
                entity.Body = @new.Body;
            }
            else
            {
                entity = new HtmlBlockEntity(@new);
                dbContext.HtmlBlocks.Add(entity);
            }
            dbContext.SaveChanges();
        }
        public void Update(Models.HtmlBlock @new, Models.HtmlBlock old)
        {
            InsertOrUpdate(@new, old);
        }

        public void Remove(Models.HtmlBlock item)
        {
            var dbContext = SiteDbContext.CreateDbContext();
            var entity = dbContext.HtmlBlocks
                   .Where(it => it.SiteName == item.Site.FullName && it.Name == item.Name)
                   .FirstOrDefault();
            if (entity != null)
            {
                dbContext.HtmlBlocks.Remove(entity);
                dbContext.SaveChanges();
            }
        }

        public void Localize(Models.HtmlBlock o, Models.Site targetSite)
        {
            var htmlBlock = Get(o);
            var entity = new HtmlBlockEntity(htmlBlock);
            entity.SiteName = targetSite.FullName;

            var dbContext = SiteDbContext.CreateDbContext();
            dbContext.HtmlBlocks.Add(entity);
            dbContext.SaveChanges();
        }
        #endregion

        #region export
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
            var dbContext = SiteDbContext.CreateDbContext();
            var entities = dbContext.HtmlBlocks
                   .Where(it => it.SiteName == site.FullName);
            foreach (var item in entities)
            {
                dbContext.HtmlBlocks.Remove(item);
            }
            dbContext.SaveChanges();

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
