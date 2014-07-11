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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Persistence.FileSystem.Storage
{
    public class JsonListFileStorage<T> : IFileStorage<T>
        where T : IPersistable, new()
    {
        #region .ctor
        string _dataFile;
        ReaderWriterLockSlim _locker;
        public JsonListFileStorage(string dataFile, ReaderWriterLockSlim locker)
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
                var json = Kooboo.Common.IO.IOUtility.ReadAsString(_dataFile);
                var list = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
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
                var json = JsonConvert.SerializeObject(list, Formatting.Indented);
                Kooboo.Common.IO.IOUtility.SaveStringToFile(_dataFile, json);
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
            throw new NotImplementedException();
        }
        #endregion

        #region Import
        public void Import(Stream zipStream, bool @override)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
