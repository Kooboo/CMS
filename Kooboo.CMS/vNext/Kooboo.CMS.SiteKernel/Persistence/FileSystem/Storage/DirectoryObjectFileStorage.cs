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
using Kooboo.Common.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.CMS.Sites.Persistence.FileSystem.Storage
{
    public class DirectoryObjectFileStorage<T> : IFileStorage<T>
         where T : IIdentifiable, IPersistable, new()
    {
        #region .ctor
        public string DataFileName = "setting.config";
        protected string _baseFolder;
        protected ReaderWriterLockSlim _lock;
        protected IEnumerable<Type> _knownTypes;
        protected Func<DirectoryInfo, T> _initialize = (dir) =>
         {
             var o = new T() { UUID = dir.Name };
             return o;
         };
        public DirectoryObjectFileStorage(string baseFolder, ReaderWriterLockSlim @lock)
            : this(baseFolder, @lock, new Type[0])
        {
        }
        public DirectoryObjectFileStorage(string baseFolder, ReaderWriterLockSlim @lock, IEnumerable<Type> knownTypes)
            : this(baseFolder, @lock, knownTypes, null)
        {
        }
        public DirectoryObjectFileStorage(string baseFolder, ReaderWriterLockSlim @lock, IEnumerable<Type> knownTypes, Func<DirectoryInfo, T> initialize)
        {
            this._baseFolder = baseFolder;
            this._lock = @lock;
            this._knownTypes = knownTypes;
            if (initialize != null)
            {
                _initialize = initialize;
            }
        }

        #endregion

        #region GetList
        public IEnumerable<T> GetList()
        {
            _lock.EnterReadLock();
            try
            {
                return Enumerate();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        private IEnumerable<T> Enumerate()
        {
            List<T> list = new List<T>();
            if (Directory.Exists(_baseFolder))
            {
                var dir = new DirectoryInfo(_baseFolder);
                foreach (var dirInfo in IOUtility.EnumerateDirectoriesExludeHidden(_baseFolder))
                {
                    if (IsValidDataItem(dirInfo))
                    {
                        var o = _initialize(dirInfo);
                        if (o != null)
                        {
                            list.Add(o);
                        }
                    }
                }
            }
            return list;
        }
        protected virtual bool IsValidDataItem(DirectoryInfo dirInfo)
        {
            var valid = !dirInfo.Name.EqualsOrNullEmpty("~versions", StringComparison.OrdinalIgnoreCase);
            if (valid)
            {
                var dataFile = Path.Combine(dirInfo.FullName, DataFileName);
                valid = File.Exists(dataFile);
            }
            return valid;
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
            return Path.Combine(_baseFolder, o.UUID);
        }
        protected virtual string GetDataFilePath(T o)
        {
            return Path.Combine(GetItemPath(o), DataFileName);
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
                if (Directory.Exists(dirPath))
                {
                    Kooboo.Common.IO.IOUtility.DeleteDirectory(dirPath, true);
                }
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
            if (items == null || items.Count() == 0)
            {
                using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
                {
                    zipFile.AddDirectory(_baseFolder, "");

                    zipFile.Save(outputStream);
                }
            }
            else
            {
                using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
                {
                    foreach (var item in items)
                    {
                        var dir = GetItemPath(item);
                        zipFile.AddDirectory(dir, Path.GetFileName(dir));
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
                zipFile.ExtractAll(_baseFolder, action);
            }
        }
        #endregion
    }
}
