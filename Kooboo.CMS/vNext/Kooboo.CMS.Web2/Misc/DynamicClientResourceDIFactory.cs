using Kooboo.Common.Web.WebResourceLoader.DynamicClientResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web2.Misc
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(DynamicClientResourceFactory))]
    public class DynamicClientResourceDIFactory : DynamicClientResourceFactory
    {
        public override IEnumerable<IDynamicClientResource> ResolveAllProviders()
        {
            return Kooboo.Common.ObjectContainer.EngineContext.Current.ResolveAll<IDynamicClientResource>();
        }
    }
}