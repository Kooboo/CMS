using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.ABTestProvider
{
    using System.IO;
    using Kooboo.CMS.Sites.Models;
    using Kooboo.CMS.Common.Persistence.Non_Relational;
    using Kooboo.CMS.Sites.ABTest;
    using Kooboo.CMS.Common;

    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IABPageSettingProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<ABPageSetting>), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISiteExportableProvider), Order = 100, Key = "ABPageSettingsProvider")]
    public class ABPageSettingsProvider : IABPageSettingProvider, ISiteImportExportStartup
    {
        #region .ctor
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);
        SiteDBContext _dbContext;
        const string DIRNAME = "ABPageSettings";
        Kooboo.CMS.Sites.Persistence.FileSystem.ABPageSettingProvider provider;
        public ABPageSettingsProvider(SiteDBContext dbContext)
        {
            this._dbContext = dbContext;
            provider = new Kooboo.CMS.Sites.Persistence.FileSystem.ABPageSettingProvider();
        }
        #endregion

        #region Export
        public void Export(Site site, IEnumerable<ABPageSetting> sources, Stream outputStream)
        {
            var allItem = sources.ToList();
            foreach (var item in allItem)
            {
                var dummy = item.AsActual();
                provider.Add(dummy);
            }
            provider.Export(site, sources, outputStream);
        }

        public void ExportToDisk(Site site)
        {
            var allItems = this.All(site);
            foreach (var item in allItems)
            {
                provider.Add(item.AsActual());
            }
        }

        public void Import(Site site, Stream zipStream, bool @override)
        {
            Clean(site);
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

        }
        #endregion

        #region CURD
        private void UpdateOrAdd(ABPageSetting item, ABPageSetting oldItem)
        {
            ((IPersistable)item).OnSaving();
            var dummy = _dbContext.ABPageSettings.FirstOrDefault(it => it.UUID.Equals(oldItem.UUID, StringComparison.OrdinalIgnoreCase)
                && it.SiteName.Equals(oldItem.Site.FullName, StringComparison.OrdinalIgnoreCase));
            if (null != dummy)
            {
                item.ToABPageSettingEntity<ABPageSettingEntity>(dummy);
            }
            else
            {
                dummy = item.ToABPageSettingEntity<ABPageSettingEntity>();
                _dbContext.ABPageSettings.Add(dummy);
            }
            _dbContext.SaveChanges();
            ((IPersistable)item).OnSaved();
        }

        private void Clean(Site site = null)
        {
            var oldItems = (site == null ? provider.All() : provider.All(site)).ToList();

            foreach (var item in oldItems)
            {
                provider.Remove(item);
            }
        }
        public IEnumerable<ABPageSetting> All(Site site)
        {
            return _dbContext.ABPageSettings.Where(it => it.SiteName.Equals(site.FullName, StringComparison.OrdinalIgnoreCase)).ToList().Select(it => it.ToABPageSetting());
        }

        public IEnumerable<ABPageSetting> All()
        {
            return _dbContext.ABPageSettings.ToList().Select(it => it.ToABPageSetting());
        }

        public ABPageSetting Get(ABPageSetting dummy)
        {
            return _dbContext.ABPageSettings.Where(it => it.UUID.Equals(dummy.UUID, StringComparison.OrdinalIgnoreCase) && it.SiteName.Equals(dummy.Site.FullName, StringComparison.OrdinalIgnoreCase))
                .ToList()
                .Select(it => it.ToABPageSetting()).FirstOrDefault();
        }

        public void Add(ABPageSetting item)
        {
            if (null != item)
            {
                UpdateOrAdd(item, item);
            }
        }

        public void Update(ABPageSetting @new, ABPageSetting old)
        {
            UpdateOrAdd(@new, old);
        }

        public void Remove(ABPageSetting item)
        {
            ((IPersistable)item).OnSaving();
            var dummy = _dbContext.ABPageSettings.FirstOrDefault(it => it.UUID.Equals(item.UUID, StringComparison.OrdinalIgnoreCase) && it.SiteName.Equals(item.Site.FullName, StringComparison.OrdinalIgnoreCase));
            if (null != dummy)
            {
                _dbContext.ABPageSettings.Remove(dummy);
                _dbContext.SaveChanges();
            }
            ((IPersistable)item).OnSaved();
        }
        #endregion
    }
}
