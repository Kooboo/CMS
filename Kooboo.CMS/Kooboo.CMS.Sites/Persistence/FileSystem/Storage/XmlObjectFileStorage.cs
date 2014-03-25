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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.CMS.Sites.Persistence.FileSystem.Storage
{
    public class XmlObjectFileStorage<T> : IFileStorage<T>
         where T : IIdentifiable, IPersistable, new()
    {
        #region .ctor
        static string DataFileExtension = ".config";
        string _dataFolder;
        ReaderWriterLockSlim _locker;
        IEnumerable<Type> _knownTypes;
        public XmlObjectFileStorage(string dataFolder, ReaderWriterLockSlim locker)
            : this(dataFolder, locker, new Type[0])
        {
        }
        public XmlObjectFileStorage(string dataFolder, ReaderWriterLockSlim locker, IEnumerable<Type> knownTypes)
        {
            this._dataFolder = dataFolder;
            this._locker = locker;
            this._knownTypes = knownTypes;
        }

        #endregion

        #region GetList
        public IEnumerable<T> GetList()
        {
            _locker.EnterReadLock();
            try
            {
                return Enumerate();
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        private IEnumerable<T> Enumerate()
        {
            List<T> list = new List<T>();
            if (Directory.Exists(_dataFolder))
            {
                var dir = new DirectoryInfo(_dataFolder);
                foreach (var fileInfo in dir.EnumerateFiles())
                {
                    if (IsValidDataItem(fileInfo.FullName))
                    {
                        var o = new T() { UUID = Path.GetFileNameWithoutExtension(fileInfo.FullName) };
                        if (o != null)
                        {
                            list.Add(o);
                        }
                    }
                }
            }
            return list;
        }
        protected virtual bool IsValidDataItem(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            return extension.EqualsOrNullEmpty(DataFileExtension, StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        #region Get
        public T Get(T dummy)
        {
            string filePath = GetDataFilePath(dummy);
            if (!File.Exists(filePath))
            {
                return default(T);
            }
            _locker.EnterReadLock();
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
                _locker.ExitReadLock();
            }
        }
        protected virtual string GetDataFilePath(T o)
        {
            return Path.Combine(_dataFolder, o.UUID + DataFileExtension);
        }

        #endregion

        #region Serialization
        private T Deserialize(T dummy, string filePath)
        {
            var o = (T)Serialization.Deserialize(dummy.GetType(), _knownTypes, filePath);
            return o;
        }
        private void Serialize(T item, string filePath)
        {
            Serialization.Serialize(item, _knownTypes, filePath);
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
            _locker.EnterWriteLock();
            try
            {
                //save settings       
                Serialize(item, filePath);
                item.OnSaved();
            }
            finally
            {
                _locker.ExitWriteLock();
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
            string filePath = GetDataFilePath(item);
            _locker.EnterWriteLock();
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }
        #endregion


        #region Export
        public void Export(IEnumerable<T> items, Stream outputStream)
        {
            if (items == null || items.Count() == 0)
            {
                using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
                {
                    zipFile.AddDirectory(_dataFolder, "");

                    zipFile.Save(outputStream);
                }
            }
            else
            {
                using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
                {
                    foreach (var item in items)
                    {
                        zipFile.AddFile(GetDataFilePath(item), "");
                    }

                    zipFile.Save(outputStream);
                }
            }

        }
        #endregion

        #region Import
        public void Import(Stream zipStream, bool @override)
        {
            using (ZipFile zipFile = ZipFile.Read(zipStream))
            {
                ExtractExistingFileAction action = ExtractExistingFileAction.DoNotOverwrite;
                if (@override)
                {
                    action = ExtractExistingFileAction.OverwriteSilently;
                }
                zipFile.ExtractAll(_dataFolder, action);
            }
        }
        #endregion
    }
}
