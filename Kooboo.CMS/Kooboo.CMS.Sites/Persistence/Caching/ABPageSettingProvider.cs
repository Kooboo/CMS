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
    public class ABPageSettingProvider : SiteElementProviderBase<ABPageSetting>, IABPageSettingProvider
    {
        #region .ctor
        IABPageSettingProvider _provider;
        public ABPageSettingProvider(IABPageSettingProvider provider)
            : base(provider)
        {
            this._provider = provider;
        }
        #endregion

        #region GetListCacheKey
        protected override string GetListCacheKey()
        {
            return "All-PageVisisRules";
        } 
        #endregion

        #region GetItemCacheKey
        protected override string GetItemCacheKey(ABPageSetting o)
        {
            return string.Format("PageVisit:{0}", o.MainPage);
        }
        #endregion

        #region Export
        public void Export(IEnumerable<ABPageSetting> sources, System.IO.Stream outputStream)
        {
            this._provider.Export(sources, outputStream);
        }
        #endregion

        #region Import
        public void Import(Models.Site site, System.IO.Stream zipStream, bool @override)
        {
            try
            {
                this._provider.Import(site, zipStream, @override);
            }
            finally
            {
                ClearObjectCache(site);
            }
        }
        #endregion
    }
}
