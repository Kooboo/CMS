using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public abstract class ObjectFileProvider<T>
        where T : IPersistable
    {
        protected virtual IEnumerable<Type> KnownTypes
        {
            get
            {
                return new Type[] { };
            }
        }
        public abstract IQueryable<T> All(Models.Site site);

        protected abstract System.Threading.ReaderWriterLockSlim GetLocker();

        public virtual T Get(T dummy)
        {
            string filePath = dummy.DataFile;
            if (!File.Exists(filePath))
            {
                return default(T);
            }
            GetLocker().EnterReadLock();
            try
            {
                var o = Deserialize(dummy, filePath);
                o.Init(dummy);               
                return o;
            }
            finally
            {
                GetLocker().ExitReadLock();
            }


        }

        protected virtual T Deserialize(T dummy, string filePath)
        {
            var o = (T)Serialization.Deserialize(dummy.GetType(), KnownTypes, filePath);
            return o;
        }
        protected virtual void Serialize(T item, string filePath)
        {
            Serialization.Serialize(item, KnownTypes, filePath);
        }

        public virtual void Add(T item)
        {
            //check
            Save(item);
        }
        protected virtual void Save(T item)
        {
            string filePath = item.DataFile;
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
            string filePath = old.DataFile;
            Save(@new);
        }

        public virtual void Remove(T item)
        {
            string filePath = item.DataFile;
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
    }
}
