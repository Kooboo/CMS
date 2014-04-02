﻿#region License
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
using Kooboo.CMS.Sites.Models;

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
            try
            {
                inner.Localize(o, targetSite);
            }
            finally
            {
                ClearObjectCache(targetSite);
            }
        }
        #endregion

        #region Export
        public void Export(Site site, IEnumerable<Models.View> sources, System.IO.Stream outputStream)
        {
            inner.Export(site, sources, outputStream);
        }
        #endregion

        #region Import
        public void Import(Models.Site site, System.IO.Stream zipStream, bool @override)
        {
            try
            {
                inner.Import(site, zipStream, @override);
            }
            finally
            {
                site.ClearCache();
            }
        }
        #endregion

        #region Copy
        public Models.View Copy(Models.Site site, string sourceName, string destName)
        {
            try
            {
                return inner.Copy(site, sourceName, destName);
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
            //not need to implement.
        }

        public void ExportToDisk(Site site)
        {
            //not need to implement.
        }
        #endregion
    }
}
