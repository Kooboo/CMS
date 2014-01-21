using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence.Couchbase.CustomErrorProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ICustomErrorProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<CustomError>), Order = 100)]
    public class CustomErrorProvider : ICustomErrorProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);

        Func<Site, string, CustomError> createModel = (Site site, string key) =>
        {
            return new CustomError() { Site = site, UUID = key };
        };
        #region .ctor
        public CustomErrorProvider()
        { 
        }
        #endregion

        public void Export(Models.Site site, System.IO.Stream outputStream)
        {
            ExportCustomErrorToDisk(site);

            var provider = new Kooboo.CMS.Sites.Persistence.FileSystem.CustomErrorProvider();
            provider.Export(site, outputStream);
        }

        public void Import(Models.Site site, System.IO.Stream zipStream, bool @override)
        {
            var provider = new Kooboo.CMS.Sites.Persistence.FileSystem.CustomErrorProvider();
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

        public IEnumerable<Models.CustomError> All(Models.Site site)
        {
            return DataHelper.QueryList<CustomError>(site, ModelExtensions.GetQueryView(ModelExtensions.CustomErrorDataType), createModel);
        }

        public IEnumerable<Models.CustomError> All()
        {
            throw new NotImplementedException();
        }

        public Models.CustomError Get(Models.CustomError dummy)
        {
            var bucketDocumentKey = ModelExtensions.GetBucketDocumentKey(ModelExtensions.CustomErrorDataType, dummy.UUID);

            return DataHelper.QueryByKey<CustomError>(dummy.Site, bucketDocumentKey, createModel);
        }

        public void Add(Models.CustomError item)
        {
            InsertOrUpdate(item, item);
        }
        private void InsertOrUpdate(CustomError @new, CustomError old)
        {
            ((IPersistable)@new).OnSaving();

            DataHelper.StoreObject(@new, @new.UUID, ModelExtensions.CustomErrorDataType);

            ((IPersistable)@new).OnSaved();
        }
        public void Update(Models.CustomError @new, Models.CustomError old)
        {
            InsertOrUpdate(@new, @old);
        }

        public void Remove(Models.CustomError item)
        {
            DataHelper.DeleteItemByKey(item.Site, ModelExtensions.GetBucketDocumentKey(ModelExtensions.CustomErrorDataType, item.UUID));
        }

        public void InitializeCustomError(Site site)
        {
            ICustomErrorProvider fileProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.CustomErrorProvider();
            foreach (var item in fileProvider.All(site))
            {
                if (item.Site == site)
                {
                    this.Add(fileProvider.Get(item));
                }
            }
        }

        public void ExportCustomErrorToDisk(Site site)
        {
            var allItem = this.All(site).ToList();
            var provider = new Kooboo.CMS.Sites.Persistence.FileSystem.CustomErrorProvider();
            var file = new CustomErrorsFile(site).PhysicalPath;
            locker.EnterWriteLock();
            try
            {
                Serialization.Serialize<List<CustomError>>(allItem, file);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
    }
}
