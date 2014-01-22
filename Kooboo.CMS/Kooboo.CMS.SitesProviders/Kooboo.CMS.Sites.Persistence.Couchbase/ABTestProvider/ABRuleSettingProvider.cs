using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence.Couchbase.ABTestProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IABRuleSettingProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<ABRuleSetting>), Order = 100)]
    public class ABRuleSettingProvider : ProviderBase<ABRuleSetting>,IABRuleSettingProvider
    {
        Kooboo.CMS.Sites.Persistence.FileSystem.ABRuleSettingProvider fileProvider;

        public ABRuleSettingProvider()
            : base(ModelExtensions.ABRuleSettingDataType, (Site site, string key) => { return new ABRuleSetting(site, key); })
        {
            fileProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.ABRuleSettingProvider(new Kooboo.CMS.Common.BaseDir());
        }

        public void Export(IEnumerable<ABRuleSetting> sources, System.IO.Stream outputStream)
        {
            foreach (var item in sources)
            {
                var abRuleSetting = Get(item);
                fileProvider.Add(abRuleSetting);
            }
            fileProvider.Export(sources, outputStream);
        }

        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            fileProvider.Import(site, zipStream, @override);
            var allItem = fileProvider.All(site);
            if (!@override)
            {
                allItem = allItem.Where(it => null == Get(it));
            }
            var dummy = allItem.ToList();
            foreach (var item in dummy)
            {
                var tempItem = fileProvider.Get(item);
                InsertOrUpdate(tempItem, tempItem);
            }
        }

        public void InitializeABRuleSetting(Site site)
        {
            foreach (var item in fileProvider.All(site))
            {
                if (item.Site == site)
                {
                    this.Add(fileProvider.Get(item));
                }
            }
        }

        public void ExportABRuleSettingToDisk(Site site)
        {
            var provider = new Kooboo.CMS.Sites.Persistence.FileSystem.ABRuleSettingProvider(new Kooboo.CMS.Common.BaseDir());
            var fileAll = provider.All(site);
            foreach (var item in fileAll)
            {
                provider.Remove(item);
            }

            var allItem = this.All(site).ToList();
            foreach (var item in allItem)
            {
                provider.Add(item);
            }
        }
    }
}
