using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Caching;
namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class LayoutProvider : CacheObjectProviderBase<Layout>, ILayoutProvider
    {
        private ILayoutProvider inner;
        public LayoutProvider(ILayoutProvider innerRepository)
            : base(innerRepository)
        {
            inner = innerRepository;
        }
        //public IEnumerable<Models.LayoutSample> AllSamples()
        //{
        //    return inner.AllSamples();
        //}

        //public Models.LayoutSample GetLayoutSample(string name)
        //{
        //    return inner.GetLayoutSample(name);
        //}

        public IQueryable<Models.Layout> All(Models.Site site)
        {
            return inner.All(site);
        }

        public void Export(IEnumerable<Models.Layout> sources, System.IO.Stream outputStream)
        {
            inner.Export(sources, outputStream);
        }

        public void Import(Models.Site site, System.IO.Stream zipStream, bool @override)
        {
            inner.Import(site, zipStream, @override);
            site.ClearCache();
        }

        public void Localize(Models.Layout o, Models.Site targetSite)
        {
            ClearObjectCache(new Layout(targetSite, o.Name));

            inner.Localize(o, targetSite);
        }

        protected override string GetCacheKey(Layout o)
        {
            return string.Format("Layout:{0}", o.Name.ToLower());
        }

        public Layout Copy(Site site, string sourceName, string destName)
        {
            ClearObjectCache(new Layout(site, destName));

            return inner.Copy(site, sourceName, destName);
        }
    }
}
