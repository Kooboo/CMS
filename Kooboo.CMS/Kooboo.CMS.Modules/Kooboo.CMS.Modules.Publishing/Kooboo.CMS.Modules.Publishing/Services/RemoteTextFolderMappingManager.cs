using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Services
{
   public class RemoteTextFolderMappingManager:ManagerBase<Kooboo.CMS.Modules.Publishing.Models.RemoteTextFolderMapping>
    {
        IRemoteTextFolderMappingProvider _remotePublishingMappingProvider;
        public RemoteTextFolderMappingManager(IRemoteTextFolderMappingProvider remotePublishingMappingProvider)
            : base(remotePublishingMappingProvider)
        {
            this._remotePublishingMappingProvider = remotePublishingMappingProvider;
        }

        public virtual RemoteTextFolderMapping Get(string uuid)
        {
            return new RemoteTextFolderMapping(uuid).AsActual();
        }

        public virtual void Delete(string[] uuids)
        {
            foreach (string uuid in uuids)
            {
                var model = new RemoteTextFolderMapping(uuid).AsActual();
                this._remotePublishingMappingProvider.Remove(model);
            }
        }
    }
}
