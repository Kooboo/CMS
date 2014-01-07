#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Membership.Models;
using Kooboo.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Membership.Persistence.Default
{
    public abstract class SettingFileProviderBase<T>
        where T : IPersistable, IIdentifiable
    {
        #region KnownTypes
        protected virtual IEnumerable<Type> KnownTypes
        {
            get
            {
                return new Type[] { };
            }
        }
        #endregion

        #region GetLocker
        protected abstract System.Threading.ReaderWriterLockSlim GetLocker();
        #endregion

        #region GetDataFilePath
        protected abstract string GetDataFilePath(T o);
        #endregion

        #region Get
        public virtual T Get(T dummy)
        {
            string filePath = GetDataFilePath(dummy);
            if (!File.Exists(filePath))
            {
                return default(T);
            }
            GetLocker().EnterReadLock();
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
                GetLocker().ExitReadLock();
            }


        }
        #endregion

        #region Serialize/Deserialize
        #region Deserialize
        protected virtual T Deserialize(T dummy, string filePath)
        {
            var o = (T)DataContractSerializationHelper.Deserialize(dummy.GetType(), KnownTypes, filePath);
            return o;
        }
        #endregion

        #region Serialize
        protected virtual void Serialize(T item, string filePath)
        {
            DataContractSerializationHelper.Serialize(item, filePath, KnownTypes);
        }
        #endregion
        #endregion

        #region Add/Update/Save
        public virtual void Add(T item)
        {
            //check
            Save(item);
        }
        protected virtual void Save(T item)
        {
            string filePath = GetDataFilePath(item);
            item.OnSaving();
            GetLocker().EnterWriteLock();
            try
            {
                //save settings       
                Serialize(item, filePath);
                item.OnSaved();
            }
            finally
            {
                GetLocker().ExitWriteLock();
            }

        }
        public virtual void Update(T @new, T old)
        {
            Save(@new);
        }
        #endregion

        #region Remove
        public virtual void Remove(T item)
        {
            string filePath = GetDataFilePath(item);
            GetLocker().EnterWriteLock();
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            finally
            {
                GetLocker().ExitWriteLock();
            }
        }
        #endregion
    }
}
