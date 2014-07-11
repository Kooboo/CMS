using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.ABTestProvider
{
    using Kooboo.Common.ObjectContainer;
    using Kooboo.CMS.Common.Persistence.Non_Relational;
    using Kooboo.CMS.Sites.ABTest;
    using Kooboo.CMS.Sites.Models;
    using Kooboo.CMS.Common;

    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IABSiteSettingProvider), Order = 100)]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IProvider<ABSiteSetting>), Order = 100)]
    public class ABSiteSettingProvider : IABSiteSettingProvider
    {
        Kooboo.CMS.Sites.Persistence.FileSystem.ABSiteSettingProvider provider;
        SiteDBContext _dbContext;
        IBaseDir _baseDir;
        public ABSiteSettingProvider(SiteDBContext dbContext, IBaseDir baseDir)
        {
            this._dbContext = dbContext;
            this._baseDir = baseDir;
            provider = new Kooboo.CMS.Sites.Persistence.FileSystem.ABSiteSettingProvider(_baseDir);
        }

        #region --- Import / Export ---
        public void SaveToDatabase(bool @override)
        {
            var allItem = provider.All();
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
        /// <summary>
        /// Remove Items in Disk
        /// </summary>
        private void Clean()
        {
            var oldItems = provider.All().ToList();

            foreach (var item in oldItems)
            {
                provider.Remove(item);
            }
        }

        public void Export(IEnumerable<ABSiteSetting> sources, Stream outputStream)
        {
            var newItems = sources.ToList();
            foreach (var item in newItems)
            {
                provider.Add(item.AsActual());
            }
            provider.Export(sources, outputStream);
        }

        public void Import(Stream zipStream, bool @override)
        {
            Clean();
            provider.Import(zipStream, @override);
            SaveToDatabase(@override);
        }
        #endregion

        public IEnumerable<ABSiteSetting> All()
        {
            return _dbContext.ABSiteSettings.ToList().Select(it => it.ToABSiteSetting());
        }

        public ABSiteSetting Get(ABSiteSetting dummy)
        {
            var temp = _dbContext.ABSiteSettings.Where(it => it.UUID.Equals(dummy.UUID, StringComparison.OrdinalIgnoreCase)
                && it.SiteName.Equals(dummy.MainSite, StringComparison.OrdinalIgnoreCase))
                .ToList()
                .Select(it => it.ToABSiteSetting())
                .FirstOrDefault();
            return temp;
        }

        public void Add(ABSiteSetting item)
        {
            if (null != item)
            {
                UpdateOrAdd(item, item);
            }
        }

        public void Update(ABSiteSetting @new, ABSiteSetting old)
        {
            UpdateOrAdd(@new, old);
        }

        public void Remove(ABSiteSetting item)
        {
            ((IPersistable)item).OnSaving();
            var dummy = _dbContext.ABSiteSettings.FirstOrDefault(it => it.UUID.Equals(item.UUID, StringComparison.OrdinalIgnoreCase)
                && it.SiteName.Equals(item.MainSite, StringComparison.OrdinalIgnoreCase));
            if (null != dummy)
            {
                _dbContext.ABSiteSettings.Remove(dummy);
                _dbContext.SaveChanges();
            }
            ((IPersistable)item).OnSaved();
        }

        private void UpdateOrAdd(ABSiteSetting item, ABSiteSetting oldItem)
        {
            ((IPersistable)item).OnSaving();
            var dummy = _dbContext.ABSiteSettings.FirstOrDefault(it => it.UUID.Equals(oldItem.UUID, StringComparison.OrdinalIgnoreCase)
                && it.SiteName.Equals(oldItem.MainSite, StringComparison.OrdinalIgnoreCase));
            if (null != dummy)
            {
                item.ToABSiteSettingEntity<ABSiteSettingEntity>(dummy);
            }
            else
            {
                dummy = item.ToABSiteSettingEntity<ABSiteSettingEntity>();
                _dbContext.ABSiteSettings.Add(dummy);
            }
            _dbContext.SaveChanges();
            ((IPersistable)item).OnSaved();
        }
    }
}
