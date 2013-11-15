using Couchbase;
using Couchbase.Configuration;
using Couchbase.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Content.Models;
using Newtonsoft.Json;
using Enyim.Caching.Memcached;

namespace Kooboo.CMS.Content.Persistence.Couchbase
{
    public static class DatabaseHelper
    {
       
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

        public static CouchbaseClient GetClient(this Repository repository)
        {
            var cf = GetCouchbaseClientConfiguration();
            cf.Bucket = repository.GetBucketName();
            cf.BucketPassword = repository.GetBucketPassword();
            // performance 
            //if (!ExistBucket(cf.Bucket))
            //{
            //    return null;
            //}
            CouchbaseClient client = new CouchbaseClient(cf);
            return client;           
        }

        public static CouchbaseCluster GetCouchbaseCluster()
        {
            var cf = GetCouchbaseClientConfiguration();
            CouchbaseCluster cc = new global::Couchbase.Management.CouchbaseCluster(cf);
            return cc;
        }

        public static bool ExistBucket(string bucketName)
        {
            var cc = GetCouchbaseCluster();
            var buckets = cc.ListBuckets();
            return buckets.Where(it => it.Name.Equals(bucketName)).Count() > 0;
        }

        public static void CreateBucket(Bucket bucket)
        {
            var cc = GetCouchbaseCluster();
            cc.CreateBucket(bucket);
        }

        public static void DeleteBucket(this Repository repository)
        {
            var cc = GetCouchbaseCluster();
            cc.DeleteBucket(repository.GetBucketName());
        }

        public static bool CreateDesignDocument(string bucket, string name, string document)
        {
            var cc = GetCouchbaseCluster();
            return cc.CreateDesignDocument(bucket, name, document);
        }

        public static bool DeleteDesignDocument(string bucket, string name)
        {
            var cc = GetCouchbaseCluster();
            return cc.DeleteDesignDocument(bucket, name);
        }

        public static IList<IViewRow> GetCategories(this Repository repository)
        {
            using (var bucket = repository.GetClient())
            {
                var view = bucket.GetView(repository.GetDefaultViewDesign(), "All_CategoriesData").Stale(StaleMode.False);
                return view.ToList();
            }
        }

        public static bool CreateDefaultViews(this Repository repository)
        {
            return CreateDesignDocument(repository.GetBucketName(), repository.GetDefaultViewDesign(), repository.DefaultViews());
        }

        //public static bool CreateSchemaViews(this Repository repository,Schema schema)
        //{
        //    return CreateDesignDocument(repository.GetBucketName(), schema.GetDesginName(), schema.SchemaViews());
        //}

        //public static bool DeleteSchemaViews(this Repository repository, Schema schema)
        //{
        //    return DeleteDesignDocument(repository.GetBucketName(), schema.GetDesginName());
        //}

        //public static IList<IViewRow> GetSchemas(this Schema schema)
        //{
        //    using (var bucket = schema.Repository.GetClient())
        //    {
        //        var view = bucket.GetView(schema.GetDesginName(), "All");
        //        return view.ToList();
        //    }
        //}

        //public static void DropSchemas(this Schema schema)
        //{
        //    var list = schema.GetSchemas();
        //    using (var bucket = schema.Repository.GetClient())
        //    {
        //        list.ForEach((it, index) =>
        //        {
        //            bucket.Remove(it.ItemId);
        //        });
        //    }
        //}


    }
}
