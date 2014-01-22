using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence.Couchbase
{
    public class ProviderBase<T>
        where T : ISiteObject, IIdentifiable, IPersistable
    {
        protected string DataType;
        protected Func<Site, string, T> CreateModel;

        public ProviderBase(string dataType, Func<Site, string, T> createModel)
        {
            DataType = dataType;
            CreateModel = createModel;
        }

        public virtual IEnumerable<T> All(Models.Site site)
        {
            return DataHelper.QueryList<T>(site, ModelExtensions.GetQueryView(DataType), CreateModel);
        }

        public virtual IEnumerable<T> All()
        {
            throw new NotImplementedException();
        }

        public virtual T Get(T dummy)
        {
            var bucketDocumentKey = ModelExtensions.GetBucketDocumentKey(DataType, dummy.UUID);
            if (dummy.Site == null)
            {
                dummy.Site = Site.Current;
            }
            return DataHelper.QueryByKey<T>(dummy.Site, bucketDocumentKey, CreateModel);
        }

        public virtual void Add(T item)
        {
            InsertOrUpdate(item, item);
        }
        public void InsertOrUpdate(T @new, T old)
        {
            ((IPersistable)@new).OnSaving();
            DataHelper.StoreObject(@new, @new.UUID, DataType);
            ((IPersistable)@new).OnSaved();
        }
        public virtual void Update(T @new, T old)
        {
            InsertOrUpdate(@new, old);
        }

        public virtual void Remove(T item)
        {
            DataHelper.DeleteItemByKey(item.Site, ModelExtensions.GetBucketDocumentKey(DataType, item.UUID));
        }
    }
}
