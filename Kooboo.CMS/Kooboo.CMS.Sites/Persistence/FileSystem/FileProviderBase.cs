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
using Kooboo.CMS.Sites.Persistence.FileSystem.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public abstract class FileProviderBase<T>
         where T : IPersistable, ISiteObject, IIdentifiable, new()
    {
        //#region KnownTypes
        //protected virtual IEnumerable<Type> KnownTypes
        //{
        //    get
        //    {
        //        return new Type[] { };
        //    }
        //}
        //#endregion

        //#region GetLocker
        //protected abstract System.Threading.ReaderWriterLockSlim GetLocker();
        //#endregion
        #region Abstract methods
        protected abstract IFileStorage<T> GetFileStorage(Site site);

        //protected abstract T CreateObject(Site site, FileInfo fileInfo);
        #endregion

        #region All
        public virtual IEnumerable<T> All(Site site)
        {
            var fileStorage = GetFileStorage(site);

            var list = fileStorage.GetList();

            if (typeof(ISiteObject).IsAssignableFrom(typeof(T)))
            {
                foreach (var item in list)
                {
                    ((ISiteObject)item).Site = site;
                }
            }

            return list;
        }

        public virtual IEnumerable<T> All()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Get
        public virtual T Get(T dummy)
        {
            var fileStorage = GetFileStorage(dummy.Site);

            return fileStorage.Get(dummy);
        }
        #endregion

        #region Add
        public virtual void Add(T item)
        {
            var fileStorage = GetFileStorage(item.Site);

            fileStorage.Add(item);
        }
        #endregion

        #region Update
        public virtual void Update(T @new, T old)
        {
            var fileStorage = GetFileStorage(@new.Site);

            fileStorage.Update(@new, old);
        }
        #endregion

        #region Remove
        public virtual void Remove(T item)
        {
            var fileStorage = GetFileStorage(item.Site);

            fileStorage.Remove(item);
        }
        #endregion

        #region Import/Export
        public virtual void Export(Site site, IEnumerable<T> sources, Stream outputStream)
        {
            var fileStorage = GetFileStorage(site);
            fileStorage.Export(sources, outputStream);
        }

        public virtual void Import(Site site, Stream zipStream, bool @override)
        {
            var fileStorage = GetFileStorage(site);
            fileStorage.Import(zipStream, @override);
        }
        #endregion

        #region ISiteElementProvider InitializeToDB/ExportToDisk
        public virtual void InitializeToDB(Site site)
        {
            //not need to implement.
        }

        public virtual void ExportToDisk(Site site)
        {
            //not need to implement.
        }
        #endregion
    }
}