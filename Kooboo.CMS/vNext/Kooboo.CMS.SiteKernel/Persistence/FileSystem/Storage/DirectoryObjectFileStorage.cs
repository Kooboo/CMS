#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Ionic.Zip;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.Common.Data.IsolatedStorage;
using Kooboo.Common.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.CMS.SiteKernel.Persistence.FileSystem.Storage
{
    public class DirectoryObjectFileStorage<T> : IFileStorage<T>
         where T : IIdentifiable, IPersistable, new()
    {
        #region .ctor
        public string DataFileName = "setting.config";
        protected Kooboo.Common.Data.IsolatedStorage.IIsolatedStorage isolatedStorage;
        protected string pathInStorage;
        protected ReaderWriterLockSlim _lock;
        protected IEnumerable<Type> _knownTypes;
        protected Func<string, T> _initialize = (path) =>
         {
             var o = new T() { UUID = Path.GetFileName(path) };
             return o;
         };
        public DirectoryObjectFileStorage(IIsolatedStorage isolatedStorage, string pathInStorage, ReaderWriterLockSlim @lock)
            : this(isolatedStorage, pathInStorage, @lock, new Type[0])
        {
        }
        public DirectoryObjectFileStorage(IIsolatedStorage isolatedStorage, string pathInStorage, ReaderWriterLockSlim @lock, IEnumerable<Type> knownTypes)
            : this(isolatedStorage, pathInStorage, @lock, knownTypes, null)
        {
        }
        public DirectoryObjectFileStorage(IIsolatedStorage isolatedStorage, string pathInStorage, ReaderWriterLockSlim @lock, IEnumerable<Type> knownTypes, Func<string, T> initialize)
        {
            this.isolatedStorage = isolatedStorage;
            this.pathInStorage = pathInStorage;
            this._lock = @lock;
            this._knownTypes = knownTypes;
            if (initialize != null)
            {
                _initialize = initialize;
            }
        }

        #endregion

        #region GetList
        public IEnumerable<T> GetList(string parentItemName = null)
        {
            _lock.EnterReadLock();
            try
            {
                return Enumerate(parentItemName);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        private IEnumerable<T> Enumerate(string parentItemName)
        {
            List<T> list = new List<T>();

            foreach (var item in isolatedStorage.GetDirectoryNames(pathInStorage))
            {
                var itemFullName = item;
                if (!string.IsNullOrEmpty(parentItemName))
                {
                    itemFullName = Path.Combine(FullNameHelper.ToPathName(parentItemName), item);
                }
                if (IsValidDataItem(Path.Combine(pathInStorage, itemFullName)))
                {
                    var o = _initialize(itemFullName);
                    if (o != null)
                    {
                        list.Add(o);
                    }
                }
            }
            return list;
        }
        protected virtual bool IsValidDataItem(string dirName)
        {
            var valid = !dirName.EqualsOrNullEmpty("~versions", StringComparison.OrdinalIgnoreCase);
            if (valid)
            {
                valid = isolatedStorage.FileExists(Path.Combine(dirName, DataFileName));
            }
            return valid;
        }
        #endregion

        #region Get
        public T Get(T dummy)
        {
            string filePath = GetDataFilePath(dummy);
            if (!this.isolatedStorage.FileExists(filePath))
            {
                return default(T);
            }
            _lock.EnterReadLock();
            try
            {
                var o = Deserialize(dummy, filePath);
                if (o != null)
                {
                    o.Init(dummy);
                }
                return o;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
        protected virtual string GetItemPath(T o)
        {
            return Path.Combine(pathInStorage, FullNameHelper.ToPathName(o.UUID));
        }
        protected virtual string GetDataFilePath(T o)
        {
            return Path.Combine(GetItemPath(o), DataFileName);
        }

        #endregion

        #region Serialization
        private T Deserialize(T dummy, string filePath)
        {
            using (var storageFileStream = isolatedStorage.OpenFile(filePath, FileMode.Open))
            {
                using (var newStream = storageFileStream.Stream.NewNamespace())
                {
                    var o = (T)XmlSerialization.Deserialize(dummy.GetType(), _knownTypes, newStream);
                    return o;
                }
            }
        }
        private void Serialize(T item, string filePath)
        {
            using (var storageFileStream = isolatedStorage.OpenFile(filePath, FileMode.OpenOrCreate))
            {
                XmlSerialization.Serialize(item, _knownTypes, storageFileStream.Stream);
                isolatedStorage.SaveFile(storageFileStream);
            }
        }
        #endregion

        #region Add
        public void Add(T item, bool @override = true)
        {
            Save(item);
        }
        #endregion

        private void Save(T item)
        {
            string filePath = GetDataFilePath(item);
            item.OnSaving();
            _lock.EnterWriteLock();
            try
            {
                //save settings       
                Serialize(item, filePath);
                item.OnSaved();
            }
            finally
            {
                _lock.ExitWriteLock();
            }

        }

        #region Update
        public void Update(T item, T oldItem)
        {
            Save(item);
        }
        #endregion

        #region Remove
        public void Remove(T item)
        {
            string dirPath = GetItemPath(item);
            _lock.EnterWriteLock();
            try
            {
                isolatedStorage.DeleteDirectory(dirPath);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        #endregion


        #region Export
        public void Export(IEnumerable<T> items, Stream outputStream)
        {
            //export
            //if (items == null || items.Count() == 0)
            //{
            //    using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
            //    {
            //        zipFile.AddDirectory(_baseFolder, "");

            //        zipFile.Save(outputStream);
            //    }
            //}
            //else
            //{
            //    using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
            //    {
            //        foreach (var item in items)
            //        {
            //            var dir = GetItemPath(item);
            //            zipFile.AddDirectory(dir, Path.GetFileName(dir));
            //        }

            //        zipFile.Save(outputStream);
            //    }
            //}

        }
        #endregion

        #region Import
        public void Import(Stream zipStream, bool @override)
        {
            //export
            //using (ZipFile zipFile = ZipFile.Read(zipStream))
            //{
            //    ExtractExistingFileAction action = ExtractExistingFileAction.DoNotOverwrite;
            //    if (@override)
            //    {
            //        action = ExtractExistingFileAction.OverwriteSilently;
            //    }
            //    zipFile.ExtractAll(_baseFolder, action);
            //}
        }
        #endregion
    }
}
