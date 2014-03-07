#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.ABTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class ABSiteSettingProvider : ProviderBase<ABSiteSetting>, IABSiteSettingProvider
    {
        #region .ctor
        IABSiteSettingProvider _provider;
        public ABSiteSettingProvider(IABSiteSettingProvider provider)
            : base(provider)
        {
            this._provider = provider;
        }
        #endregion

        #region GetItemCacheKey
        protected override string GetItemCacheKey(ABSiteSetting o)
        {
            return string.Format("SiteVisitRule:{0}", o.MainSite);
        }
        #endregion

        #region GetListCacheKey
        protected override string GetListCacheKey()
        {
            return "All-SiteVisitRules:";
        }
        #endregion

        #region Export
        public void Export(IEnumerable<ABSiteSetting> sources, System.IO.Stream outputStream)
        {
            this._provider.Export(sources, outputStream);
        } 
        #endregion

        #region Import
        public void Import(System.IO.Stream zipStream, bool @override)
        {
            try
            {
                this._provider.Import(zipStream, @override);
            }
            finally
            {
                ClearObjectCache(null);
            }
        } 
        #endregion
    }
}
