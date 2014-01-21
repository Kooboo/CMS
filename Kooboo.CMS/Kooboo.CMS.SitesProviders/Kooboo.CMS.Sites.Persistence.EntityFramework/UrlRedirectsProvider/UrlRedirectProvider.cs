using Kooboo.CMS.Caching;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence.FileSystem;
using Kooboo.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.UrlRedirectsProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IUrlRedirectProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<UrlRedirect>), Order = 100)]
    public class UrlRedirectProvider : IUrlRedirectProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);

        #region .ctor
        SiteDBContext _dbContext;
        public UrlRedirectProvider(SiteDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        #endregion

        #region --- Import && Export ---
        public void Export(Site site, System.IO.Stream outputStream)
        {
            var provider = new Kooboo.CMS.Sites.Persistence.FileSystem.UrlRedirectProvider();
            InitializeFromDatabase(site);
            provider.Export(site, outputStream);
        }

        /// <summary>
        /// Save records in Database to Disk
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public void InitializeFromDatabase(Site site)
        {
            var allItem = this.All(site).ToList();
            var provider = new Kooboo.CMS.Sites.Persistence.FileSystem.UrlRedirectProvider();
            var file = new UrlRedirectsFile(site).PhysicalPath;
            locker.EnterWriteLock();
            try
            {
                Serialization.Serialize<List<UrlRedirect>>(allItem, file);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            var provider = new Kooboo.CMS.Sites.Persistence.FileSystem.UrlRedirectProvider();
            provider.Import(site, zipStream, @override);
            SaveToDatabase(site, @override);
        }

        public void SaveToDatabase(Site site, bool @override)
        {
            var provider = new FileSystem.UrlRedirectProvider();
            var allItem = provider.All(site);
            if (!@override)
            {
                allItem = allItem.Where(it => null == Get(it));
            }
            var dummy = allItem.ToList();
            foreach (var item in dummy)
            {
                UpdateOrAdd(item, item);
            }
        }
        #endregion

        #region --- CRUD ---
        public void Add(UrlRedirect item)
        {
            UpdateOrAdd(item, item);
        }

        public IEnumerable<UrlRedirect> All()
        {
            return GetAllUrlRedirects();
        }

        public IEnumerable<UrlRedirect> All(Site site)
        {
            var db = GetAllUrlRedirects();
            if (null != db && db.Any())
            {
                return db.Where(it => it.Site.Equals(site));
            }
            return Enumerable.Empty<UrlRedirect>();
        }

        public UrlRedirect Get(UrlRedirect dummy)
        {
            var obj = _dbContext.UrlRedirects.FirstOrDefault(it =>
               it.UUID.Equals(dummy.UUID, StringComparison.OrdinalIgnoreCase));
            if (null != obj)
            {
                return obj.ToUrlRedirect();
            }
            return null;
        }

        public void Remove(UrlRedirect item)
        {
            var obj = _dbContext.UrlRedirects.FirstOrDefault(it =>
                it.UUID.Equals(item.UUID, StringComparison.OrdinalIgnoreCase));
            if (null != obj)
            {
                _dbContext.UrlRedirects.Remove(obj);
                _dbContext.SaveChanges();
            }
        }

        public void Update(UrlRedirect item, UrlRedirect oldItem)
        {
            UpdateOrAdd(item, oldItem);
        }

        private List<UrlRedirect> GetAllUrlRedirects()
        {
            return _dbContext.UrlRedirects.ToArray().Select(it => it.ToUrlRedirect()).ToList();
        }

        private void UpdateOrAdd(UrlRedirect @new, UrlRedirect old)
        {
            ((IPersistable)@new).OnSaving();
            var dummy = _dbContext.UrlRedirects.FirstOrDefault(it => it.UUID.Equals(old.UUID, StringComparison.OrdinalIgnoreCase)
                && it.SiteName.Equals(old.Site.FullName, StringComparison.OrdinalIgnoreCase));

            if (null != dummy)
            {
                @new.ToUrlRedirectEntity(dummy);
            }
            else
            {
                dummy = @new.ToUrlRedirectEntity<UrlRedirectEntity>();
                _dbContext.UrlRedirects.Add(dummy);
            }
            _dbContext.SaveChanges();
            ((IPersistable)@new).OnSaved();
        }
        #endregion
    }


}
