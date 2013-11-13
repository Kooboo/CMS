using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Membership.Persistence;
using Kooboo.CMS.Sites.Globalization;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence.Couchbase
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISiteProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Site>))]
    public class SiteProvider : Kooboo.CMS.Sites.Persistence.FileSystem.SiteProvider
    {
        SiteInitializer _initializer;
        public SiteProvider(IBaseDir baseDir, IMembershipProvider membershipProvider, IElementRepositoryFactory elementRepositoryFactory, SiteInitializer initializer)
            : base(baseDir, membershipProvider, elementRepositoryFactory)
        {
            _initializer = initializer;
        }
        public override void Initialize(Site site)
        {
            _initializer.Initialize(site);
            base.Initialize(site);
        }
    }
}
