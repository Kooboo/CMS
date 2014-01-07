using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Modules.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IOutgoingQueueProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<OutgoingQueue>))]
    public class OutgoingQueueProvider : FileSystemProviderBase<OutgoingQueue>, IOutgoingQueueProvider
    {
        public override IEnumerable<OutgoingQueue> All()
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

        #region CreateQuery
        public IQueryable<OutgoingQueue> CreateQuery()
        {
            return this.All().AsQueryable();
        }

        public IQueryable<OutgoingQueue> CreateQuery(string siteName)
        {
            return this.All().Where(it => it.SiteName.Equals(siteName, StringComparison.OrdinalIgnoreCase)).AsQueryable();
        }
        #endregion

        #region GetJobItems
        public IEnumerable<OutgoingQueue> GetJobItems(int maxItems)
        {
            return this.CreateQuery().Where(it => it.Status == QueueStatus.Pending)
                .OrderBy(it => it.RetryTimes).Take(maxItems);
        }
        #endregion
    }
}
