#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Persistence.Couchbase.UserProvider
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IUserProvider), Order = 100)]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IProvider<User>), Order = 100)]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ISiteExportableProvider), Order = 100, Key = "UserProvider")]
    public class UserProvider : IUserProvider
    {
        Func<Site, string, User> createModel = (Site site, string key) =>
        {
            return new User() { Site = site, UUID = key };
        };
        #region .ctor
        public UserProvider()
        {

        }
        #endregion

        #region All
        public IEnumerable<User> All(Site site)
        {
            return DataHelper.QueryList<User>(site, ModelExtensions.GetQueryViewName(ModelExtensions.UserDataType), createModel);
        }
        #endregion

        #region Get
        public User Get(Models.User dummy)
        {
            var bucketDocumentKey = ModelExtensions.GetBucketDocumentKey(ModelExtensions.UserDataType, dummy.UUID);

            return DataHelper.QueryByKey<User>(dummy.Site,bucketDocumentKey, createModel);
        }
        #endregion

        #region Add/Update
        public void Add(Models.User item)
        {
            InsertOrUpdate(item, item);
        }
        private void InsertOrUpdate(User @new, User old)
        {
            ((IPersistable)@new).OnSaving();

            DataHelper.StoreObject(@new, @new.UUID, ModelExtensions.UserDataType);

            ((IPersistable)@new).OnSaved();
        }

        public void Update(Models.User @new, Models.User old)
        {
            InsertOrUpdate(@new, old);
        }
        #endregion

        #region Remove
        public void Remove(Models.User item)
        {
            DataHelper.DeleteItemByKey(item.Site, ModelExtensions.GetBucketDocumentKey(ModelExtensions.UserDataType, item.UUID));
        }
        #endregion

        #region All
        public IEnumerable<Models.User> All()
        {
            throw new NotSupportedException();
        }
        #endregion


        #region ISiteElementProvider InitializeToDB/ExportToDisk
        public void InitializeToDB(Site site)
        {
            //not need to implement.
        }

        public void ExportToDisk(Site site)
        {
            //not need to implement.
        }
        #endregion
    }
}
