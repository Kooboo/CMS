using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence.Couchbase
{
    public class SiteInitializer
    {
        public virtual void Initialize(Site site)
        {
            var bucketName = site.GetBucketName();
            if (!DatabaseHelper.ExistBucket(bucketName))
            {
                DatabaseHelper.CreateBucket(bucketName);
                System.Threading.Thread.Sleep(3000);              
            }

        }
    }
}
