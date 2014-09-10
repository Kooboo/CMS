using Kooboo.Globalization;
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
        IRemoteTextFolderMappingProvider _textFolderMappingProvider;
        public RemoteSettingManager(IRemoteEndpointSettingProvider remoteSettingProvider, IRemoteTextFolderMappingProvider textFolderMappingProvider)
            : base(remoteSettingProvider)
        {
            this._remoteSettingProvider = remoteSettingProvider;
            this._textFolderMappingProvider = textFolderMappingProvider;
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
                if (model != null)
                {
                    if (Relations(model).Count() > 0)
                    {
                        throw new Exception(string.Format("'{0}' is being used".Localize(), uuid));
                    }
                    this._remoteSettingProvider.Remove(model);
                }
            }
        }

        public virtual IEnumerable<RelationModel> Relations(RemoteEndpointSetting remoteEndpointSetting)
        {
            var allTextFolderMappings = _textFolderMappingProvider.CreateQuery(remoteEndpointSetting.SiteName);

            return allTextFolderMappings.Select(it => it.AsActual()).Where(it => it.RemoteEndpoint.EqualsOrNullEmpty(remoteEndpointSetting.UUID, StringComparison.OrdinalIgnoreCase))
                   .Select(it => new RelationModel()
                   {
                       ObjectUUID = it.UUID,
                       RelationObject = it.RemoteEndpoint,
                       RelationType = "Text folder mapping"
                   });
        }

        #endregion
    }
}
