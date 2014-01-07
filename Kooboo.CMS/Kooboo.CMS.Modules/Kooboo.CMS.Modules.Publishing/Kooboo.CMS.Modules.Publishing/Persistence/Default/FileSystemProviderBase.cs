using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Modules.Publishing.Models.Paths;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Persistence.Default
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
            var pathInstance = PathFactory.GetPath(dummy);
            if (File.Exists(pathInstance.DataFile))
            {
                GetLocker().EnterReadLock();
                try
                {
                    var item = (T)Serialization.Deserialize(dummy.GetType(), KnownTypes, pathInstance.DataFile);
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
            var pathInstance = PathFactory.GetPath(item);
            item.OnSaving();
            IO.IOUtility.EnsureDirectoryExists(Path.GetDirectoryName(pathInstance.PhysicalPath));
            GetLocker().EnterWriteLock();
            try
            {
                Serialization.Serialize<T>(item,pathInstance.DataFile);
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
            var pathInstance = PathFactory.GetPath(item);
            GetLocker().EnterWriteLock();
            try
            {
                int i = 0;
            DELETE:
                try
                {
                    if (File.Exists(pathInstance.DataFile))
                    {
                        File.Delete(pathInstance.DataFile);
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

        public virtual void Remove<T>(string uuid)
        {
            var pathInstance = PathFactory.GetPath<T>();
            var file = Path.Combine(pathInstance.PhysicalPath, uuid + ".config");
            if (File.Exists(file))
            {
                int i = 0;
            DELETE:
                try
                {
                    GetLocker().EnterWriteLock();
                    File.Delete(file);
                }
                catch(Exception ex)
                {
                    if (i < 3)
                    {
                        i++;
                        goto DELETE;
                    }
                    throw ex;
                }
                finally{
                    GetLocker().ExitWriteLock();
                }
            }
        }

        #endregion

        #region GetLocker
        protected abstract System.Threading.ReaderWriterLockSlim GetLocker();
        #endregion

        #region All
        public virtual IEnumerable<T> All()
        {
            var lst = new List<T>();
            var pathInstance = PathFactory.GetPath<T>();
            if (Directory.Exists(pathInstance.PhysicalPath))
            {
                var files=Directory.GetFiles(pathInstance.PhysicalPath, "*.config", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    if (File.Exists(file))
                    {
                        GetLocker().EnterReadLock();
                        try
                        {
                            var item = (T)Serialization.Deserialize(typeof(T), KnownTypes, file);
                            lst.Add(item);
                        }
                        finally
                        {
                            GetLocker().ExitReadLock();
                        }
                    }
                }
            }
            return lst;
            //throw new NotSupportedException("The method does not supported in Kooboo.CMS.Modules.Publishing.");
        }
        #endregion
    }
}
