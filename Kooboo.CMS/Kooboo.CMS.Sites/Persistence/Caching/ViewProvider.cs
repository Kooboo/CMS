using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Caching;

namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class ViewProvider : CacheObjectProviderBase<Kooboo.CMS.Sites.Models.View>, IViewProvider
    {
        private IViewProvider inner;
        public ViewProvider(IViewProvider innerRepository)
            : base(innerRepository)
        {
            this.inner = innerRepository;
        }
        protected override string GetCacheKey(Models.View o)
        {
            return string.Format("View:{0}", o.Name.ToLower());
        }

        public IQueryable<Models.View> All(Models.Site site)
        {
            return inner.All(site);
        }

        public void Localize(Models.View o, Models.Site targetSite)
        {
            ClearObjectCache(new Models.View(targetSite, o.Name));
            inner.Localize(o, targetSite);
        }

        public void Export(IEnumerable<Models.View> sources, System.IO.Stream outputStream)
        {
            inner.Export(sources, outputStream);
        }

        public void Import(Models.Site site, System.IO.Stream zipStream, bool @override)
        {
            inner.Import(site, zipStream, @override);

            site.ClearCache();
        }

        public Models.View Copy(Models.Site site, string sourceName, string destName)
        {
            ClearObjectCache(new Models.View(site, destName));
            return inner.Copy(site, sourceName, destName);
        }
    }
}
