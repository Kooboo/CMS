using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Caching;
using Kooboo.CMS.Caching;
namespace Kooboo.CMS.Content.Persistence.Caching
{
    public class MediaFolderProvider : CacheProviderBase<MediaFolder>, IMediaFolderProvider
    {
        private IMediaFolderProvider inner;
        public MediaFolderProvider(IMediaFolderProvider innerProvider)
            : base(innerProvider)
        {
            inner = innerProvider;
        }


        public IQueryable<MediaFolder> ChildFolders(MediaFolder parent)
        {
            return parent.Repository.ObjectCache().GetCache<MediaFolder[]>("MediaFolderProvider.ChildFolders:Parent:" + parent.FullName.ToLower(), () =>
            {
                return inner.ChildFolders(parent).ToArray();
            }).AsQueryable();
        }

        public IQueryable<MediaFolder> All(Repository repository)
        {
            return repository.ObjectCache().GetCache<MediaFolder[]>("MediaFolderProvider.All", () =>
            {
                return inner.All(repository).ToArray();
            }).AsQueryable();
        }

        public void Export(Repository repository, IEnumerable<MediaFolder> models, System.IO.Stream outputStream)
        {
            inner.Export(repository, models, outputStream);
        }

        public void Import(Repository repository, MediaFolder folder, System.IO.Stream zipStream, bool @override)
        {
            inner.Import(repository, folder, zipStream, @override);
            repository.ClearCache();
        }

        protected override string GetCacheKey(MediaFolder o)
        {
            return "MediaFolder:" + o.FullName.ToLower();
        }
    }
}
