using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence.Couchbase.ABTestProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IABPageTestResultProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<ABPageTestResult>), Order = 100)]
    public class ABPageTestResultProvider:ABProviderBase<ABPageTestResult>,IABPageTestResultProvider
    {
        public ABPageTestResultProvider()
            : base(ModelExtensions.ABPageTestResultDataType, (Site site, string key) => { return new ABPageTestResult() { Site = site, UUID = key }; })
        { 
        }
    }
}
