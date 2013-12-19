using Kooboo.Web.Mvc.WebResourceLoader.DynamicClientResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Misc
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(DynamicClientResourceFactory))]
    public class DynamicClientResourceDIFactory : DynamicClientResourceFactory
    {
        public override IEnumerable<IDynamicClientResource> ResolveAllProviders()
        {
            return Kooboo.CMS.Common.Runtime.EngineContext.Current.ResolveAll<IDynamicClientResource>();
        }
    }
}