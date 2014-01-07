using Couchbase.Management;
using Kooboo.CMS.Content.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Content.Persistence.Couchbase
{
    public class SchemaProvider : Kooboo.CMS.Content.Persistence.Default.SchemaProvider
    {
        public override void Initialize(Schema schema)
        {
            base.Initialize(schema);
            //var cf = DatabaseHelper.GetCouchbaseClientConfiguration();
            //if (!DatabaseHelper.ExistBucket(cf.Bucket))
            //{
            //    Bucket bucket = new Bucket();
            //    bucket.Name = cf.Bucket;
            //    bucket.Password = cf.BucketPassword;
            //    bucket.AuthType = AuthTypes.Sasl;
            //    bucket.BucketType = BucketTypes.Membase;
            //    bucket.Quota = new Quota() { RAM = 100 };
            //    bucket.FlushOption = FlushOptions.Enabled;//支持清空

            //    DatabaseHelper.CreateBucket(bucket);
            //}
            //schema.Repository.CreateSchemaViews(schema);
        }
        public override void Remove(Schema item)
        {
            base.Remove(item);
            //item.DropSchemas();
            //item.Repository.DeleteSchemaViews(item);
        }
    }
}
