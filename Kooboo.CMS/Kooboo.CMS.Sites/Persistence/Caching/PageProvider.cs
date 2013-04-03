using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Caching;
namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class PageProvider : CacheObjectProviderBase<Page>, IPageProvider
    {
        private IPageProvider inner;
        public PageProvider(IPageProvider inner)
            : base(inner)
        {
            this.inner = inner;
        }
        public override Page Get(Page dummy)
        {
            var page = base.Get(dummy);
            return page;
        }
        public override void Add(Page item)
        {
            base.Add(item);
            item.Site.ClearCache();
        }
        public override void Update(Page @new, Page old)
        {
            base.Update(@new, old);
            @new.Site.ClearCache();
        }
        public override void Remove(Page item)
        {
            base.Remove(item);
            item.Site.ClearCache();
        }
        public IQueryable<Models.Page> ChildPages(Models.Page parentPage)
        {
            if (parentPage != null)
            {
                string cacheKey = string.Format("GetChildPages:ParentPage-{0}", parentPage.FullName.ToLower());
                var childPages = parentPage.Site.ObjectCache().Get(cacheKey) as IEnumerable<Page>;
                if (childPages == null)
                {
                    childPages = inner.ChildPages(parentPage).ToArray();
                    parentPage.Site.ObjectCache().Add(cacheKey, childPages, CacheProviderFactory.DefaultCacheItemPolicy);
                }
                return childPages.AsQueryable();
            }
            else
            {
                return new Page[0].AsQueryable();
            }
        }


        //public Models.Page GetPageByUrl(Models.Site site, string url)
        //{
        //    return inner.GetPageByUrl(site, url);
        //}

        public IQueryable<Models.Page> ByLayout(Models.Layout layout)
        {
            return inner.ByLayout(layout);
        }

        public IQueryable<Models.Page> ByView(Models.View view)
        {
            return inner.ByView(view);
        }

        public IQueryable<Models.Page> ByModule(Models.Site site, string moduleName)
        {
            return inner.ByModule(site, moduleName);
        }

        public IQueryable<Models.Page> All(Models.Site site)
        {
            string cacheKey = "AllPages";
            var pages = site.ObjectCache().Get(cacheKey) as IEnumerable<Page>;
            if (pages == null)
            {
                pages = inner.All(site).ToArray();
                site.ObjectCache().Add(cacheKey, pages, CacheProviderFactory.DefaultCacheItemPolicy);
            }
            return pages.AsQueryable();
        }

        protected override string GetCacheKey(Page page)
        {
            return string.Format("Page:{0}", page.FullName.ToLower());
        }

        public void Export(IEnumerable<Page> sources, System.IO.Stream outputStream)
        {
            inner.Export(sources, outputStream);
        }

        public void Import(Site site, Page parent, System.IO.Stream zipStream, bool @override)
        {
            inner.Import(site, parent, zipStream, @override);
            site.ClearCache();
        }

        public void Localize(Models.Page o, Models.Site targetSite)
        {
            inner.Localize(o, targetSite);
            targetSite.ClearCache();
        }


        public Page Copy(Site site, string sourcePageFullName, string newPageFullName)
        {
            var page = inner.Copy(site, sourcePageFullName, newPageFullName);
            site.ClearCache();
            return page;
        }


        //public Page GetPageByUrlIdentifier(Site site, string identifier)
        //{
        //    return inner.GetPageByUrlIdentifier(site, identifier);
        //}


        public void Move(Site site, string pageFullName, string newParent)
        {
            inner.Move(site, pageFullName, newParent);
            site.ClearCache();
        }


        public Page GetDraft(Page page)
        {
            return inner.GetDraft(page);
        }

        public void SaveAsDraft(Page page)
        {
            inner.SaveAsDraft(page);
        }


        public void RemoveDraft(Page page)
        {
            inner.RemoveDraft(page);
        }


        public void InitializePages(Site site)
        {
            inner.InitializePages(site);
        }


        public void ExportPagesToDisk(Site site)
        {
            inner.ExportPagesToDisk(site);
        }


        public void Clear(Site site)
        {
            inner.Clear(site);
        }
    }
}