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
using System.Collections;
using Kooboo.CMS.Content.Persistence.Caching;
using Kooboo.CMS.Content.Persistence;

namespace Kooboo.CMS.Search.Persistence.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public class ProviderFactory : CacheProviderFactory
    {
        public ProviderFactory(IProviderFactory innerFactory)
            : base(innerFactory)
        {

        }
        public override T GetProvider<T>()
        {
            var innerProvider = innerFactory.GetProvider<T>();

            if (innerProvider is ISearchSettingProvider)
            {
                innerProvider = (T)(object)new SearchSettingProvider((ISearchSettingProvider)innerProvider);
            }
            return innerProvider;
        }
    }
}
