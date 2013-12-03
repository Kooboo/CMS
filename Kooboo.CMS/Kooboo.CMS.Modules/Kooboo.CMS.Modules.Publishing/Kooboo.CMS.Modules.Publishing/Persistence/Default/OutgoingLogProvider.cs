using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Persistence.Default;
using Kooboo.CMS.Modules.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IPublishingLogProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<PublishingLog>))]
    public class OutgoingLogProvider : FileSystemProviderBase<PublishingLog>,IPublishingLogProvider
    {
        public override IEnumerable<PublishingLog> All()
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

        public IQueryable<PublishingLog> CreateQuery()
        {
            return this.All().AsQueryable();
        }

        public IQueryable<PublishingLog> CreateQuery(string siteName)
        {
            return this.All().Where(it => it.SiteName.Equals(siteName, StringComparison.OrdinalIgnoreCase)).AsQueryable();
        }
    }
}
