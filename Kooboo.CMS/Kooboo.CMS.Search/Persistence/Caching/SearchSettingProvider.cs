#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Content.Persistence.Default;
using System.IO;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Content.Persistence.Caching;

namespace Kooboo.CMS.Search.Persistence.Caching
{
    public class SearchSettingProvider : CacheProviderBase<SearchSetting>, ISearchSettingProvider
    {
        public SearchSettingProvider(ISearchSettingProvider provider)
            : base(provider)
        {

        }
        protected override string GetCacheKey(SearchSetting o)
        {
            return "FolderSearchSetting" + o.FolderName.ToLower();
        }

        public IEnumerable<SearchSetting> All(Content.Models.Repository repository)
        {
            return innerProvider.All(repository);
        }

        public IEnumerable<SearchSetting> All()
        {
            return innerProvider.All();
        }
    }
}
