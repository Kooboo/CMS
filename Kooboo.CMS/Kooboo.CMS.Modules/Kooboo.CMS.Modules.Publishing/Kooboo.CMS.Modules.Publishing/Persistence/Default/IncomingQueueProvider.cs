using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Modules.Publishing.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IIncomingQueueProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<IncomingQueue>))]
    public class IncomingQueueProvider : FileSystemProviderBase<IncomingQueue>, IIncomingQueueProvider
    {
        #region .ctor
        public IncomingQueueProvider(Kooboo.CMS.Sites.Persistence.ISiteProvider siteProvider)
            : base(siteProvider)
        { }
        #endregion

        #region GetLocker
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion

        public IEnumerable<IncomingQueue> GetJobItems(int maxItems)
        {
            return this.All().Where(it => it.Status == QueueStatus.Pending)
               .OrderBy(it => it.UtcCreationDate).Take(maxItems);
        }

        protected override string GetBasePath(Sites.Models.Site site)
        {
            return Path.Combine(site.PhysicalPath, PublishingPath.PublishingFolderName, "IncomingQueue");
        }
    }
}
