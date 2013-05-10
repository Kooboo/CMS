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
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Caching;
namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class UrlKeyMapProvider : SiteElementProviderBase<UrlKeyMap>, IUrlKeyMapProvider
    {
        #region .ctor
        private IUrlKeyMapProvider inner = null;
        public UrlKeyMapProvider(IUrlKeyMapProvider innerRepository)
            : base(innerRepository)
        {
            inner = innerRepository;
        }
        #endregion

        #region Export
        public void Export(Site site, System.IO.Stream outputStream)
        {
            inner.Export(site, outputStream);
        }
        #endregion

        #region Import
        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            inner.Import(site, zipStream, @override);
            site.ClearCache();
        }
        #endregion

        #region GetListCacheKey
        protected override string GetListCacheKey()
        {
            return "UrlKeyMapList";
        }
        #endregion

        #region GetItemCacheKey
        protected override string GetItemCacheKey(UrlKeyMap o)
        {
            return "UrlKeyMap:" + o.Key.ToLower();
        }
        #endregion

        #region All
        public override IEnumerable<UrlKeyMap> All()
        {
            return inner.All();
        }
        #endregion
    }
}
