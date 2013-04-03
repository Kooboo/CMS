using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;
using System.IO;
using Kooboo.Globalization;
namespace Kooboo.CMS.Content.Services
{
    public class RepositoryManager
    {
        IRepositoryProvider DBProvider
        {
            get
            {
                return Providers.RepositoryProvider;
            }
        }

        public virtual IEnumerable<Repository> All()
        {
            return DBProvider.All(null);
        }

        public virtual void Add(Repository repository)
        {
            if (string.IsNullOrEmpty(repository.Name))
            {
                throw new NameIsReqiredException();
            }
            if (DBProvider.Get(repository) != null)
            {
                throw new ItemAlreadyExistsException();
            }
            repository.DBProvider = Providers.DefaultProviderFactory.Name;
            DBProvider.Add(repository);
        }

        public virtual void Update(Repository repository, Repository old)
        {
            if (string.IsNullOrEmpty(repository.Name))
            {
                throw new NameIsReqiredException();
            }
            if (DBProvider.Get(repository) == null)
            {
                throw new ItemDoesNotExistException();
            }
            DBProvider.Update(repository, old);
        }

        public virtual void Remove(Repository repository)
        {
            if (string.IsNullOrEmpty(repository.Name))
            {
                throw new NameIsReqiredException();
            }
            if (DBProvider.Get(repository) != null)
            {
                DBProvider.Remove(repository);
            }
        }

        public virtual Repository Get(string name)
        {
            return DBProvider.Get(new Repository(name));
        }

        public virtual Repository Create(string repositoryName, string templateName)
        {
            if (string.IsNullOrEmpty(templateName))
            {
                var repository = new Repository(repositoryName);
                Add(repository);
                return repository;
            }
            else
            {
                var template = ServiceFactory.RepositoryTemplateManager.GetItemTemplate(templateName);
                if (template == null)
                {
                    throw new KoobooException("The template file does not exists.");
                }
                using (FileStream fs = new FileStream(template.TemplateFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return Create(repositoryName, fs);
                }
            }
        }
        public virtual Repository Create(string repositoryName, Stream templateStream)
        {
            var repository = DBProvider.Create(repositoryName, templateStream);
            DBProvider.Initialize(repository);
            DBProvider.Online(repository);

            return repository;
        }
        public virtual Repository Copy(Repository sourceRepository, string destRepositoryName)
        {
            var repository = DBProvider.Copy(sourceRepository, destRepositoryName);

            return repository;
        }
        public virtual void Export(string repositoryName, Stream outputStream)
        {
            Repository repository = new Repository(repositoryName);
            if (Get(repositoryName) == null)
            {
                throw new Exception(string.Format("The repository does not exists:'{0}'".Localize(), repositoryName));
            }
            DBProvider.Export(repository, outputStream);
        }

        public virtual void Offline(string repositoryName)
        {
            DBProvider.Offline(new Repository(repositoryName));
        }
        public virtual void Online(string repositoryName)
        {
            DBProvider.Online(new Repository(repositoryName));
        }
        public virtual bool IsOnline(string repositoryName)
        {
            return DBProvider.IsOnline(new Repository(repositoryName));
        }
    }
}
