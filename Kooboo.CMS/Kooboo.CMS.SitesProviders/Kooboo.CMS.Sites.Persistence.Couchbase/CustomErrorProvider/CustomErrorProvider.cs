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
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISiteExportableProvider), Order = 100, Key = "CustomErrorProvider")]
    public class CustomErrorProvider : ProviderBase<CustomError>, ICustomErrorProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);

        //Func<Site, string, CustomError> createModel = (Site site, string key) =>
        //{
        //    return new CustomError() { Site = site, UUID = key };
        //};
        #region .ctor
        public CustomErrorProvider()
            : base(ModelExtensions.CustomErrorDataType, (Site site, string key) =>
            {
                return new CustomError() { Site = site, UUID = key };
            })
        {
        }
        #endregion

        #region Export
        public void Export(Models.Site site, IEnumerable<CustomError> customErrors, System.IO.Stream outputStream)
        {
            ExportToDisk(site);

            var provider = new Kooboo.CMS.Sites.Persistence.FileSystem.CustomErrorProvider();
            provider.Export(site, customErrors, outputStream);
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


        public void InitializeToDB(Site site)
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

        public void ExportToDisk(Site site)
        {
            var allItems = this.All(site).ToList();
            var fileProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.CustomErrorProvider();
            foreach (var item in allItems)
            {
                fileProvider.Add(item);
            }
        }
        #endregion
    }
}
