using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Persistence;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Services
{
    public class RemoteTextFolderMappingManager : ManagerBase<Kooboo.CMS.Modules.Publishing.Models.RemoteTextFolderMapping>
    {
        IRemoteTextFolderMappingProvider _remotePublishingMappingProvider;
        public RemoteTextFolderMappingManager(IRemoteTextFolderMappingProvider remotePublishingMappingProvider)
            : base(remotePublishingMappingProvider)
        {
            this._remotePublishingMappingProvider = remotePublishingMappingProvider;
        }

        public virtual RemoteTextFolderMapping Get(Site site, string uuid)
        {
            return new RemoteTextFolderMapping(site, uuid).AsActual();
        }

        public virtual void Delete(Site site, string[] uuids)
        {
            foreach (string uuid in uuids)
            {
                var model = new RemoteTextFolderMapping(site,uuid).AsActual();
                if (model!=null)
                {
                    this._remotePublishingMappingProvider.Remove(model);
                }                
            }
        }
    }
}
