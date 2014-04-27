using Kooboo.CMS.Caching;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.CustomErrorsProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ICustomErrorProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Kooboo.CMS.Sites.Models.CustomError>), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISiteExportableProvider), Order = 100, Key = "CustomErrorProvider")]
    public class CustomErrorProvider : ICustomErrorProvider, ISiteImportExportStartup
    {
        #region .ctor
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);
        Kooboo.CMS.Sites.Persistence.FileSystem.CustomErrorProvider provider;
        SiteDBContext _dbContext;
        public CustomErrorProvider(SiteDBContext dbContext)
        {
            this._dbContext = dbContext;
            provider = new Kooboo.CMS.Sites.Persistence.FileSystem.CustomErrorProvider();
        }
        #endregion

        #region --- Import && Export ---
        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            provider.Import(site, zipStream, @override);
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

        public void Export(Site site, System.IO.Stream outputStream)
        {
            ExportToDisk(site);
            provider.Export(site, outputStream);
        }

        public void ExportToDisk(Site site)
        {
            var allItem = this.All(site).ToList();
            var file = new CustomErrorsFile(site).PhysicalPath;
            locker.EnterWriteLock();
            try
            {
                Serialization.Serialize<List<CustomError>>(allItem, file);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public void ImportToDatabase(Site site, bool @override)
        {
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


        public void InitializeToDB(Site site)
        {
            //
        }
        #endregion

        #region --- CRUD ---
        public void Add(Models.CustomError item)
        {
            UpdateOrAdd(item, item);
        }

        public IEnumerable<Models.CustomError> All()
        {
            return _dbContext.CustomErrors.ToList().Select(it => CustomErrorEntityExtensions.ToCustomError(it));
        }

        public IEnumerable<Models.CustomError> All(Models.Site site)
        {
            return All().Where(it => it.Site.Equals(site));
        }

        public Models.CustomError Get(Models.CustomError dummy)
        {
            return All().FirstOrDefault(it => it.UUID.Equals(dummy.UUID) && it.Site == dummy.Site);
        }

        public void Remove(Models.CustomError item)
        {
            ((IPersistable)item).OnSaving();
            var dummy = _dbContext.CustomErrors.FirstOrDefault(it => it.UUID.Equals(item.UUID, StringComparison.OrdinalIgnoreCase) && it.SiteName.Equals(item.Site.FullName, StringComparison.OrdinalIgnoreCase));
            if (null != dummy)
            {
                _dbContext.CustomErrors.Remove(dummy);
                _dbContext.SaveChanges();
            }
            ((IPersistable)item).OnSaved();
        }

        public void Update(Models.CustomError item, Models.CustomError oldItem)
        {
            UpdateOrAdd(item, oldItem);
        }

        private void UpdateOrAdd(Models.CustomError item, Models.CustomError oldItem)
        {
            ((IPersistable)item).OnSaving();
            var dummy = _dbContext.CustomErrors.FirstOrDefault(it => it.UUID.Equals(oldItem.UUID, StringComparison.OrdinalIgnoreCase)
                && it.SiteName.Equals(oldItem.Site.FullName, StringComparison.OrdinalIgnoreCase));
            if (null != dummy)
            {
                item.ToCustomErrorEntity<CustomErrorEntity>(dummy);
            }
            else
            {
                dummy = item.ToCustomErrorEntity<CustomErrorEntity>();
                _dbContext.CustomErrors.Add(dummy);
            }
            _dbContext.SaveChanges();
            ((IPersistable)item).OnSaved();
        }
        #endregion



    }
}
