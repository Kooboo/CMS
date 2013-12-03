using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Modules.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IRemoteEndpointSettingProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<RemoteEndpointSetting>))]
    public class RemoteEndpointSettingProvider:FileSystemProviderBase<RemoteEndpointSetting>,IRemoteEndpointSettingProvider
    {
        public override IEnumerable<RemoteEndpointSetting> All()
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


        public IQueryable<RemoteEndpointSetting> CreateQuery()
        {
            return this.All().AsQueryable();
        }

        public IQueryable<RemoteEndpointSetting> CreateQuery(string siteName)
        {
            return this.All().Where(it => it.SiteName.Equals(siteName, StringComparison.OrdinalIgnoreCase)).AsQueryable();
        }
    }
}
