using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Caching;
namespace Kooboo.CMS.Content.Persistence.Caching
{
    public class RepositoryProvider : IRepositoryProvider
    {
        private IRepositoryProvider inner;
        public RepositoryProvider(IRepositoryProvider innerProvider)
        {
            inner = innerProvider;
        }
        public IQueryable<Models.Repository> All()
        {
            return inner.All();
        }

        public IQueryable<Models.Repository> All(Models.Repository repository)
        {
            return inner.All(repository);
        }

        public Models.Repository Get(Models.Repository dummy)
        {
            var cacheKey = GetCacheKey(dummy);
            var o = (Repository)dummy.ObjectCache().Get(cacheKey);
            if (o == null)
            {
                o = inner.Get(dummy);
                if (o == null)
                {
                    return o;
                }
                dummy.ObjectCache().Add(cacheKey, o, CacheProviderFactory.DefaultCacheItemPolicy);
            }
            return o;
        }
        private string GetCacheKey(Repository repository)
        {
            return "Repository:" + repository.Name.ToLower();
        }
        public void Add(Models.Repository item)
        {
            inner.Add(item);
        }

        public void Update(Models.Repository @new, Models.Repository old)
        {
            inner.Update(@new, old);
            var cacheKey = GetCacheKey(@new);
            @new.ObjectCache().Remove(cacheKey);
        }

        public void Remove(Models.Repository item)
        {
            inner.Remove(item);
            @item.ClearCache();
        }


        public Repository Create(string repositoryName, System.IO.Stream templateStream)
        {
            return inner.Create(repositoryName, templateStream);
        }

        public void Initialize(Repository repository)
        {
            inner.Initialize(repository);
        }

        public void Export(Repository repository, System.IO.Stream outputStream)
        {
            inner.Export(repository, outputStream);
        }

        public void Offline(Repository repository)
        {
            inner.Offline(repository);
        }

        public void Online(Repository repository)
        {
            inner.Online(repository);
        }

        public bool IsOnline(Repository repository)
        {
            return inner.IsOnline(repository);
        }


        public Repository Copy(Repository sourceRepository, string destRepositoryName)
        {
            return inner.Copy(sourceRepository, destRepositoryName);
        }


        public bool TestDbConnection()
        {
            return inner.TestDbConnection();
        }
    }
}
