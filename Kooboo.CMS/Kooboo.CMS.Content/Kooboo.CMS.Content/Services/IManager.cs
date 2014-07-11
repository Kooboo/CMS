#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;
using System;
using System.Linq;
using System.Collections.Generic;
namespace Kooboo.CMS.Content.Services
{
    public interface IManager<T, Provider>
        where T : class
        where Provider : IContentElementProvider<T>
    {
        IEnumerable<T> All(Repository repository, string filterName);
        T Get(Repository repository, string name);
        void Update(Repository repository, T @new, T @old);
        void Add(Repository repository, T item);
        void Remove(Repository repository, T item);
    }

    public abstract class RepositoryElementManager<T, Provider> : IManager<T, Provider>
        where T : class,IRepositoryElement
        where Provider : class, IContentElementProvider<T>
    {
        public virtual Provider GetProvider()
        {
            return Providers.DefaultProviderFactory.GetProvider<Provider>();
        }

        public virtual IEnumerable<T> All(Repository repository, string filterName)
        {
            var r = GetProvider().All(repository);
            if (!string.IsNullOrEmpty(filterName))
            {
                r = r.Where(it => it.Name.Contains(filterName, StringComparison.CurrentCultureIgnoreCase));
            }

            return r;
        }

        public abstract T Get(Repository repository, string name);

        public virtual void Update(Repository repository, T @new, T @old)
        {
            var dbProvider = GetProvider();
            old.Repository = repository;
            @new.Repository = repository;
            if (dbProvider.Get(old) == null)
            {
                throw new ItemDoesNotExistException();
            }
            dbProvider.Update(@new, @old);
        }

        public virtual void Add(Repository repository, T o)
        {
            var dbProvider = GetProvider();
            o.Repository = repository;
            if (dbProvider.Get(o) != null)
            {
                throw new ItemAlreadyExistsException();
            }

            dbProvider.Add(o);
        }

        public virtual void Remove(Repository repository, T o)
        {
            var dbProvider = GetProvider();
            o.Repository = repository;
            if (dbProvider.Get(o) == null)
            {
                throw new ItemDoesNotExistException();
            }

            dbProvider.Remove(o);
        }
    }
}
