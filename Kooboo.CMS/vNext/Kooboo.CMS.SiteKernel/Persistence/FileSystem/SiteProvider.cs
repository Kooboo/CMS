#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.CMS.SiteKernel.Persistence.FileSystem.Storage;
using Kooboo.Common.Data.IsolatedStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Persistence.FileSystem
{
    [Kooboo.Common.ObjectContainer.Dependency.DependencyAttribute(typeof(ISiteProvider))]
    [Kooboo.Common.ObjectContainer.Dependency.DependencyAttribute(typeof(IProvider<Site>))]
    public class SiteProvider : FileProviderBase<Site>, ISiteProvider
    {
        #region .ctor
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        IBaseDir baseDir;
        public SiteProvider(IBaseDir baseDir)
        {
            this.baseDir = baseDir;
        }
        #endregion
        #region
        protected override IFileStorage<Site> GetFileStorage(Site site)
        {
            IIsolatedStorage isolatedStorage;
            if (site != null)
            {
                var basePath = site.DiskStoragePath(baseDir);

                isolatedStorage = new DiskIsolateStorage(site.Name, basePath);
            }
            else
            {
                isolatedStorage = new DiskIsolateStorage("RootSites", baseDir.Cms_DataPhysicalPath);
            }

            var directoryStorage = new DirectoryObjectFileStorage<Site>(isolatedStorage, "", locker);
            return directoryStorage;
        }
        #endregion

        public virtual IEnumerable<Site> RootSites()
        {
            return All();
        }

        public IEnumerable<Site> ChildSites(Site parentSite)
        {
            var fileStorage = GetFileStorage(parentSite);

            var list = fileStorage.GetList().ToArray();


            foreach (var item in list)
            {
                ((ISiteObject)item).Site = parentSite;
            }

            return list;
        }

        public virtual IEnumerable<Site> All()
        {
            return All(null);
        }

        public void Import(Models.Site data, Stream zipData, IDictionary<string, object> options)
        {
            throw new NotImplementedException();
        }

        public Stream Export(IEnumerable<Models.Site> data, IDictionary<string, object> options)
        {
            throw new NotImplementedException();
        }

    }
}
