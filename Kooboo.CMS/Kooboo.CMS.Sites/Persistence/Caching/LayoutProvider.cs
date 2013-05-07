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
    public class LayoutProvider : SiteElementProviderBase<Layout>, ILayoutProvider
    {
        #region .ctor
        private ILayoutProvider inner;
        public LayoutProvider(ILayoutProvider innerRepository)
            : base(innerRepository)
        {
            inner = innerRepository;
        }
        #endregion
        
        #region Export
        public void Export(IEnumerable<Models.Layout> sources, System.IO.Stream outputStream)
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

        #region Localize
        public void Localize(Models.Layout o, Models.Site targetSite)
        {
            ClearObjectCache(targetSite);

            inner.Localize(o, targetSite);
        }
        #endregion

        #region GetItemCacheKey
        protected override string GetItemCacheKey(Layout o)
        {
            return string.Format("Layout:{0}", o.Name.ToLower());
        }
        #endregion

        #region Copy
        public Layout Copy(Site site, string sourceName, string destName)
        {
            ClearObjectCache(site);

            return inner.Copy(site, sourceName, destName);
        }
        #endregion



    }
}
