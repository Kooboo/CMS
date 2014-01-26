using Couchbase;
using Couchbase.Configuration;
using Couchbase.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Enyim.Caching.Memcached;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence.Couchbase
{
    public static class DatabaseHelper
    {
        static Dictionary<string, CouchbaseClient> couchbaseClients = new Dictionary<string, CouchbaseClient>(StringComparer.OrdinalIgnoreCase);
        static string defaultClientName = "KoobooDefaultCouchbaseClient";
        #region GetCouchbaseClientConfiguration
        public static CouchbaseClientConfiguration GetCouchbaseClientConfiguration()
        {
            CouchbaseClientConfiguration cf = new CouchbaseClientConfiguration();
            cf.Username = DatabaseSettings.Instance.Username;
            cf.Password = DatabaseSettings.Instance.Password;
            DatabaseSettings.Instance.Urls.ForEach((url, index) =>
            {
                cf.Urls.Add(url);
            });
            return cf;
        }
        #endregion

        #region GetClient
        public static CouchbaseClient GetClient(this Site site)
        {
            var key = string.Empty;
            if (site == null || string.IsNullOrEmpty(site.FullName))
            {
                key = defaultClientName;
            }
            else
            {
                key = site.FullName;
            }
            if (!couchbaseClients.ContainsKey(key))
            {
                lock (couchbaseClients)
                {
                    var cf = GetCouchbaseClientConfiguration();
                    cf.Bucket = site.GetBucketName();
                    cf.BucketPassword = site.GetBucketPassword();
                    // performance 
                    //if (!ExistBucket(cf.Bucket))
                    //{
                    //    return null;
                    //}
                    CouchbaseClient client = new CouchbaseClient(cf);
                    couchbaseClients[key] = client;
                }
            }
            return couchbaseClients[key];
        }
        #endregion

        #region GetCouchbaseCluster
        public static CouchbaseCluster GetCouchbaseCluster()
        {
            var cf = GetCouchbaseClientConfiguration();
            CouchbaseCluster cc = new global::Couchbase.Management.CouchbaseCluster(cf);
            return cc;
        }
        #endregion

        #region ExistBucket
        public static bool ExistBucket(string bucketName)
        {
            var cc = GetCouchbaseCluster();
            var buckets = cc.ListBuckets();
            return buckets.Where(it => it.Name.Equals(bucketName)).Count() > 0;
        }
        #endregion

        #region ExistView
        public static List<string> ExistedView = new List<string>();

        public static string GetViewCacheName(string bucketName, string viewName)
        {
            return bucketName + "|" + viewName;
        }
        public static bool CheckExistsByCache(this IView<IViewRow> view,string bucketName,string viewName)
        {
            var viewCacheName = GetViewCacheName(bucketName, viewName);
            if (ExistedView.Contains(viewCacheName))
            {
                return true;
            }
            else
            {
                if (view.CheckExists())
                {
                    ExistedView.Add(GetViewCacheName(bucketName, viewName));
                    return true;
                }
                else
                {
                    return false;
                }
 
            }
        }
        #endregion

        #region CreateBucket
        public static void CreateBucket(string bucketName)
        {
            var cc = GetCouchbaseCluster();

            var cf = DatabaseHelper.GetCouchbaseClientConfiguration();

            Bucket bucket = new Bucket();
            bucket.Name = bucketName;
            bucket.AuthType = AuthTypes.Sasl;
            bucket.BucketType = BucketTypes.Membase;
            bucket.Quota = new Quota() { RAM = DatabaseSettings.Instance.BucketRAM };//RamQuotaMB must be at least 100
            bucket.FlushOption = FlushOptions.Enabled;//支持清空
            bucket.ReplicaNumber = (ReplicaNumbers)DatabaseSettings.Instance.ReplicaNumber;
            bucket.ReplicaIndex = DatabaseSettings.Instance.ReplicaIndex;
            cc.CreateBucket(bucket);
        }
        #endregion

        #region DeleteBucket
        public static void DeleteBucket(this Site site)
        {
            var cc = GetCouchbaseCluster();
            cc.DeleteBucket(site.GetBucketName());
        }
        #endregion

        #region CreateDesignDocument
        public static bool CreateDesignDocument(string bucket, string name, string document)
        {
            var cc = GetCouchbaseCluster();
            return cc.CreateDesignDocument(bucket, name, document);
        }
        #endregion

        #region DeleteDesignDocument
        public static bool DeleteDesignDocument(string bucket, string name)
        {
            var cc = GetCouchbaseCluster();
            return cc.DeleteDesignDocument(bucket, name);
        }
        #endregion
    }
}
