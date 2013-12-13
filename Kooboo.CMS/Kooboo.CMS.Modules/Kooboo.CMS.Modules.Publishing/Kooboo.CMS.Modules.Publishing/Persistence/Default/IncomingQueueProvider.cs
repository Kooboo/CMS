using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Modules.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IIncomingQueueProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<IncomingQueue>))]
    public class IncomingQueueProvider : FileSystemProviderBase<IncomingQueue>, IIncomingQueueProvider
    {
        public override IEnumerable<IncomingQueue> All()
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
        public IQueryable<IncomingQueue> CreateQuery()
        {
            return this.All().AsQueryable();
        }

        public IQueryable<IncomingQueue> CreateQuery(string siteName)
        {
            return this.All().Where(it => it.SiteName.Equals(siteName, StringComparison.OrdinalIgnoreCase)).AsQueryable();
        }
        #endregion

        public IEnumerable<IncomingQueue> GetJobItems(int maxItems)
        {
            return this.CreateQuery().Where(it => it.Status == QueueStatus.Pending)
               .OrderBy(it => it.UtcCreationDate).Take(maxItems);
        }
    }
}
