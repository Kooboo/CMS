using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence.Couchbase.ABTestProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IABPageSettingProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<ABPageSetting>), Order = 100)]
    public class ABPageSettingProvider : ProviderBase<ABPageSetting>, IABPageSettingProvider
    {
        Kooboo.CMS.Sites.Persistence.FileSystem.ABPageSettingProvider fileProvider;

        public ABPageSettingProvider()
            : base(ModelExtensions.ABPageSettingDataType, (Site site, string key) => 
            { return new ABPageSetting() { Site = site, MainPage = key }; })
        {
            fileProvider = new Kooboo.CMS.Sites.Persistence.FileSystem.ABPageSettingProvider();
        }

        public void Export(IEnumerable<ABTest.ABPageSetting> sources, System.IO.Stream outputStream)
        {
            foreach (var item in sources)
            {
                fileProvider.Add(item.AsActual());
            }
            fileProvider.Export(sources, outputStream);
        }

        public void Import(Models.Site site, System.IO.Stream zipStream, bool @override)
        {
            var allItem = fileProvider.All(site);
            foreach (var item in allItem)
            {
                fileProvider.Remove(item);
            }
            fileProvider.Import(site, zipStream, @override);
            allItem = fileProvider.All(site);
            if (!@override)
            {
                allItem = allItem.Where(it => null == Get(it));
            }
            var dummy = allItem.ToList();
            foreach (var item in dummy)
            {
                var tempItem = fileProvider.Get(item);
                tempItem.Site = site;
                InsertOrUpdate(tempItem, tempItem);
            }
        }
        public void InitializeABPageSetting(Site site)
        {
            foreach (var item in fileProvider.All(site))
            {
                if (item.Site == site)
                {
                    this.Add(fileProvider.Get(item));
                }
            }
        }

        public void ExportABPageSettingToDisk(Site site)
        {
            var fileAll = fileProvider.All(site);
            foreach (var item in fileAll)
            {
                fileProvider.Remove(item);
            }

            var allItem = this.All(site).ToList();
            foreach (var item in allItem)
            {
                fileProvider.Add(item.AsActual());
            }
        }
    }
}
