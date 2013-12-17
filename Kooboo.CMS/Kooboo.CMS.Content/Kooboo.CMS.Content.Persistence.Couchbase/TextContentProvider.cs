using Enyim.Caching.Memcached;
using Kooboo.CMS.Content.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Kooboo.CMS.Content.Persistence.Default;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Couchbase;
using Kooboo.CMS.Content.Query;
using Couchbase.Operations;

namespace Kooboo.CMS.Content.Persistence.Couchbase
{
    public class TextContentProvider : ITextContentProvider
    {
        #region Category
        public void AddCategories(Models.TextContent content, params Models.Category[] categories)
        {
            if (categories != null && categories.Length > 0)
            {
                var bucket = content.GetRepository().GetClient();

                categories.ForEach((it, index) =>
                {
                    bucket.ExecuteStore(StoreMode.Set, it.GetDocumentKey(), it.ToJson(), PersistTo.One);
                });
            }
        }

        public void ClearCategories(Models.TextContent content)
        {
            var bucket = content.GetRepository().GetClient();

            var view = bucket.GetView(content.GetRepository().GetDefaultViewDesign(), "CategorisData_By_ContentUUID").Stale(StaleMode.False).Reduce(false);
            var ret = view.Where(it => it.Info["value"].ToString().Equals(content.UUID)).Select(it => it.ItemId);
            ret.ForEach((key, index) =>
            {
                bucket.Remove(key);
            });
        }

        public void DeleteCategories(Models.TextContent content, params Models.Category[] categories)
        {
            var bucket = content.GetRepository().GetClient();

            categories.ForEach((it, index) =>
            {
                bucket.ExecuteRemove(it.GetDocumentKey(), PersistTo.One);
            });
        }
        #endregion

        #region Export/Import
        public IEnumerable<Models.Category> ExportCategoryData(Models.Repository repository)
        {
            var ret = repository.QueryByCategory(null).Select(it => it.ToCategory());
            return ret;

        }

        public IEnumerable<IDictionary<string, object>> ExportSchemaData(Models.Schema schema)
        {
            return schema.CreateQuery().ToArray().Select(it => new Dictionary<string, object>(it));
        }

        public void ImportCategoryData(Models.Repository repository, IEnumerable<Models.Category> data)
        {
            var bucket = repository.GetClient();

            ////导入站点不用PersistTo
            data.ForEach((it, index) =>
            {
                bucket.Store(StoreMode.Set, it.GetDocumentKey(), it.ToJson());
            });

        }

        public void ImportSchemaData(Models.Schema schema, IEnumerable<IDictionary<string, object>> data)
        {
            var ret = data.Select(it => new TextContent(it) { Repository = schema.Repository.GetBucketName() });
            if (ret.Count() > 0)
            {
                var bucket = schema.Repository.GetClient();

                //导入站点不用PersistTo
                ret.ForEach((it, index) =>
                {
                    bucket.Store(StoreMode.Add, it.UUID, it.ToJson());
                });
            }
        }
        #endregion

        #region QueryCategories
        public IEnumerable<Models.Category> QueryCategories(Models.TextContent content)
        {
            return content.GetRepository().QueryCategoriesBy(content.UUID).Select(it => it.ToCategory()).Where(it => it.ContentUUID.Equals(content.UUID));
        }
        #endregion

        #region CRUD
        public void Add(Models.TextContent content)
        {
            content.StoreFiles();

            ((IPersistable)content).OnSaving();
            var bucket = content.GetRepository().GetClient();

            //bucket.Store(StoreMode.Set, "Schema." + content.UUID, content.ToJson());
            bucket.ExecuteStore(StoreMode.Set, content.UUID, content.ToJson(), PersistTo.One);
            ((IPersistable)content).OnSaved();
        }

        public void Delete(Models.TextContent content)
        {
            var bucket = content.GetRepository().GetClient();

            bucket.ExecuteRemove(content.UUID, PersistTo.One);
            TextContentFileHelper.DeleteFiles(content);
        }

        public object Execute(IContentQuery<Models.TextContent> query)
        {
            Query.CouchbaseQueryTranslator translator = new Query.CouchbaseQueryTranslator();
            return translator.Translate(query).Execute();
        }

        public void Update(Models.TextContent @new, Models.TextContent old)
        {
            //if (@new["_id"] == null && old["_id"] != null)
            //{
            //    @new["_id"] = old["_id"];
            //}

            @new.StoreFiles();
            ((IPersistable)@new).OnSaving();
            var bucket = old.GetRepository().GetClient();

            bucket.ExecuteStore(StoreMode.Replace, old.UUID, @new.ToJson(), PersistTo.One);
            ((IPersistable)@new).OnSaved();
        }
        #endregion

        #region CreateTransaction/NotSupported
        public ITransactionUnit CreateTransaction(Models.Repository repository)
        {
            throw new NotSupportedException("Not supported for Couchbase");
        }
        #endregion

        #region ExecuteNonQuery/NotSupported
        public void ExecuteNonQuery(Models.Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Not supported for Couchbase");
        }
        #endregion

        #region ExecuteScalar/NotSupported

        public object ExecuteScalar(Models.Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Not supported for Couchbase");
        }
        #endregion

        #region ExecuteQuery/Used to query the data from view.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="queryText">The query text is the name of view. Composite of "DesignName/ViewName" </param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<IDictionary<string, object>> ExecuteQuery(Models.Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params KeyValuePair<string, object>[] parameters)
        {
            if (string.IsNullOrEmpty(queryText))
            {
                throw new Exception("Parameter:queryText is Null Or Empty.");
            }

            string[] paras = queryText.Split('/');
            if (paras.Length != 2)
            {
                throw new Exception("Parameter:queryText format is error.");
            }

            var client = repository.GetClient();
            var view = client.GetView(paras[0], paras[1]);

            List<Dictionary<string, object>> ret = new List<Dictionary<string, object>>();
            foreach (var row in view)
            {
                var dict = row.Info["value"] as Dictionary<string, object>;
                ret.Add(dict);
            }
            return ret;
        }
        #endregion
    }
}
