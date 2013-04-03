using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Caching;
namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class UrlKeyMapProvider : CacheAllProviderBase<UrlKeyMap>, IUrlKeyMapProvider
    {
        private IUrlKeyMapProvider inner = null;
        public UrlKeyMapProvider(IUrlKeyMapProvider innerRepository)
            : base(innerRepository)
        {
            inner = innerRepository;
        }
        public void Export(Site site, System.IO.Stream outputStream)
        {
            inner.Export(site, outputStream);
        }

        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            inner.Import(site, zipStream, @override);
            site.ClearCache();
        }

        protected override string GetCacheKey(Site site)
        {
            return "UrlKeyMapList";
        }

        protected override string GetCacheKey(UrlKeyMap o)
        {
            return "UrlKeyMap:" + o.Key.ToLower();
        }
    }
}
