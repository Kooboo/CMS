using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence.Couchbase.UrlRedirectProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IUrlRedirectProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<UrlRedirect>), Order = 100)]
    public class UrlRedirectProvider :ProviderBase<UrlRedirect>, IUrlRedirectProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);

        //Func<Site, string, UrlRedirect> createModel = (Site site, string key) =>
        //{
        //    return new UrlRedirect() { Site = site, UUID = key };
        //};
        #region .ctor
        public UrlRedirectProvider()
            : base(ModelExtensions.UrlRedirectDataType, (Site site, string key) =>
            {
                return new UrlRedirect() { Site = site, UUID = key };
            })
        {
        }
        #endregion
        public void Export(Site site, System.IO.Stream outputStream)
        {
            ExportUrlRedirectToDisk(site);

            var provider = new Kooboo.CMS.Sites.Persistence.FileSystem.UrlRedirectProvider();
            provider.Export(site, outputStream);
        }

        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            var provider = new Kooboo.CMS.Sites.Persistence.FileSystem.UrlRedirectProvider();
            provider.Import(site, zipStream, @override);
            var allItem = provider.All(site);
            if (!@override)
            {
                allItem = allItem.Where(it => null == Get(it));
            }
            var dummy = allItem.ToList();
            foreach (var item in dummy)
            {
                InsertOrUpdate(item, item);
            }
        }

        //public IEnumerable<UrlRedirect> All(Site site)
        //{
        //    return DataHelper.QueryList<UrlRedirect>(site, ModelExtensions.GetQueryView(ModelExtensions.UrlRedirectDataType), createModel);
        //}

        //public IEnumerable<UrlRedirect> All()
        //{
        //    throw new NotImplementedException();
        //}

        //public UrlRedirect Get(UrlRedirect dummy)
        //{
        //    var bucketDocumentKey = ModelExtensions.GetBucketDocumentKey(ModelExtensions.UrlRedirectDataType, dummy.UUID);

        //    return DataHelper.QueryByKey<UrlRedirect>(dummy.Site, bucketDocumentKey, createModel);
        //}

        //public void Add(UrlRedirect item)
        //{
        //    InsertOrUpdate(item, item);
        //}
        //private void InsertOrUpdate(UrlRedirect @new, UrlRedirect old)
        //{
        //    ((IPersistable)@new).OnSaving();

        //    DataHelper.StoreObject(@new, @new.UUID, ModelExtensions.UrlRedirectDataType);

        //    ((IPersistable)@new).OnSaved();
        //}
        //public void Update(UrlRedirect @new, UrlRedirect old)
        //{
        //    InsertOrUpdate(@new, old);
        //}

        //public void Remove(UrlRedirect item)
        //{
        //    DataHelper.DeleteItemByKey(item.Site, ModelExtensions.GetBucketDocumentKey(ModelExtensions.UrlRedirectDataType, item.UUID));
        //}

        public void InitializeUrlRedirect(Site site, bool regenUUID = false)
        {
            IUrlRedirectProvider fileProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.UrlRedirectProvider();
            foreach (var item in fileProvider.All(site))
            {
                if (item.Site == site)
                {
                    var urlRedirect = fileProvider.Get(item);
                    if (regenUUID)
                    {
                        urlRedirect.UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(8);
                    }
                    this.Add(urlRedirect);
                }
            }
        }

        public void ExportUrlRedirectToDisk(Site site)
        {
            var allItem = this.All(site).ToList();
            var provider = new Kooboo.CMS.Sites.Persistence.FileSystem.UrlRedirectProvider();
            var file = new UrlRedirectsFile(site).PhysicalPath;
            locker.EnterWriteLock();
            try
            {
                Serialization.Serialize<List<UrlRedirect>>(allItem, file);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
    }
}
