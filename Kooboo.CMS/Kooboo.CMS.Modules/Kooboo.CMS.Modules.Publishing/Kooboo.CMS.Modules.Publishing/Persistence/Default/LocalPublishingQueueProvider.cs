using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Modules.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ILocalPublishingQueueProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<LocalPublishingQueue>))]
    public class LocalPublishingQueueProvider : FileSystemProviderBase<LocalPublishingQueue>, ILocalPublishingQueueProvider
    {
        public override IEnumerable<LocalPublishingQueue> All()
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

        public IQueryable<LocalPublishingQueue> CreateQuery()
        {
            return this.All().AsQueryable();
        }

        public IQueryable<LocalPublishingQueue> CreateQuery(string siteName)
        {
            return this.All().Where(it => it.SiteName.Equals(siteName, StringComparison.OrdinalIgnoreCase)).AsQueryable();
        }

        public IEnumerable<LocalPublishingQueue> GetJobItems(DateTime utcExecutionTime, int maxItems)
        {
            return this.CreateQuery().Where(it => it.Status == QueueStatus.Pending)
                .Where(it => (it.UtcTimeToPublish == null && it.UtcTimeToUnpublish == null)
                    || (it.UtcTimeToPublish != null && it.UtcTimeToPublish.Value <= utcExecutionTime)
                    || (it.UtcTimeToUnpublish != null && it.UtcTimeToUnpublish.Value <= utcExecutionTime))
                    .OrderBy(it => it.UtcTimeToPublish)
                    .Take(maxItems);
        }
    }
}
