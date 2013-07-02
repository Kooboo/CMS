#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Member.Models;
using Kooboo.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Member.Persistence.Default
{
    public abstract class ListProviderBase<T>
        where T : IMemberElement, IPersistable, IIdentifiable
    {
        #region abstract
        protected abstract string GetDataFile(Membership membership);

        protected abstract System.Threading.ReaderWriterLockSlim GetLocker();
        #endregion

        #region All
        public virtual IEnumerable<T> All(Membership membership)
        {
            var filePath = GetDataFile(membership);
            if (!File.Exists(filePath))
            {
                return (new T[0]);
            }
            GetLocker().EnterReadLock();
            try
            {
                var list = DataContractSerializationHelper.Deserialize<List<T>>(filePath) ?? new List<T>();
                foreach (var item in list)
                {
                    item.Membership = membership;
                }
                return list;
            }
            finally
            {
                GetLocker().ExitReadLock();
            }
        }

        public virtual IEnumerable<T> All()
        {
            throw new NotSupportedException("The method does not supported.");
        }

        #endregion

        #region Get
        public virtual T Get(T dummy)
        {
            var all = this.All(dummy.Membership);
            var item = all.Where(it => it.Equals(dummy)).FirstOrDefault();
            if (item != null)
            {
                item.Init(dummy);
            }
            return item;
        } 
        #endregion

        #region Add/Update/Remove
        public virtual void Add(T item)
        {
            var list = this.All(item.Membership).ToList();

            list.Add(item);

            item.OnSaving();
            this.Save(item.Membership, list);
            item.OnSaved();
        }

        public virtual void Update(T item, T oldItem)
        {
            List<T> list = this.All(item.Membership).ToList();
            var index = list.IndexOf(oldItem);
            if (index != -1)
            {
                list.RemoveAt(index);
                list.Insert(index, item);
            }
            item.OnSaving();
            this.Save(item.Membership, list);
            item.OnSaved();
        }

        public virtual void Remove(T item)
        {
            var list = this.All(item.Membership).ToList();
            var index = list.IndexOf(item);
            if (index != -1)
            {
                list.RemoveAt(index);
            }

            this.Save(item.Membership, list);
        }

        protected virtual void Save(Membership membership, List<T> list)
        {
            var file = GetDataFile(membership);
            GetLocker().EnterWriteLock();
            try
            {
                DataContractSerializationHelper.Serialize<List<T>>(list, file);
            }
            finally
            {
                GetLocker().ExitWriteLock();
            }

        } 
        #endregion
    }
}
