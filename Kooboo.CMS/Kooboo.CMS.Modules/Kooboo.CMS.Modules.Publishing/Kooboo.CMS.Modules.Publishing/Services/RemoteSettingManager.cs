using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Services
{
    public class RemoteSettingManager : ManagerBase<Kooboo.CMS.Modules.Publishing.Models.RemoteEndpointSetting>
    {
        #region .ctor
        IRemoteEndpointSettingProvider _remoteSettingProvider;
        public RemoteSettingManager(IRemoteEndpointSettingProvider remoteSettingProvider)
            : base(remoteSettingProvider)
        {
            this._remoteSettingProvider = remoteSettingProvider;
        } 
        #endregion

        #region Get
        public virtual RemoteEndpointSetting Get(string uuid)
        {
            return new RemoteEndpointSetting(uuid).AsActual();
        } 
        #endregion

        #region Delete
        public virtual void Delete(string[] uuids)
        {
            foreach (string uuid in uuids)
            {
                var model = new RemoteEndpointSetting(uuid).AsActual();
                this._remoteSettingProvider.Remove(model);
            }
        } 
        #endregion        
    }
}
