using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Sites.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Persistence.Default
{
    public abstract class FileSystemProviderBase<T>
        where T : ISiteObject, IPersistable, IIdentifiable
    {
        #region .ctor
        Kooboo.CMS.Sites.Persistence.ISiteProvider _siteProvider;
        public FileSystemProviderBase(Kooboo.CMS.Sites.Persistence.ISiteProvider siteProvider)
        {
            _siteProvider = siteProvider;
        }
        #endregion

        #region KnownTypes
        protected virtual IEnumerable<Type> KnownTypes
        {
            get
            {
                return new Type[] { };
            }
        }
        #endregion

        #region Abstract methods
        protected abstract string GetBasePath(Site site);

        protected virtual string GetItemDataPath(T item)
        {
            var basePath = GetBasePath(item.Site);

            return Path.Combine(basePath, item.UUID + ".config");
        }
        #endregion

        #region Get
        public virtual T Get(T dummy)
        {
            var dataFilePath = GetItemDataPath(dummy);
            if (File.Exists(dataFilePath))
            {
                GetLocker().EnterReadLock();
                try
                {
                    var item = (T)Serialization.Deserialize(dummy.GetType(), KnownTypes, dataFilePath);
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
            var dataFile = GetItemDataPath(item);
            item.OnSaving();
            IO.IOUtility.EnsureDirectoryExists(Path.GetDirectoryName(dataFile));
            GetLocker().EnterWriteLock();
            try
            {
                Serialization.Serialize<T>(item, dataFile);
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
            var dataFile = GetItemDataPath(item);
            GetLocker().EnterWriteLock();
            try
            {
                int i = 0;
            DELETE:
                try
                {
                    if (File.Exists(dataFile))
                    {
                        File.Delete(dataFile);
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

        //public virtual void Remove<T>(string uuid)
        //{
        //    var pathInstance = PathFactory.GetPath<T>();
        //    var file = Path.Combine(pathInstance.PhysicalPath, uuid + ".config");
        //    if (File.Exists(file))
        //    {
        //        int i = 0;
        //    DELETE:
        //        try
        //        {
        //            GetLocker().EnterWriteLock();
        //            File.Delete(file);
        //        }
        //        catch (Exception ex)
        //        {
        //            if (i < 3)
        //            {
        //                i++;
        //                goto DELETE;
        //            }
        //            throw ex;
        //        }
        //        finally
        //        {
        //            GetLocker().ExitWriteLock();
        //        }
        //    }
        //}

        #endregion

        #region GetLocker
        protected abstract System.Threading.ReaderWriterLockSlim GetLocker();
        #endregion

        #region All
        public virtual IEnumerable<T> All()
        {
            var lst = new List<T>();
            foreach (var site in _siteProvider.AllSites())
            {
                lst.AddRange(All(site));
            }
            return lst;
        }

        public virtual IEnumerable<T> All(Site site)
        {
            var lst = new List<T>();
            var basePath = GetBasePath(site);
            if (Directory.Exists(basePath))
            {
                GetLocker().EnterReadLock();
                try
                {
                    var files = Directory.GetFiles(basePath, "*.config", SearchOption.TopDirectoryOnly);
                    foreach (var file in files)
                    {
                        if (File.Exists(file))
                        {
                            var item = (T)Serialization.Deserialize(typeof(T), KnownTypes, file);
                            item.Site = site;
                            item.Init(item);
                            lst.Add(item);
                        }
                    }
                }
                finally
                {
                    GetLocker().ExitReadLock();
                }
            }
            return lst;
        }
        #endregion
    }
}
