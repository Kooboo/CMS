using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using System.IO;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public abstract class ListFileRepository<T>
        where T : IPersistable
    {
        protected abstract string GetFile(Site site);
        protected abstract System.Threading.ReaderWriterLockSlim GetLocker();

        public virtual IQueryable<T> All(Models.Site site)
        {
            var filePath = GetFile(site);
            if (!File.Exists(filePath))
            {
                return (new T[0]).AsQueryable();
            }
            GetLocker().EnterReadLock();
            try
            {
                var list = Serialization.DeserializeSettings<List<T>>(filePath) ?? new List<T>();
                foreach (var item in list)
                {
                    item.Site = site;
                }
                return list.AsQueryable();
            }
            finally
            {
                GetLocker().ExitReadLock();
            }
        }

        public virtual T Get(T dummy)
        {
            var all = this.All(dummy.Site);
            var item = all.Where(it => it.Equals(dummy)).FirstOrDefault();
            if (item != null)
            {
                item.Init(dummy);
            }
            return item;
        }

        public virtual void Add(T item)
        {
            var list = this.All(item.Site).ToList();

            list.Add(item);

            item.OnSaving();
            this.Save(item.Site, list);
            item.OnSaved();
        }

        public virtual void Update(T item, T oldItem)
        {
            List<T> list = this.All(item.Site).ToList();
            var index = list.IndexOf(oldItem);
            if (index != -1)
            {
                list.RemoveAt(index);
                list.Insert(index, item);
            }
            item.OnSaving();
            this.Save(item.Site, list);
            item.OnSaved();
        }

        public virtual void Remove(T item)
        {
            var list = this.All(item.Site).ToList();
            var index = list.IndexOf(item);
            if (index != -1)
            {
                list.RemoveAt(index);
            }

            this.Save(item.Site, list);
        }



        protected virtual void Save(Site site, List<T> list)
        {
            var file = GetFile(site);
            GetLocker().EnterWriteLock();
            try
            {
                Serialization.Serialize<List<T>>(list, file);
            }
            finally
            {
                GetLocker().ExitWriteLock();
            }

        }


    }
}
