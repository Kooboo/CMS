using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Sites.Models;
using System.IO;
using Kooboo.Globalization;
using Ionic.Zip;
using Kooboo.CMS.Content.Caching;
using Kooboo.CMS.Caching;

namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class SiteProvider : ISiteProvider
    {
        private ISiteProvider inner;
        public SiteProvider(ISiteProvider inner)
        {
            this.inner = inner;
        }
        public Models.Site GetSiteByHostNameNPath(string hostName, string requestPath)
        {
            var domainTable = GetDomainTable();
            var fullPath = hostName + "/" + requestPath.Trim('/') + "/";
            return domainTable.Where(it => fullPath.StartsWith(it.Key, StringComparison.OrdinalIgnoreCase))
                .Select(it => it.Value).FirstOrDefault();
        }

        public IDictionary<string, Site> GetDomainTable()
        {
            string cacheKey = string.Format("GetDomainTable");
            return GetCachedData<IDictionary<string, Site>>(cacheKey, () => this.inner.GetDomainTable());

        }
        private static T GetCachedData<T>(string cacheKey, Func<T> cacheData)
            where T : class
        {
            return CacheManagerFactory.DefaultCacheManager.GlobalObjectCache().GetCache(cacheKey, cacheData);
        }

        //public Models.Site GetSiteByHostName(string hostName)
        //{
        //    string cacheKey = string.Format("GetSiteByHostName:HostName-{0}", hostName.ToLower());
        //    return GetCachedData<Site>(cacheKey, () => inner.GetSiteByHostName(hostName));
        //}

        public IEnumerable<Models.Site> AllSites()
        {
            string cacheKey = "AllSites";
            return GetCachedData<Site[]>(cacheKey, () => inner.AllSites().ToArray());
        }

        public IEnumerable<Models.Site> AllRootSites()
        {
            string cacheKey = "AllRootSites";
            return GetCachedData<Site[]>(cacheKey, () => inner.AllRootSites().ToArray());
        }

        public IEnumerable<Models.Site> ChildSites(Models.Site site)
        {
            string cacheKey = "ChildSites-SiteName:" + site.FullName;
            return GetCachedData<Site[]>(cacheKey, () => inner.ChildSites(site).ToArray());
        }

        public IQueryable<Models.Site> All(Models.Site site)
        {
            return inner.All(site);
        }

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
                dummy.ObjectCache().Add(cacheKey, site, CacheProviderFactory.DefaultCacheItemPolicy);
            }
            return site;
        }

        private static string GetCacheKey(Models.Site site)
        {
            var cacheKey = string.Format("Site:{0}", site.FullName.ToLower());
            return cacheKey;
        }

        public void Add(Models.Site item)
        {
            inner.Add(item);
            CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
        }

        public void Update(Models.Site @new, Models.Site old)
        {

            inner.Update(@new, old);

            CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
            @new.ClearCache();
        }

        public void Remove(Models.Site item)
        {


            inner.Remove(item);

            item.ClearCache();
            CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
        }


        public void Offline(Site site)
        {

            inner.Offline(site);
            CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
        }

        public void Online(Site site)
        {

            inner.Online(site);
            CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
        }

        public bool IsOnline(Site site)
        {
            return inner.IsOnline(site);
        }

        public Site Create(Site parentSite, string siteName, Stream packageStream, string repositoryName)
        {
            var site = inner.Create(parentSite, siteName, packageStream, repositoryName);
            CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();
            return site;
        }

        public void Initialize(Site site)
        {
            inner.Initialize(site);
        }

        public void Export(Site site, Stream outputStream)
        {
            inner.Export(site, outputStream);
        }
    }
}
