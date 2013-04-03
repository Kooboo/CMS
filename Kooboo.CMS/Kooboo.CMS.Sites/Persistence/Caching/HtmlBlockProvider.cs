using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Caching;
namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class HtmlBlockProvider : CacheObjectProviderBase<HtmlBlock>, IHtmlBlockProvider
    {
        private IHtmlBlockProvider inner;
        public HtmlBlockProvider(IHtmlBlockProvider inner)
            : base(inner)
        {
            this.inner = inner;
        }
        protected override string GetCacheKey(HtmlBlock o)
        {
            return string.Format("HtmlBlock:{0}", o.Name.ToLower());
        }

        public IQueryable<HtmlBlock> All(Site site)
        {
            return inner.All(site);
        }

        public void Localize(HtmlBlock o, Site targetSite)
        {
            inner.Localize(o, targetSite);
            ClearObjectCache(new HtmlBlock(targetSite, o.Name));
        }

        public void InitializeHtmlBlocks(Site site)
        {
            inner.InitializeHtmlBlocks(site);
        }

        public void ExportHtmlBlocksToDisk(Site site)
        {
            inner.ExportHtmlBlocksToDisk(site);
        }


        public void Clear(Site site)
        {
            inner.Clear(site);
        }


        public void Export(IEnumerable<HtmlBlock> sources, System.IO.Stream outputStream)
        {
            inner.Export(sources, outputStream);
        }

        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            inner.Import(site, zipStream, @override);
            site.ClearCache();
        }
    }
}
