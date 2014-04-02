﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Membership.Persistence;

namespace Kooboo.CMS.Sites.Persistence.Couchbase.ABTestProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IABSiteSettingProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<ABSiteSetting>), Order = 100)]
    public class ABSiteSettingProvider : IABSiteSettingProvider
    {
        #region .ctor
        Kooboo.CMS.Sites.Persistence.FileSystem.ABSiteSettingProvider fileProvider;

        Func<Site, string, ABSiteSetting> createModel = (Site site, string key) =>
        {
            return new ABSiteSetting() { MainSite = key };
        };
        public ABSiteSettingProvider(IBaseDir baseDir)
        {
            fileProvider = new FileSystem.ABSiteSettingProvider(baseDir);
        } 
        #endregion

        #region CURD
        public IEnumerable<ABSiteSetting> All()
        {
            return DataHelper.QueryList<ABSiteSetting>(new Site(), ModelExtensions.GetQueryViewName(ModelExtensions.ABSiteSettingDataType), createModel);
        }

        public ABSiteSetting Get(ABSiteSetting dummy)
        {
            var bucketDocumentKey = ModelExtensions.GetBucketDocumentKey(ModelExtensions.ABSiteSettingDataType, dummy.UUID);

            return DataHelper.QueryByKey<ABSiteSetting>(new Site(), bucketDocumentKey, createModel);
        }

        public void Add(ABSiteSetting item)
        {
            InsertOrUpdate(item, item);
        }
        private void InsertOrUpdate(ABSiteSetting @new, ABSiteSetting old)
        {
            ((IPersistable)@new).OnSaving();
            DataHelper.StoreObject(new Site(), @new, @new.UUID, ModelExtensions.ABSiteSettingDataType);
            ((IPersistable)@new).OnSaved();
        }
        public void Update(ABSiteSetting @new, ABSiteSetting old)
        {
            InsertOrUpdate(@new, old);
        }

        public void Remove(ABSiteSetting item)
        {
            DataHelper.DeleteItemByKey(new Site(), ModelExtensions.GetBucketDocumentKey(ModelExtensions.ABSiteSettingDataType, item.UUID));
        }

        #endregion

        #region Export
        public void Export(IEnumerable<ABSiteSetting> sources, System.IO.Stream outputStream)
        {
            ExportABSiteSettingToDisk();
            fileProvider.Export(sources, outputStream);
        }

        public void Import(System.IO.Stream zipStream, bool @override)
        {
            var allItem = fileProvider.All();
            foreach (var item in allItem)
            {
                fileProvider.Remove(item);
            }
            fileProvider.Import(zipStream, @override);
            allItem = fileProvider.All();
            if (!@override)
            {
                allItem = allItem.Where(it => null == Get(it));
            }
            var dummy = allItem.ToList();
            foreach (var item in dummy)
            {
                var tempItem = fileProvider.Get(item);
                InsertOrUpdate(tempItem, tempItem);
            }
        }

        public void InitializeABSiteSetting()
        {
            foreach (var item in fileProvider.All())
            {
                this.Add(fileProvider.Get(item));
            }
        }

        public void ExportABSiteSettingToDisk()
        {
            var fileAll = fileProvider.All();
            foreach (var item in fileAll)
            {
                fileProvider.Remove(item);
            }

            var allItem = this.All().ToList();
            foreach (var item in allItem)
            {
                fileProvider.Add(item.AsActual());
            }
        } 
        #endregion
    }
}
