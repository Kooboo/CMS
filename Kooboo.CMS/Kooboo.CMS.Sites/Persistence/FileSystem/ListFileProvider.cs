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
using Kooboo.CMS.Sites.Models;
using System.IO;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Persistence.FileSystem.Storage;
using Ionic.Zip;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public abstract class ListFileRepository<T>
        where T : ISiteObject, IPersistable, IIdentifiable, new()
    {
        #region abstract method
        protected abstract string GetFile(Site site);
        protected abstract System.Threading.ReaderWriterLockSlim GetLocker();
        #endregion

        #region All
        public virtual IEnumerable<T> All(Site site)
        {
            var filePath = GetFile(site);
            var xmlListFileStorage = new XmlListFileStorage<T>(filePath, GetLocker());

            var list = xmlListFileStorage.GetList();

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
            throw new NotSupportedException("The method does not supported in Kooboo.CMS.Sites.");
        }
        #endregion

        #region Get
        public virtual T Get(T dummy)
        {
            var fileStorage = GetFileStorage(dummy.Site);

            return fileStorage.Get(dummy);
        }

        #endregion

        protected virtual IFileStorage<T> GetFileStorage(Site site)
        {
            var filePath = GetFile(site);

            var xmlListFileStorage = new XmlListFileStorage<T>(filePath, GetLocker());
            return xmlListFileStorage;
        }

        #region Add
        public virtual void Add(T item)
        {
            var fileStorage = GetFileStorage(item.Site);

            fileStorage.Add(item);
        }
        #endregion

        #region Update
        public virtual void Update(T item, T oldItem)
        {
            var fileStorage = GetFileStorage(item.Site);

            fileStorage.Update(item, oldItem);
        }
        #endregion

        #region Remove
        public virtual void Remove(T item)
        {
            var fileStorage = GetFileStorage(item.Site);

            fileStorage.Remove(item);
        }
        #endregion

        #region Export
        public virtual void Export(Site site, IEnumerable<T> items, System.IO.Stream outputStream)
        {
            var fileStorage = GetFileStorage(site);
            fileStorage.Export(items, outputStream);
        }
        #endregion

        #region Import
        public virtual void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            var fileStorage = GetFileStorage(site);
            fileStorage.Import(zipStream, @override);
        }
        #endregion

    }
}
