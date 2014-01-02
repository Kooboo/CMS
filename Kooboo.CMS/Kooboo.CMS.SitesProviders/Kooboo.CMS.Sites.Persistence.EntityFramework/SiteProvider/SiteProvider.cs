#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Caching;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Membership.Persistence;
using Kooboo.CMS.Sites.Globalization;
using Kooboo.CMS.Sites.Models;
using Kooboo.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.SiteProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISiteProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Site>), Order = 100)]
    public class SiteProvider : Kooboo.CMS.Sites.Persistence.FileSystem.SiteProvider
    {
        #region .ctor
        SiteDBContext _dbContext;
        public SiteProvider(IBaseDir baseDir, IMembershipProvider membershipProvider, IElementRepositoryFactory elementRepositoryFactory, SiteDBContext dbContext)
            : base(baseDir, membershipProvider, elementRepositoryFactory)
        {
            this._dbContext = dbContext;
        }
        #endregion

        public override void Initialize(Site site)
        {
            UpdateOrAdd(site, site);
            base.Initialize(site);
        }
        #region --- CRUD ---

        private void UpdateOrAdd(Site @new, Site old)
        {
            ((IPersistable)@new).OnSaving();
            var dummy = _dbContext.SiteSettings.FirstOrDefault(it => it.FullName.Equals(old.FullName, StringComparison.OrdinalIgnoreCase));
            if (null != dummy)
            {
                @new.ToSiteSettingEntity(dummy);
            }
            else
            {
                dummy = @new.ToSiteSettingEntity<SiteEntity>();
                _dbContext.SiteSettings.Add(dummy);
            }
            _dbContext.SaveChanges();
            ((IPersistable)@new).OnSaved();
            ClearCache();
        }

        public override Site Get(Site dummyObject)
        {
            var site = GetAllSites().Where(it => it.FullName.Equals(dummyObject.FullName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (null == site)
            {
                return base.Get(dummyObject);
            }
            return site;
        }

        public override void Update(Site @new, Site old)
        {
            UpdateOrAdd(@new, old);
            base.Update(@new, old);
        }

        public override void Add(Site item)
        {
            UpdateOrAdd(item, item);
            base.Add(item);
        }

        public override void Remove(Site item)
        {
            var obj = _dbContext.SiteSettings.FirstOrDefault(it => it.FullName.Equals(item.FullName, StringComparison.OrdinalIgnoreCase));
            if (null != obj)
            {
                _dbContext.SiteSettings.Remove(obj);
                _dbContext.SaveChanges();
            }
            foreach (var sub in Providers.SiteProvider.ChildSites(item))
            {
                Remove(sub);
            }
            base.Remove(item);
            ClearCache();
        }
        #endregion

        private const string cacheKey = "SqlServer:SiteProvider:SitesTable";

        private List<Site> GetAllSites()
        {
            var cacheObject = CacheManagerFactory.DefaultCacheManager.GlobalObjectCache();
            return cacheObject.GetCache<List<Site>>(cacheKey, () =>
            {
                return _dbContext.SiteSettings
                    .ToArray().Select(it => it.ToSite()).ToList();
            });
        }

        private static void ClearCache()
        {
            CacheManagerFactory.DefaultCacheManager.GlobalObjectCache().Remove(cacheKey);
        }
    }

    public static class SiteEntityHelper
    {
        private static Type[] KnownTypes = new Type[]{
                typeof(Site),
                typeof(PagePosition),
                typeof(ViewPosition),
                typeof(ModulePosition),
                typeof(HtmlPosition),
                typeof(ContentPosition),
                typeof(HtmlBlockPosition)
                };
        public static T ToSiteSettingEntity<T>(this Site model)
            where T : ISiteSettingEntity, new()
        {
            return ToSiteSettingEntity(model, new T());
        }
        public static T ToSiteSettingEntity<T>(this Site model, T entity)
            where T : ISiteSettingEntity
        {
            entity.FullName = model.FullName;
            if (null != model.Parent)
            {
                entity.ParentName = model.Parent.FullName;
            }
            entity.ObjectXml = DataContractSerializationHelper.SerializeAsXml(model, KnownTypes);
            return entity;
        }
        public static Site ToSite(this ISiteSettingEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            Site dummy;
            if (String.IsNullOrWhiteSpace(entity.ParentName))
            {
                dummy = new Site(entity.FullName);
            }
            else
            {
                dummy = new Site(new Site(entity.ParentName), entity.FullName);
            }
            var site = DataContractSerializationHelper.DeserializeFromXml<Site>(entity.ObjectXml, KnownTypes);
            ((IPersistable)site).Init(dummy);
            return site;
        }
    }
}
