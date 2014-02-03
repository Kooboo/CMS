using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Membership.Persistence;
using Kooboo.CMS.Sites.Globalization;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence.Couchbase
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISiteProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Site>), Order = 100)]
    public class SiteProvider : Kooboo.CMS.Sites.Persistence.FileSystem.SiteProvider
    {
        #region .ctor
        SiteInitializer _initializer;
        CustomErrorProvider.CustomErrorProvider customErrorProvider;
        UrlRedirectProvider.UrlRedirectProvider urlRedirectProvider;
        ABTestProvider.ABPageSettingProvider abPageSettingProvider;
        ABTestProvider.ABRuleSettingProvider abRuleSettingProvider;
        //ABTestProvider.ABSiteSettingProvider abSiteSettingProvider;
        public SiteProvider(IBaseDir baseDir, IMembershipProvider membershipProvider, IElementRepositoryFactory elementRepositoryFactory, SiteInitializer initializer)
            : base(baseDir, membershipProvider, elementRepositoryFactory)
        {
            customErrorProvider = new CustomErrorProvider.CustomErrorProvider();
            urlRedirectProvider = new UrlRedirectProvider.UrlRedirectProvider();
            abPageSettingProvider = new ABTestProvider.ABPageSettingProvider();
            abRuleSettingProvider = new ABTestProvider.ABRuleSettingProvider();
            //abSiteSettingProvider = new ABTestProvider.ABSiteSettingProvider();
            _initializer = initializer;
        }
        public override void Initialize(Site site)
        {
            _initializer.Initialize(site);
            base.Initialize(site);

            customErrorProvider.InitializeCustomError(site);
            urlRedirectProvider.InitializeUrlRedirect(site, true);
            abPageSettingProvider.InitializeABPageSetting(site);
            abRuleSettingProvider.InitializeABRuleSetting(site);
            //abSiteSettingProvider.InitializeABSiteSetting();
        }

        static SiteProvider()
        {
            //Create Kooboo_CMS global bucket
            var defaultBucket = DatabaseSettings.Instance.DefaultBucketName;
            if (!DatabaseHelper.ExistBucket(defaultBucket))
            {
                DatabaseHelper.CreateBucket(defaultBucket);
                System.Threading.Thread.Sleep(3000);     
            }
            DatabaseHelper.CreateDesignDocument(defaultBucket, ModelExtensions.GetQueryView(ModelExtensions.ABRuleSettingDataType), string.Format(DataHelper.viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.ABRuleSettingDataType), ModelExtensions.ABRuleSettingDataType));
            DatabaseHelper.CreateDesignDocument(defaultBucket, ModelExtensions.GetQueryView(ModelExtensions.ABSiteSettingDataType), string.Format(DataHelper.viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.ABSiteSettingDataType), ModelExtensions.ABSiteSettingDataType));
            System.Threading.Thread.Sleep(3000);
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
                Kooboo.HealthMonitoring.Log.LogException(e);
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

        public override void Export(Site site, System.IO.Stream outputStream, bool includeDatabase, bool includeSubSites)
        {
            ExportDataToDisk(site, includeSubSites);
            base.Export(site, outputStream, includeDatabase, includeSubSites);
        }
        private void ExportDataToDisk(Site site,bool includeSubSites)
        {
            customErrorProvider.ExportCustomErrorToDisk(site);
            urlRedirectProvider.ExportUrlRedirectToDisk(site);
            abPageSettingProvider.ExportABPageSettingToDisk(site);
            abRuleSettingProvider.ExportABRuleSettingToDisk(site);
            //abSiteSettingProvider.ExportABSiteSettingToDisk();
            if(includeSubSites)
            {
                var subSites=ChildSites(site);
                foreach(var s in subSites)
                {
                    ExportDataToDisk(s, includeSubSites);
                }
            }
        }
        #endregion
    }
}
