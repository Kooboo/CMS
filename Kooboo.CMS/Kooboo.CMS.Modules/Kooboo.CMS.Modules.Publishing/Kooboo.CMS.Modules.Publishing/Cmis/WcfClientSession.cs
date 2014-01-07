using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Cmis
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ICmisSession))]
    public class WcfClientSession : ICmisSession
    {
        public ICmisService OpenSession(string endpointUrl, string userName, string password)
        {
            //return null;
            return new CmisWcfClient(endpointUrl, userName, password);
        }
    }
}
