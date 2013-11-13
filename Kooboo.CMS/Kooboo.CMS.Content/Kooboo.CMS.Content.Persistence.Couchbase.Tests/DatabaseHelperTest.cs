using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.Content.Persistence.Couchbase;
using Couchbase.Management;
using Couchbase.Configuration;
using Kooboo.CMS.Content.Models;
using System.Collections.Generic;

namespace Kooboo.CMS.Content.Persistence.Couchbase.Tests
{
    [TestClass]
    public class DatabaseHelperTest
    {
        private Repository repository = null;
        private ICouchbaseClientConfiguration cf = null;
        public DatabaseHelperTest()
        {
            cf = DatabaseHelper.GetCouchbaseClientConfiguration();
            if (!DatabaseHelper.ExistBucket(cf.Bucket))
            {
                CreateBucket();
            }
            repository = new Repository(cf.Bucket);
            repository.CreateDefaultViews();
        }

        private void CreateBucket()
        {
            Bucket bucket = new Bucket();
            bucket.Name = cf.Bucket;
            bucket.Password = cf.BucketPassword;
            bucket.AuthType = AuthTypes.Sasl;
            bucket.BucketType = BucketTypes.Membase;
            bucket.Quota = new Quota() { RAM = 100 };
            bucket.FlushOption = FlushOptions.Enabled;//支持清空

            DatabaseHelper.CreateBucket(bucket);
        }

        [TestMethod]
        public void Test_DeleteBucket_And_CreateBucket()
        {
            repository.DeleteBucket();
            if (!DatabaseHelper.ExistBucket(cf.Bucket))
            {
                CreateBucket();
                repository.CreateDefaultViews();
            }
            Assert.AreEqual(true, DatabaseHelper.ExistBucket(repository.GetBucketName()));
        }

        [TestMethod]
        public void Test_ExistBucket()
        {
            var cf = DatabaseHelper.GetCouchbaseClientConfiguration();
            DatabaseHelper.ExistBucket(cf.Bucket);
        }

        [TestMethod]
        public void Test_GetClient()
        {
            var client = repository.GetClient();
            Assert.IsNotNull(client);
        }

        [TestMethod]
        public void Test_CreateCategoryViews()
        {
            var flag = repository.CreateDefaultViews();
            Assert.AreEqual(true, flag);
        }
    }
}
