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
using System.IO;
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Account.Persistence.FileSystem
{
    public abstract class ObjectFileRepository<T>
        where T : IPersistable
    {
        protected abstract System.Threading.ReaderWriterLockSlim GetLocker();
        protected abstract string GetFilePath(T o);
        protected abstract string GetBasePath();
        protected abstract T CreateObject(string filePath);
        public virtual IEnumerable<T> All()
        {
            GetLocker().EnterReadLock();
            try
            {
                return AllEnumerable().Select(o => Get(o));
            }
            finally
            {
                GetLocker().ExitReadLock();
            }

        }
        private IEnumerable<T> AllEnumerable()
        {
            if (Directory.Exists(GetBasePath()))
            {
                foreach (var filePath in Directory.EnumerateFiles(GetBasePath(), "*.config"))
                {
                    //string fileName = Path.GetFileNameWithoutExtension(filePath);
                    yield return CreateObject(filePath);
                }
            }
        }
        public virtual T Get(T dummy)
        {
            string filePath = GetFilePath(dummy);
            if (File.Exists(filePath))
            {
                GetLocker().EnterWriteLock();
                try
                {
                    var item = Kooboo.Runtime.Serialization.DataContractSerializationHelper.Deserialize<T>(filePath);
                    item.Init(dummy);
                    return item;
                }
                finally
                {
                    GetLocker().ExitWriteLock();
                }
            }
            return default(T);
        }

        public virtual void Add(T item)
        {
            string filePath = GetFilePath(item);
            if (File.Exists(filePath))
            {
                throw new KoobooException("The item is already exists.");
            }
            Save(item, filePath);
        }

        protected void Save(T item, string filePath)
        {
            GetLocker().EnterWriteLock();
            try
            {
                item.OnSaving();
                Kooboo.Runtime.Serialization.DataContractSerializationHelper.Serialize<T>(item, filePath);
                item.OnSaved();
            }
            finally
            {
                GetLocker().ExitWriteLock();
            }

        }

        public virtual void Update(T @new, T old)
        {
            string filePath = GetFilePath(@old);
            if (!File.Exists(filePath))
            {
                throw new KoobooException("The item is not exists.");
            }
            Save(@new, filePath);
        }

        public virtual void Remove(T item)
        {
            string filePath = GetFilePath(item);
            if (File.Exists(filePath))
            {
                GetLocker().EnterWriteLock();
                try
                {
                    File.Delete(filePath);
                }
                finally
                {
                    GetLocker().ExitWriteLock();
                }

            }
        }
    }
}
