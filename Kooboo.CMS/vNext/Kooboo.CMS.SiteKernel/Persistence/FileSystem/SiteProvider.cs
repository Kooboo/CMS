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
using System.Threading;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Persistence.FileSystem
{
    [Kooboo.Common.ObjectContainer.Dependency.DependencyAttribute(typeof(ISiteProvider))]
    [Kooboo.Common.ObjectContainer.Dependency.DependencyAttribute(typeof(IProvider<Site>))]
    public class SiteProvider : ISiteProvider
    {
        #region .ctor
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        IBaseDir baseDir;
        public SiteProvider(IBaseDir baseDir)
        {
            this.baseDir = baseDir;
        }
        #endregion

        #region GetFileStorage
        private class SiteFileStorage : DirectoryObjectFileStorage<Site>
        {
            public SiteFileStorage(IIsolatedStorage isolatedStorage, string pathInStorage, ReaderWriterLockSlim @lock)
                : base(isolatedStorage, pathInStorage, @lock)
            {
            }
            public SiteFileStorage(IIsolatedStorage isolatedStorage, string pathInStorage, ReaderWriterLockSlim @lock, IEnumerable<Type> knownTypes)
                : base(isolatedStorage, pathInStorage, @lock, knownTypes)
            {
            }
            public SiteFileStorage(IIsolatedStorage isolatedStorage, string pathInStorage, ReaderWriterLockSlim @lock, IEnumerable<Type> knownTypes, Func<string, Site> initialize)
                : base(isolatedStorage, pathInStorage, @lock, knownTypes, initialize)
            {

            }
            protected override string GetItemPath(Site o)
            {
                return o.Name;
            }
            protected override string GetDataFilePath(Site o)
            {
                return DataFileName;
            }
        }
        protected virtual IFileStorage<Site> GetFileStorage(Site site, string pathInStorage = "")
        {
            IIsolatedStorage isolatedStorage;
            if (site != null)
            {
                isolatedStorage = site.GetIsolatedStorage();
            }
            else
            {
                isolatedStorage = new DiskIsolateStorage("RootSites", Path.Combine(baseDir.Cms_DataPhysicalPath, "Sites"));
            }

            var directoryStorage = new SiteFileStorage(isolatedStorage, pathInStorage, locker, new Type[0], (name) =>
            {
                return new Site(site, name);
            });
            return directoryStorage;
        }
        #endregion

        #region All
        public virtual IEnumerable<Site> RootSites()
        {
            return All();
        }
        public virtual IEnumerable<Site> All()
        {
            var fileStorage = GetFileStorage(null);

            var list = fileStorage.GetList().ToArray();

            return list;
        }
        #endregion

        #region Get
        public Site Get(Site dummy)
        {
            var fileStorage = GetFileStorage(dummy);

            return fileStorage.Get(dummy);
        }
        #endregion

        #region Add
        public void Add(Site item)
        {
            var fileStorage = GetFileStorage(item.Parent, "Sites");

            fileStorage.Add(item);
        }
        #endregion

        #region Update
        public void Update(Site @new, Site old)
        {
            var fileStorage = GetFileStorage(@new);

            fileStorage.Update(@new, old);
        }
        #endregion

        #region Remove
        public void Remove(Site item)
        {
            var fileStorage = GetFileStorage(item);

            fileStorage.Remove(item);
        }

        #endregion

        #region ChildSites
        public IEnumerable<Site> ChildSites(Site parentSite)
        {
            var fileStorage = GetFileStorage(parentSite, "Sites");

            var list = fileStorage.GetList().ToArray();

            foreach (var item in list)
            {
                ((ISiteObject)item).Site = parentSite;
            }

            return list;
        }

        #endregion

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
