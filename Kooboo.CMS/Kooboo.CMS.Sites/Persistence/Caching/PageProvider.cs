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
    public class PageProvider : SiteElementProviderBase<Page>, IPageProvider
    {
        #region .ctor
        private IPageProvider inner;
        public PageProvider(IPageProvider inner)
            : base(inner)
        {
            this.inner = inner;
        }
        #endregion

        #region ChildPages
        public IEnumerable<Models.Page> ChildPages(Models.Page parentPage)
        {
            if (parentPage != null)
            {
                string cacheKey = string.Format("GetChildPages:ParentPage-{0}", parentPage.FullName.ToLower());
                var childPages = parentPage.Site.ObjectCache().Get(cacheKey) as IEnumerable<Page>;
                if (childPages == null)
                {
                    childPages = inner.ChildPages(parentPage).ToArray();
                    parentPage.Site.ObjectCache().Add(cacheKey, childPages, Kooboo.CMS.Caching.ObjectCacheExtensions.DefaultCacheItemPolicy);
                }
                return childPages.AsQueryable();
            }
            else
            {
                return new Page[0].AsQueryable();
            }
        }
        #endregion

        #region ByLayout
        public IEnumerable<Models.Page> ByLayout(Models.Layout layout)
        {
            return inner.ByLayout(layout);
        }
        #endregion

        #region ByView
        public IEnumerable<Models.Page> ByView(Models.View view)
        {
            return inner.ByView(view);
        }
        #endregion

        #region ByModule
        public IEnumerable<Models.Page> ByModule(Models.Site site, string moduleName)
        {
            return inner.ByModule(site, moduleName);
        }
        #endregion

        #region ByHtmlBlock
        public IEnumerable<Models.Page> ByHtmlBlock(HtmlBlock htmlBlock)
        {
            return inner.ByHtmlBlock(htmlBlock);
        }
        #endregion

        protected override string GetListCacheKey()
        {
            return "AllPages:";
        }

        #region GetItemCacheKey
        protected override string GetItemCacheKey(Page page)
        {
            return string.Format("Page:{0}", page.FullName.ToLower());
        }
        #endregion

        #region Export
        public void Export(Site site, IEnumerable<Page> sources, System.IO.Stream outputStream)
        {
            inner.Export(site, sources, outputStream);
        }
        #endregion

        #region Import
        public void Import(Site site, Page parent, System.IO.Stream zipStream, bool @override)
        {
            try
            {
                inner.Import(site, parent, zipStream, @override);
            }
            finally
            {
                site.ClearCache();
            }
        }

        #endregion

        #region Localize
        public void Localize(Models.Page o, Models.Site targetSite)
        {
            try
            {
                inner.Localize(o, targetSite);
            }
            finally
            {
                targetSite.ClearCache();
            }

        }
        #endregion

        #region Copy
        public Page Copy(Site site, string sourcePageFullName, string newPageFullName)
        {
            try
            {
                var page = inner.Copy(site, sourcePageFullName, newPageFullName);

                return page;
            }
            finally
            {
                site.ClearCache();
            }
        }
        #endregion

        #region Move
        public void Move(Site site, string pageFullName, string newParent)
        {
            try
            {
                inner.Move(site, pageFullName, newParent);
            }
            finally
            {
                site.ClearCache();
            }
        }
        #endregion

        #region GetDraft
        public Page GetDraft(Page page)
        {
            return inner.GetDraft(page);
        }
        #endregion

        #region SaveAsDraft
        public void SaveAsDraft(Page page)
        {
            inner.SaveAsDraft(page);
        }
        #endregion

        #region RemoveDraft
        public void RemoveDraft(Page page)
        {
            inner.RemoveDraft(page);
        }
        #endregion

        //#region InitializePages
        //public void InitializePages(Site site)
        //{
        //    try
        //    {
        //        inner.InitializePages(site);
        //    }
        //    finally
        //    {
        //        ClearObjectCache(site);
        //    }
        //}
        //#endregion

        //#region ExportPagesToDisk
        //public void ExportPagesToDisk(Site site)
        //{
        //    inner.ExportPagesToDisk(site);
        //}
        //#endregion

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

        #region All
        public override IEnumerable<Page> All()
        {
            return inner.All();
        }
        #endregion


        #region InitializeToDB/ExportToDisk
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