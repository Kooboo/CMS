using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Caching;
using Kooboo.CMS.Caching;
namespace Kooboo.CMS.Content.Persistence.Caching
{
    public class TextFolderProvider : CacheProviderBase<TextFolder>, ITextFolderProvider
    {
        private ITextFolderProvider inner;
        public TextFolderProvider(ITextFolderProvider innerProvider)
            : base(innerProvider)
        {
            inner = innerProvider;
        }
        public IQueryable<TextFolder> BySchema(Schema schema)
        {
            return inner.BySchema(schema);
        }

        public IQueryable<TextFolder> ChildFolders(TextFolder parent)
        {
            return parent.Repository.ObjectCache().GetCache<TextFolder[]>("TextFolderProvider.ChildFolders:Parent:" + parent.FullName.ToLower(), () =>
            {
                return inner.ChildFolders(parent).ToArray();
            }).AsQueryable();           
        }

        public IQueryable<TextFolder> All(Repository repository)
        {
            return repository.ObjectCache().GetCache<TextFolder[]>("TextFolderProvider.All", () =>
            {
                return inner.All(repository).ToArray();
            }).AsQueryable();
        }

        public void Export(Repository repository, IEnumerable<TextFolder> models, System.IO.Stream outputStream)
        {
            inner.Export(repository, models, outputStream);
        }

        public void Import(Repository repository, TextFolder folder, System.IO.Stream zipStream, bool @override)
        {
            inner.Import(repository, folder, zipStream, @override);
            repository.ClearCache();
        }

        protected override string GetCacheKey(TextFolder o)
        {
            return "TextFolder:" + o.FullName.ToLower();
        }
    }
}
