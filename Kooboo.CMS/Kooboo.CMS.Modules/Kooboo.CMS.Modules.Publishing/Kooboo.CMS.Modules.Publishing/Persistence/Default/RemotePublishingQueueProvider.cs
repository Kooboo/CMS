using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Modules.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IRemotePublishingQueueProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<RemotePublishingQueue>))]
    public class RemotePublishingQueueProvider : FileSystemProviderBase<RemotePublishingQueue>, IRemotePublishingQueueProvider
    {
        public override IEnumerable<RemotePublishingQueue> All()
        {
            return base.All();
        }

        #region GetLocker
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion

        public IQueryable<RemotePublishingQueue> CreateQuery()
        {
            return this.All().AsQueryable();
        }

        public IQueryable<RemotePublishingQueue> CreateQuery(string siteName)
        {
            return this.All().Where(it => it.SiteName.Equals(siteName, StringComparison.OrdinalIgnoreCase)).AsQueryable();
        }

        public IEnumerable<RemotePublishingQueue> GetJobItems(DateTime utcExecutionTime, int maxItems)
        {
            return this.CreateQuery().Where(it => it.Status == QueueStatus.Pending)
                .Where(it => (it.UtcTimeToPublish != null && it.UtcTimeToPublish.Value <= utcExecutionTime)
                    || (it.UtcTimeToUnpublish != null && it.UtcTimeToUnpublish.Value <= utcExecutionTime))
                    .OrderBy(it => it.RetryTimes)
                    .Take(maxItems);
        }
    }
}
