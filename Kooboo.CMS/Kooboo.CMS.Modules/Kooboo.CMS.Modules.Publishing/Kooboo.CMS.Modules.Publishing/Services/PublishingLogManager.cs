using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Services
{
     public class PublishingLogManager : ManagerBase<Kooboo.CMS.Modules.Publishing.Models.PublishingLog>
    {
        #region .ctor
        IPublishingLogProvider _outgoingLogProvider;
        public PublishingLogManager(IPublishingLogProvider outgoingLogProvider)
            : base(outgoingLogProvider)
        {
            this._outgoingLogProvider = outgoingLogProvider;
        } 
        #endregion

        #region Get
        public virtual PublishingLog Get(string uuid)
        {
            return new PublishingLog(uuid).AsActual();
        } 
        #endregion

        #region Delete
        public virtual void Delete(string[] uuids)
        {
            foreach (string uuid in uuids)
            {
                var model = new PublishingLog(uuid).AsActual();
                this._outgoingLogProvider.Remove(model);
            }
        } 
        #endregion        
    }
}
