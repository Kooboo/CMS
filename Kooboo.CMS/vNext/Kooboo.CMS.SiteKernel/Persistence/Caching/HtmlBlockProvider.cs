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
    public class HtmlBlockProvider : SiteElementProviderBase<HtmlBlock>, IHtmlBlockProvider
    {
        #region .ctor
        private IHtmlBlockProvider inner;
        public HtmlBlockProvider(IHtmlBlockProvider inner)
            : base(inner)
        {
            this.inner = inner;
        }
        #endregion

        #region GetItemCacheKey
        protected override string GetItemCacheKey(HtmlBlock o)
        {
            return string.Format("HtmlBlock:{0}", o.Name.ToLower());
        }
        #endregion

        #region Localize
        public void Localize(HtmlBlock o, Site targetSite)
        {
            inner.Localize(o, targetSite);
            ClearObjectCache(targetSite);
        }
        #endregion

        #region InitializeHtmlBlocks
        public void InitializeHtmlBlocks(Site site)
        {
            //try
            //{
            //    inner.InitializeHtmlBlocks(site);
            //}
            //finally
            //{
            //    ClearObjectCache(site);
            //}

        }
        #endregion

        #region ExportHtmlBlocksToDisk
        public void ExportHtmlBlocksToDisk(Site site)
        {
            //inner.ExportHtmlBlocksToDisk(site);
        }

        #endregion

        #region Clear
        public void Clear(Site site)
        {
            try
            {
                inner.Clear(site);
            }
            finally
            {
                ClearObjectCache(site);
            }

        }
        #endregion

        #region Export
        public void Export(Site site, IEnumerable<HtmlBlock> sources, System.IO.Stream outputStream)
        {
            inner.Export(site, sources, outputStream);
        }
        #endregion

        #region Import
        public void Import(Site site, System.IO.Stream zipStream, bool @override)
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


        #region ISiteElementProvider InitializeToDB/ExportToDisk
        public void InitializeToDB(Site site)
        {
            try
            {
                inner.InitializeToDB(site);
            }
            finally
            {
                ClearObjectCache(site);
            }
        }

        public void ExportToDisk(Site site)
        {
            inner.ExportToDisk(site);
        }
        #endregion

    }
}
