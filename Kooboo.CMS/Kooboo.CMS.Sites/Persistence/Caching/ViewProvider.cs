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
using Kooboo.CMS.Sites.Caching;

namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class ViewProvider : SiteElementProviderBase<Kooboo.CMS.Sites.Models.View>, IViewProvider
    {
        #region .ctor
        private IViewProvider inner;
        public ViewProvider(IViewProvider innerRepository)
            : base(innerRepository)
        {
            this.inner = innerRepository;
        }
        #endregion

        #region GetItemCacheKey
        protected override string GetItemCacheKey(Models.View o)
        {
            return string.Format("View:{0}", o.Name.ToLower());
        }

        #endregion

        #region Localize
        public void Localize(Models.View o, Models.Site targetSite)
        {
            ClearObjectCache(targetSite);
            inner.Localize(o, targetSite);
        }
        #endregion

        #region Export
        public void Export(IEnumerable<Models.View> sources, System.IO.Stream outputStream)
        {
            inner.Export(sources, outputStream);
        }
        #endregion

        #region Import
        public void Import(Models.Site site, System.IO.Stream zipStream, bool @override)
        {
            inner.Import(site, zipStream, @override);

            site.ClearCache();
        }
        #endregion

        #region Copy
        public Models.View Copy(Models.Site site, string sourceName, string destName)
        {
            ClearObjectCache(site);
            return inner.Copy(site, sourceName, destName);
        }
        #endregion     
    }
}
