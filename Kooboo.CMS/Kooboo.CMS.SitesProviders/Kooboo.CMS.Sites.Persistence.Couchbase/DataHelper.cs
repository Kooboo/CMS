using Enyim.Caching.Memcached;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Couchbase.Operations;
using Couchbase;
namespace Kooboo.CMS.Sites.Persistence.Couchbase
{
    public class DataHelper
    {

        public static string viewTemplate = @"{{""views"":{{""{0}"":{{""map"":""function(doc,meta){{if(doc._datatype_==='{1}'){{emit(meta.id,null);}}}}""}}}}}}";

        public static IView<IViewRow> GetView(CouchbaseClient bucket, string bucketName, string designName, string viewName)
        {
            var view = bucket.GetView(viewName, viewName).Stale(global::Couchbase.StaleMode.False).Reduce(false);
            if (!view.CheckExistsByCache(bucketName, viewName))
            {
                var created = DatabaseHelper.CreateDesignDocument(bucketName, viewName, string.Format(viewTemplate, viewName, ModelExtensions.GetDataType(viewName)));
                if (created)
                {
                    DatabaseHelper.ExistedView.Add(DatabaseHelper.GetViewCacheName(bucketName, viewName));
                    System.Threading.Thread.Sleep(3000);
                }
                else
                {
                    throw new Exception("Create view " + viewName + " faild");
                }
            }
            return view;
        }
        #region DeleteItemByKey
        public static void DeleteItemByKey(Site site, string key)
        {
            var bucket = site.GetClient();

            bucket.ExecuteRemove(key, PersistTo.One);
        }
        #endregion
        #region QueryByKey
        public static T QueryByKey<T>(Site site, string key)
        {
            var bucket = site.GetClient();

            object json;
            var result = bucket.ExecuteTryGet(key, out json);
            if (result.HasValue)
            {
                var obj = ModelExtensions.JsonToObject<T>(json.ToString());
                if (obj is ISiteObject)
                    ((ISiteObject)obj).Site = site;
                return obj;
            }
            else
            {
                return default(T);
            }

        }
        public static T QueryByKey<T>(Site site, string key, Func<Site, string, T> createModel)
            where T : IPersistable
        {
            var bucket = site.GetClient();

            object json;
            var result = bucket.ExecuteTryGet(key, out json);
            if (result.HasValue)
            {
                var rawKey = ModelExtensions.GetRawDocumentKey(key);
                var obj = ModelExtensions.JsonToObject<T>(json.ToString());
                obj.Init(createModel(site, rawKey));
                return obj;
            }
            else
            {
                return default(T);
            }

        }
        #endregion
        #region QueryList
        public static IEnumerable<T> QueryList<T>(Site site, string viewName)
        {
            var bucket = site.GetClient();

            if (bucket != null)
            {
                var view = GetView(bucket, site.GetBucketName(), viewName, viewName);

                var idList = view.Select(it => it.ItemId).ToArray();

                return bucket.ExecuteGet(idList).Select(it => ModelExtensions.JsonToObject<T>(it.Value.Value.ToString()))
                    .Select(it => { if (it is ISiteObject) ((ISiteObject)it).Site = site; return it; });
            }
            else
            {
                return new T[0];
            }

        }
        public static IEnumerable<T> QueryList<T>(Site site, string viewName, Func<Site, string, T> createModel)
            where T : IPersistable
        {
            var bucket = site.GetClient();

            if (bucket != null)
            {
                var view = GetView(bucket, site.GetBucketName(), viewName, viewName);

                var idList = view.Select(it => it.ItemId).ToArray();

                return bucket.ExecuteGet(idList).Select(it => ModelExtensions.ToModel<T>(site, it.Key, it.Value.Value.ToString(), createModel));
            }
            else
            {
                return new T[0];
            }
        }
        #endregion
        #region StoreObject
        public static void StoreObject(ISiteObject o, string key, string dataType)
        {
            var bucket = o.Site.GetClient();

            bucket.ExecuteStore(StoreMode.Set, ModelExtensions.GetBucketDocumentKey(dataType, key), o.ToJson(dataType), PersistTo.One);

        }
        public static void StoreObject(Site site, object o, string key, string dataType)
        {
            ///For both scenarios, you should use an observe command from a client with the persistto argument to verify the persistent state for the document, then force an update of the view using stale=false. This will ensure that the document is correctly updated in the view index.
            var bucket = site.GetClient();

            bucket.ExecuteStore(StoreMode.Set, ModelExtensions.GetBucketDocumentKey(dataType, key), o.ToJson(dataType), PersistTo.One);
        }
        #endregion
    }
}
