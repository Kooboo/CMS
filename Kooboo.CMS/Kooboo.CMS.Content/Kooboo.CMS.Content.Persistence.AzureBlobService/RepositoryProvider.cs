using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.AzureBlobService
{
    public class RepositoryProvider : IRepositoryProvider
    {
        public void Initialize(Models.Repository repository)
        {
            inner.Initialize(repository);
            MediaBlobHelper.InitializeRepositoryContainer(repository);
        }

        public void Remove(Models.Repository item)
        {
            inner.Remove(item);
            MediaBlobHelper.DeleteRepositoryContainer(item);
        }

        public bool TestDbConnection()
        {
            return inner.TestDbConnection();
        }

        private IRepositoryProvider inner;
        public RepositoryProvider(IRepositoryProvider innerProvider)
        {
            inner = innerProvider;
        }
        public IQueryable<Repository> All()
        {
            return inner.All();
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

        public Repository Create(string repositoryName, System.IO.Stream templateStream)
        {
            return inner.Create(repositoryName, templateStream);
        }

        public Repository Copy(Repository sourceRepository, string destRepositoryName)
        {
            return inner.Copy(sourceRepository, destRepositoryName);
        }

        public void Export(Repository repository, System.IO.Stream outputStream)
        {
            inner.Export(repository, outputStream);
        }

        public IQueryable<Repository> All(Repository repository)
        {
            return inner.All(repository);
        }

        public Repository Get(Repository dummy)
        {
            return inner.Get(dummy);
        }

        public void Add(Repository item)
        {
            inner.Add(item);
        }

        public void Update(Repository @new, Repository old)
        {
            inner.Update(@new, old);
        }
    }
}
