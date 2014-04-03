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
    public class XmlListFileStorage<T> : IFileStorage<T>
         where T : IPersistable, new()
    {
        #region .ctor
        string _dataFile;
        ReaderWriterLockSlim _locker;

        public XmlListFileStorage(string dataFile, ReaderWriterLockSlim locker)
        {
            this._dataFile = dataFile;
            this._locker = locker;
        }
        #endregion

        #region GetList
        public virtual IEnumerable<T> GetList()
        {
            if (!File.Exists(_dataFile))
            {
                return (new T[0]);
            }
            _locker.EnterReadLock();
            try
            {
                var list = Serialization.DeserializeSettings<List<T>>(_dataFile) ?? new List<T>();
                return list;
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }
        #endregion

        #region Get
        public virtual T Get(T dummy)
        {
            var all = this.GetList();
            return Get(all, dummy);
        }

        private T Get(IEnumerable<T> list, T dummy)
        {
            var item = list.Where(it => it.Equals(dummy)).FirstOrDefault();
            if (item != null)
            {
                item.Init(dummy);
            }
            return item;
        }
        #endregion

        #region Add
        public virtual void Add(T item, bool @override = true)
        {
            var list = this.GetList().ToList();
            var existsItem = Get(list, item);
            if (existsItem != null)
            {
                if (@override)
                {
                    Remove(list, existsItem);
                }
                else
                {
                    //throw exception?
                    return;
                }
            }
            list.Add(item);

            item.OnSaving();
            this.SaveList(list);
            item.OnSaved();
        }
        #endregion

        #region Update
        public virtual void Update(T item, T oldItem)
        {
            List<T> list = this.GetList().ToList();
            var index = list.IndexOf(oldItem);
            if (index != -1)
            {
                list.RemoveAt(index);
                list.Insert(index, item);
            }
            item.OnSaving();
            this.SaveList(list);
            item.OnSaved();
        }
        #endregion

        #region Remove
        public virtual void Remove(T item)
        {
            var list = this.GetList().ToList();
            Remove(list, item);

            this.SaveList(list);
        }

        private static void Remove(List<T> list, T item)
        {
            var index = list.IndexOf(item);
            if (index != -1)
            {
                list.RemoveAt(index);
            }
        }
        #endregion

        #region SaveList
        public virtual void SaveList(List<T> list)
        {
            _locker.EnterWriteLock();
            try
            {
                Serialization.Serialize<List<T>>(list, _dataFile);
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
            if (items != null && items.Count() > 0)
            {
                var tempPath = Path.Combine(Path.GetDirectoryName(_dataFile), "TMEP");
                var exportFile = Path.Combine(tempPath, Path.GetFileName(_dataFile));

                var xmlListFileStorage = new XmlListFileStorage<T>(exportFile, new System.Threading.ReaderWriterLockSlim());

                foreach (var item in items)
                {
                    var data = Get(item);
                    xmlListFileStorage.Add(data, true);
                }

                using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
                {
                    zipFile.AddFile(exportFile, "");
                    zipFile.Save(outputStream);
                }

                try
                {
                    System.IO.File.Delete(exportFile);
                }
                catch (Exception e)
                {
                    Kooboo.HealthMonitoring.Log.LogException(e);
                }
            }
            else
            {
                using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
                {
                    zipFile.AddFile(_dataFile, "");
                    zipFile.Save(outputStream);
                }
            }
        }

        #endregion

        #region Import
        public void Import(Stream zipStream, bool @override)
        {
            var tempPath = Path.Combine(Path.GetDirectoryName(_dataFile), "TMEP");
            var exportFile = Path.Combine(tempPath, Path.GetFileName(_dataFile));
            //ensure the old file was removed.
            if (File.Exists(exportFile))
            {
                File.Delete(exportFile);
            }
            //extract the zip file.
            using (ZipFile zipFile = ZipFile.Read(zipStream))
            {
                zipFile.ExtractAll(tempPath);
            }
            //get the items in the temp file.
            if (File.Exists(exportFile))
            {
                var xmlListFileStorage = new XmlListFileStorage<T>(exportFile, new System.Threading.ReaderWriterLockSlim());
                foreach (var item in xmlListFileStorage.GetList())
                {
                    this.Add(item);
                }
            }

            System.IO.File.Delete(exportFile);
        }
        #endregion
    }
}
