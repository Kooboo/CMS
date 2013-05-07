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
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Content.Persistence.Default
{
    public abstract class FileSystemProviderBase<T>
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

        #region Get
        public virtual T Get(T dummy)
        {
            var path = PathFactory.GetPath(dummy);
            if (File.Exists(path.SettingFile))
            {
                GetLocker().EnterReadLock();
                try
                {
                    var item = (T)Serialization.Deserialize(dummy.GetType(), KnownTypes, path.SettingFile);
                    item.Init(dummy);
                    return item;
                }
                finally
                {
                    GetLocker().ExitReadLock();
                }
            }
            return default(T);
        } 
        #endregion

        #region Add
        public virtual void Add(T item)
        {
            Save(item);
        } 
        #endregion

        #region Save
        protected virtual void Save(T item)
        {
            var path = PathFactory.GetPath(item);
            item.OnSaving();
            IO.IOUtility.EnsureDirectoryExists(Path.GetDirectoryName(path.SettingFile));
            GetLocker().EnterWriteLock();
            try
            {
                Serialization.Serialize<T>(item, path.SettingFile);
            }
            finally
            {
                GetLocker().ExitWriteLock();
            }
            item.OnSaved();
        } 
        #endregion

        #region Update
        public virtual void Update(T @new, T old)
        {
            Save(@new);
        } 
        #endregion

        #region Remove
        public virtual void Remove(T item)
        {
            var path = PathFactory.GetPath(item);
            GetLocker().EnterWriteLock();
            try
            {
                int i = 0;
            DELETE:
                try
                {
                    if (File.Exists(path.SettingFile))
                    {
                        File.Delete(path.SettingFile);
                    }
                    if (Directory.Exists(path.PhysicalPath))
                    {
                        Directory.Delete(path.PhysicalPath, true);
                    }
                }
                catch (Exception e)
                {
                    if (i < 3)
                    {
                        i++;
                        goto DELETE;
                    }
                    throw e;
                }

            }
            finally
            {
                GetLocker().ExitWriteLock();
            }
        }
        
        #endregion

        #region GetLocker
        protected abstract System.Threading.ReaderWriterLockSlim GetLocker(); 
        #endregion

        #region All
        public virtual IEnumerable<T> All()
        {
            throw new NotSupportedException("The method does not supported in Kooboo.CMS.Content.");
        } 
        #endregion
    }
}
