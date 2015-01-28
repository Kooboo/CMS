using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Modules.Publishing.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IRemotePublishingQueueProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<RemotePublishingQueue>))]
    public class RemotePublishingQueueProvider : FileSystemProviderBase<RemotePublishingQueue>, IRemotePublishingQueueProvider
    {
         #region .ctor
        public RemotePublishingQueueProvider(Kooboo.CMS.Sites.Persistence.ISiteProvider siteProvider)
            : base(siteProvider)
        {}
        #endregion    
        #region GetLocker
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion
        protected override string GetBasePath(Sites.Models.Site site)
        {
            return Path.Combine(site.PhysicalPath, PublishingPath.PublishingFolderName, "RemotePublishingQueue");
        }

        public IEnumerable<RemotePublishingQueue> GetJobItems(DateTime utcExecutionTime, int maxItems)
        {
            return this.All().Where(it => it.Status == QueueStatus.Pending)
                .Where(it => (it.UtcTimeToPublish != null && it.UtcTimeToPublish.Value <= utcExecutionTime)
                    || (it.UtcTimeToUnpublish != null && it.UtcTimeToUnpublish.Value <= utcExecutionTime))
                    .OrderBy(it => it.RetryTimes)
                    .Take(maxItems);
        }
    }
}
