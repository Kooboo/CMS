#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Ionic.Zip;
using Kooboo.CMS.Caching;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Caching;
using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Sites.Models;
using Kooboo.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class SiteProvider : ISiteProvider
    {
        #region .ctor
        private ISiteProvider inner;
        public SiteProvider(ISiteProvider inner)
        {
            this.inner = inner;
        }
        #endregion

        #region GetSiteByHostNameNPath
        public Models.Site GetSiteByHostNameNPath(string hostName, string requestPath)
        {
            var domainTable = GetDomainTable();
            var fullPath = hostName + "/" + requestPath.Trim('/') + "/";
            return domainTable.Where(it => fullPath.StartsWith(it.Key, StringComparison.OrdinalIgnoreCase))
                .Select(it => it.Value).FirstOrDefault();
        }
        #endregion

        #region GetDomainTable
        public IDictionary<string, Site> GetDomainTable()
        {
            string cacheKey = string.Format("GetDomainTable");
            return GetCachedData<IDictionary<string, Site>>(cacheKey, () => this.inner.GetDomainTable());

        }
        #endregion

        #region GetCachedData
        private static T GetCachedData<T>(string cacheKey, Func<T> cacheData)
           where T : class
        {
            return CacheManagerFactory.DefaultCacheManager.GlobalObjectCache().GetCache(cacheKey, cacheData);
        }
        #endregion

        #region AllSites
        public IEnumerable<Models.Site> AllSites()
        {
            string cacheKey = "AllSites";
            return GetCachedData<Site[]>(cacheKey, () => inner.AllSites().ToArray());
        }

        #endregion

        #region AllRootSites
        public IEnumerable<Models.Site> AllRootSites()
        {
            string cacheKey = "AllRootSites";
            return GetCachedData<Site[]>(cacheKey, () => inner.AllRootSites().ToArray());
        }
        #endregion

        #region ChildSites
        public IEnumerable<Models.Site> ChildSites(Models.Site site)
        {
            string cacheKey = "ChildSites-SiteName:" + site.FullName;
            return GetCachedData<Site[]>(cacheKey, () => inner.ChildSites(site).ToArray());
        }
        #endregion

        #region All
        public IEnumerable<Models.Site> All(Models.Site site)
        {
            return inner.All(site);
        }
        public IEnumerable<Site> All()
        {
            return inner.All();
        }
        #endregion

        #region Get
        public Models.Site Get(Models.Site dummy)
        {
            var cacheKey = GetCacheKey(dummy);
            var site = (Site)dummy.ObjectCache().Get(cacheKey);
            if (site == null)
            {
                site = inner.Get(dummy);
                if (site == null)
                {
                    return site;
                }
                dummy.ObjectCache().Add(cacheKey, site, Kooboo.CMS.Caching.ObjectCacheExtensions.DefaultCacheItemPolicy);
            }
            return site;
        }
        #endregion

        #region GetCacheKey
        private static string GetCacheKey(Models.Site site)
        {
            var cacheKey = string.Format("Site:{0}", site.FullName.ToLower());
            return cacheKey;
        }
        #endregion

        #region Add
        public void Add(Models.Site item)
        {
            inner.Add(item);
            CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
        }
        #endregion

        #region Update
        public void Update(Models.Site @new, Models.Site old)
        {

            inner.Update(@new, old);

            CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
            @new.ClearCache();
        }
        #endregion

        #region Remove
        public void Remove(Models.Site item)
        {
            inner.Remove(item);

            item.ClearCache();
            CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
        }
        #endregion

        #region Offline
        public void Offline(Site site)
        {

            inner.Offline(site);
            CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
        }
        #endregion

        #region Online
        public void Online(Site site)
        {
            inner.Online(site);
            CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
        }
        #endregion

        #region IsOnline
        public bool IsOnline(Site site)
        {
            return inner.IsOnline(site);
        }
        #endregion

        #region Create
        public Site Create(Site parentSite, string siteName, Stream packageStream, string repositoryName)
        {
            var site = inner.Create(parentSite, siteName, packageStream, repositoryName);
            CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
            return site;
        }
        #endregion

        #region Initialize
        public void Initialize(Site site)
        {
            inner.Initialize(site);
        }
        #endregion

        #region Export
        public void Export(Site site, Stream outputStream, bool includeDatabase, bool includeSubSites)
        {
            inner.Export(site, outputStream, includeDatabase, includeSubSites);
        }
        #endregion        
    }
}
