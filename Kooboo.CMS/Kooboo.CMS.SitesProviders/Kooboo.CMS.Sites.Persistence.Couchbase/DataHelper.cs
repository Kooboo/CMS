using Enyim.Caching.Memcached;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Couchbase.Operations;
namespace Kooboo.CMS.Sites.Persistence.Couchbase
{
    public class DataHelper
    {
        #region DeleteItemByKey
        public static void DeleteItemByKey(Site site, string key)
        {
            using (var bucket = site.GetClient())
            {
                bucket.ExecuteRemove(key, PersistTo.One);
            }
        }
        #endregion
        #region QueryByKey
        public static T QueryByKey<T>(Site site, string viewName, string key)
        {
            using (var bucket = site.GetClient())
            {
                object json;
                var result = bucket.ExecuteTryGet(key, out json);
                if (result.HasValue)
                {
                    var obj = ModelExtensions.JsonToObject<T>(json.ToString());
                    return obj;
                }
                else
                {
                    return default(T);
                }
            }
        }
        public static T QueryByKey<T>(Site site, string viewName, string key, Func<Site, string, T> createModel)
            where T : IPersistable
        {
            using (var bucket = site.GetClient())
            {
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
        }
        #endregion
        #region QueryList
        public static IEnumerable<T> QueryList<T>(Site site, string viewName)
        {
            using (var bucket = site.GetClient())
            {
                if (bucket != null)
                {
                    var view = bucket.GetView(ModelExtensions.DesignName, viewName).Stale(global::Couchbase.StaleMode.False);

                    var rows = view.ToArray();

                    return rows.Select(it => it.ToObject<T>());
                }
                else
                {
                    return new T[0];
                }
            }
        }
        public static IEnumerable<T> QueryList<T>(Site site, string viewName, Func<Site, string, T> createModel)
            where T : IPersistable
        {
            using (var bucket = site.GetClient())
            {
                if (bucket != null)
                {
                    var view = bucket.GetView(ModelExtensions.DesignName, viewName).Stale(global::Couchbase.StaleMode.False);

                    var rows = view.ToArray();

                    return rows.Select(it => it.ToModel<T>(site, createModel));
                }
                else
                {
                    return new T[0];
                }

            }
        }
        #endregion
        #region StoreObject
        public static void StoreObject(ISiteObject o, string key, string dataType)
        {
            using (var bucket = o.Site.GetClient())
            {
                bucket.ExecuteStore(StoreMode.Set, ModelExtensions.GetBucketDocumentKey(dataType, key), o.ToJson(dataType), PersistTo.One);
            }
        }
        public static void StoreObject(Site site, object o, string key, string dataType)
        {
            ///For both scenarios, you should use an observe command from a client with the persistto argument to verify the persistent state for the document, then force an update of the view using stale=false. This will ensure that the document is correctly updated in the view index.
            using (var bucket = site.GetClient())
            {
                bucket.ExecuteStore(StoreMode.Set, ModelExtensions.GetBucketDocumentKey(dataType, key), o.ToJson(dataType), PersistTo.One);
            }
        }
        #endregion
    }
}
