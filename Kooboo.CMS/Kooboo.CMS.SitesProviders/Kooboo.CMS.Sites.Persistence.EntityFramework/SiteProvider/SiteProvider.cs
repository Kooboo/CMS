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
        Kooboo.CMS.Sites.Persistence.EntityFramework.UrlRedirectsProvider.UrlRedirectProvider urlRedirectProvider;
        Kooboo.CMS.Sites.Persistence.EntityFramework.CustomErrorsProvider.CustomErrorProvider customErrorProvider;
        public SiteProvider(IBaseDir baseDir, IMembershipProvider membershipProvider, IElementRepositoryFactory elementRepositoryFactory, SiteDBContext dbContext)
            : base(baseDir, membershipProvider, elementRepositoryFactory)
        {
            this._dbContext = dbContext;
            urlRedirectProvider = new UrlRedirectsProvider.UrlRedirectProvider(_dbContext);
            customErrorProvider = new CustomErrorsProvider.CustomErrorProvider(_dbContext);
        }
        #endregion

        public override void Initialize(Site site)
        {
            UpdateOrAdd(site, site);
            base.Initialize(site);
            urlRedirectProvider.SaveToDatabase(site, true);
            customErrorProvider.SaveToDatabase(site, true);
        }

        public override void Export(Site site, System.IO.Stream outputStream, bool includeDatabase, bool includeSubSites)
        {
            urlRedirectProvider.InitializeFromDatabase(site);
            customErrorProvider.InitializeFromDatabase(site);
            base.Export(site, outputStream, includeDatabase, includeSubSites);
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
            ((IPersistable)item).OnSaving();
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
            ((IPersistable)item).OnSaved();
        }
        #endregion

        private List<Site> GetAllSites()
        {
            return _dbContext.SiteSettings.ToArray().Select(it => it.ToSite()).ToList();

        }

    }


}
