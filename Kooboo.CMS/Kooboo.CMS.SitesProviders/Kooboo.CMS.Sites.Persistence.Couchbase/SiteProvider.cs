using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Membership.Persistence;
using Kooboo.CMS.Sites.Globalization;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Sites.Persistence.Couchbase
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ISiteProvider), Order = 100)]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IProvider<Site>), Order = 100)]
    public class SiteProvider : Kooboo.CMS.Sites.Persistence.FileSystem.SiteProvider
    {
        #region .ctor
        SiteInitializer _initializer;
        public SiteProvider(IBaseDir baseDir, IMembershipProvider membershipProvider, ISiteExportableProvider[] exportableProivders, SiteInitializer initializer, RepositoryManager repositoryManager)
            : base(baseDir, membershipProvider, exportableProivders,repositoryManager)
        {
            //abSiteSettingProvider = new ABTestProvider.ABSiteSettingProvider();
            _initializer = initializer;
        }
        #endregion

        Func<Site, string, Site> createModel = (Site site, string key) =>
        {
            return new Site(key);
        };

        #region Get/Update/Save/Delete
        public override void Add(Site item)
        {
            try
            {
                _initializer.Initialize(item);
                InsertOrUpdate(item, item);
            }
            catch (Exception e)
            {
                //Maybe unable to save site setting when the data bucket is initializing. Ignore the save exception when site creating.
               Kooboo.Common.Logging.Logger.Error(e.Message, e);
            }

            base.Add(item);
        }
        public override void Update(Site @new, Site old)
        {
            InsertOrUpdate(@new, old);
            base.Update(@new, old);
        }
        private void InsertOrUpdate(Site @new, Site old)
        {
            ((IPersistable)@new).OnSaving();
            DataHelper.StoreObject(@new, @new.UUID, ModelExtensions.SiteDataType);
            ((IPersistable)@new).OnSaved();
        }
        public override Site Get(Site dummyObject)
        {
            var bucketDocumentKey = ModelExtensions.GetBucketDocumentKey(ModelExtensions.SiteDataType, dummyObject.FullName);

            var site = DataHelper.QueryByKey<Site>(dummyObject, bucketDocumentKey, createModel);

            if (site == null)
            {
                return base.Get(dummyObject);
            }
            return site;
        }
        public override void Remove(Site item)
        {
            DataHelper.DeleteItemByKey(item, ModelExtensions.GetBucketDocumentKey(ModelExtensions.SiteDataType, item.Name));
            base.Remove(item);
        }
      
        #endregion
    }
}
