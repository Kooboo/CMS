#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class ABRuleSettingProvider : SiteElementProviderBase<ABRuleSetting>, IABRuleSettingProvider
    {
        #region .ctor
        private IABRuleSettingProvider _provider;
        public ABRuleSettingProvider(IABRuleSettingProvider provider)
            : base(provider)
        {
            this._provider = provider;
        }
        #endregion

        #region GetListCacheKey
        protected override string GetListCacheKey()
        {
            return "All-VisitRuleSettings:";
        }
        #endregion

        #region GetItemCacheKey
        protected override string GetItemCacheKey(ABRuleSetting o)
        {
            return string.Format("VisitRuleSetting:{0}", o.Name);
        }
        #endregion

        #region Export
        public void Export(Site site, IEnumerable<ABRuleSetting> sources, System.IO.Stream outputStream)
        {
            _provider.Export(site, sources, outputStream);
        }
        #endregion

        #region Import
        public void Import(Models.Site site, System.IO.Stream zipStream, bool @override)
        {
            try
            {
                _provider.Import(site, zipStream, @override);
            }
            finally
            {
                ClearObjectCache(site);
            }
        }
        #endregion

        #region ISiteElementProvider InitializeToDB/ExportToDisk
        public void InitializeToDB(Site site)
        {
            try
            {
                _provider.InitializeToDB(site);
            }
            finally
            {
                ClearObjectCache(site);
            }
        }

        public void ExportToDisk(Site site)
        {
            _provider.ExportToDisk(site);
        }
        #endregion
    }
}
