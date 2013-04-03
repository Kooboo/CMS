using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;

namespace Kooboo.CMS.Content.Persistence.Default
{
    public abstract class FileSystemProviderBase<T>
        where T : IPersistable
    {
        protected virtual IEnumerable<Type> KnownTypes
        {
            get
            {
                return new Type[] { };
            }
        }
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

        public virtual void Add(T item)
        {
            Save(item);
        }
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

        public virtual void Update(T @new, T old)
        {
            if (@new is IRepositoryElement)
            {
                var newPath = PathFactory.GetPath<T>(@new);
                var oldPath = PathFactory.GetPath<T>(old);
                if (!@new.Equals(old) && oldPath.Exists())
                {
                    oldPath.Rename(((IRepositoryElement)@new).Name);
                }
            }

            Save(@new);
        }

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
        
        protected abstract System.Threading.ReaderWriterLockSlim GetLocker();
    }
}
