using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Content.Persistence.Default;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime;
using Couchbase;

namespace Kooboo.CMS.Content.Persistence.Couchbase
{
    public class ProviderFactory : Default.ProviderFactory, Kooboo.CMS.Common.Runtime.Dependency.IDependencyRegistrar
    {
        public override string Name
        {
            get
            {
                return "Couchbase";
            }
        }

        public int Order
        {
            get
            {
                return 2;
            }
        }

        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.AddComponent<IProviderFactory, ProviderFactory>();
            containerManager.AddComponent<IRepositoryProvider, RepositoryProvider>();
            containerManager.AddComponent<ISchemaProvider, SchemaProvider>();
            containerManager.AddComponent<IContentProvider<TextContent>, TextContentProvider>();
            containerManager.AddComponent<ITextContentProvider, TextContentProvider>();
        }
    }
}
