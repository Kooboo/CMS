using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Content.Persistence.Default;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime;
using Enyim.Caching.Memcached;
using Kooboo.CMS.Common;
using Couchbase.Management;

namespace Kooboo.CMS.Content.Persistence.Couchbase
{
    public class RepositoryProvider : Default.RepositoryProvider
    {
        public RepositoryProvider(IBaseDir baseDir)
            : base(baseDir) { }
        public override void Add(Repository item)
        {
            base.Add(item);
            Initialize(item);
        }
        public override void Initialize(Models.Repository repository)
        {
            //create bucket
            var bucketName = repository.GetBucketName();
            if (!DatabaseHelper.ExistBucket(bucketName))
            {
                var cf = DatabaseHelper.GetCouchbaseClientConfiguration();

                Bucket bucket = new Bucket();
                bucket.Name = bucketName;
                bucket.AuthType = AuthTypes.Sasl;
                bucket.BucketType = BucketTypes.Membase;
                bucket.Quota = new Quota() { RAM = DatabaseSettings.Instance.BucketRAM };//RamQuotaMB must be at least 100
                bucket.FlushOption = FlushOptions.Enabled;//支持清空
                bucket.ReplicaNumber = (ReplicaNumbers)DatabaseSettings.Instance.ReplicaNumber;
                bucket.ReplicaIndex = DatabaseSettings.Instance.ReplicaIndex;
                DatabaseHelper.CreateBucket(bucket);

                //此处需暂停几秒钟，否则，通过选择模板创建站点的方式，在导入数据时，会出现数据未导入的情况
                //大致原因在于，Couchbae在数据库创建之后，需要几秒钟的初始化过程，在这个过程中插入任何数据都将失败
                System.Threading.Thread.Sleep(3000);
                //create category views
                repository.CreateDefaultViews();
            }
            base.Initialize(repository);
        }

        public override void Remove(Models.Repository item)
        {
            base.Remove(item);
            item.DeleteBucket();
        }

        public override bool TestDbConnection()
        {
            return true;
            //var client = DatabaseHelper.GetClient();
            //var flag = client.ExecuteStore(StoreMode.Set, "Test_DbConnection", ".");
            //return flag.Success;
        }
    }
}
