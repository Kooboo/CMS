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

    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IABRuleSettingProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<ABRuleSetting>), Order = 100)]
    public class ABRuleSettingsProvider : IABRuleSettingProvider, ISiteImportExportStartup
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);
        SiteDBContext _dbContext;
        IBaseDir _baseDir;
        Kooboo.CMS.Sites.Persistence.FileSystem.ABRuleSettingProvider provider;
        const string DIRNAME = "ABRuleSettings";
        public ABRuleSettingsProvider(SiteDBContext dbContext, IBaseDir baseDir)
        {
            this._dbContext = dbContext;
            this._baseDir = baseDir;
            provider = new Kooboo.CMS.Sites.Persistence.FileSystem.ABRuleSettingProvider(_baseDir);
        }
        #region --- Import / Export ---
        private void Clean(Site site = null)
        {
            var oldItems = (site == null ? provider.All() : provider.All(site)).ToList();

            foreach (var item in oldItems)
            {
                provider.Remove(item);
            }
        }
        public void Export(IEnumerable<ABTest.ABRuleSetting> sources, Stream outputStream)
        {
            var allItem = sources.ToList();
            foreach (var item in allItem)
            {
                var dummy = item.AsActual();
                provider.Add(dummy);
            }
            provider.Export(sources, outputStream);
        }

        public void ExportToDisk(Site site)
        {
            var allItem = this.All(site);
            foreach (var item in allItem)
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
        #endregion

        public IEnumerable<ABTest.ABRuleSetting> All(Site site)
        {
            if (null == site)
            {
                return _dbContext.ABRuleSettings.Where(it => String.IsNullOrEmpty(it.SiteName)).ToList().Select(it => it.ToABRuleSetting());
            }
            return _dbContext.ABRuleSettings.Where(it => it.SiteName.Equals(site.FullName, StringComparison.OrdinalIgnoreCase)).ToList().Select(it => it.ToABRuleSetting());
        }

        public IEnumerable<ABTest.ABRuleSetting> All()
        {
            return _dbContext.ABRuleSettings.ToList().Select(it => it.ToABRuleSetting());
        }

        public ABTest.ABRuleSetting Get(ABTest.ABRuleSetting dummy)
        {
            var temp = _dbContext.ABRuleSettings.Where(it => it.UUID.Equals(dummy.UUID, StringComparison.OrdinalIgnoreCase));
            if (null != dummy.Site)
            {
                temp = temp.Where(it => it.SiteName.Equals(dummy.Site.FullName, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                temp = temp.Where(it => String.IsNullOrEmpty(it.SiteName));
            }
            var result = temp.ToList()
                .Select(it => it.ToABRuleSetting())
                .FirstOrDefault();
            return result;
        }

        public void Add(ABTest.ABRuleSetting item)
        {
            if (null != item)
            {
                UpdateOrAdd(item, item);
            }
        }

        public void Update(ABTest.ABRuleSetting @new, ABTest.ABRuleSetting old)
        {
            UpdateOrAdd(@new, old);
        }

        public void Remove(ABTest.ABRuleSetting item)
        {
            ((IPersistable)item).OnSaving();
            var dummy = _dbContext.ABRuleSettings.Where(it => it.UUID.Equals(item.UUID, StringComparison.OrdinalIgnoreCase));
            if (null != item.Site)
            {
                dummy = dummy.Where(it => it.SiteName.Equals(item.Site.FullName, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                dummy = dummy.Where(it => String.IsNullOrEmpty(it.SiteName));
            }
            var temp = dummy.FirstOrDefault();
            if (null != temp)
            {
                _dbContext.ABRuleSettings.Remove(temp);
                _dbContext.SaveChanges();
            }
            ((IPersistable)item).OnSaved();
        }


        private void UpdateOrAdd(ABRuleSetting item, ABRuleSetting oldItem)
        {
            ((IPersistable)item).OnSaving();
            var dummy = _dbContext.ABRuleSettings.Where(it => it.UUID.Equals(oldItem.UUID, StringComparison.OrdinalIgnoreCase));
            if (null == oldItem.Site)
            {
                dummy = dummy.Where(it => String.IsNullOrEmpty(it.SiteName));
            }
            else
            {
                dummy = dummy.Where(it => it.SiteName.Equals(oldItem.Site.FullName, StringComparison.OrdinalIgnoreCase));
            }
            var temp = dummy.FirstOrDefault();
            if (null != temp)
            {
                item.ToABRuleSettingEntity<ABRuleSettingEntity>(temp);
            }
            else
            {
                temp = item.ToABRuleSettingEntity<ABRuleSettingEntity>();
                _dbContext.ABRuleSettings.Add(temp);
            }
            _dbContext.SaveChanges();
            ((IPersistable)item).OnSaved();
        }
    }
}
