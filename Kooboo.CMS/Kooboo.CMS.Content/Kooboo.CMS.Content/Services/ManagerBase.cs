using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Content.Models;
using Kooboo.Extensions;
using System.Web.Mvc;
namespace Kooboo.CMS.Content.Services
{
    [HandleError(Order = 1)]
    public abstract class ManagerBase<T, Provider> : IManager<T, Provider>
        where T : class, IRepositoryElement
        where Provider : IProvider<T>
    {
        #region IManager<T> Members

        public virtual Provider GetProvider()
        {
            return Providers.DefaultProviderFactory.GetProvider<Provider>();
        }

        public virtual IEnumerable<T> All(Models.Repository repository, string filterName)
        {
            var r = GetProvider().All(repository);
            if (!string.IsNullOrEmpty(filterName))
            {
                r = r.Where(it => it.Name.Contains(filterName, StringComparison.CurrentCultureIgnoreCase));
            }

            return r;
        }

        public abstract T Get(Models.Repository repository, string name);


        public virtual void Add(Models.Repository repository, T item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new NameIsReqiredException();
            }

            var dbProvider = GetProvider();
            item.Repository = repository;
            if (dbProvider.Get(item) != null)
            {
                throw new ItemAlreadyExistsException();
            }

            dbProvider.Add(item);
        }

        public virtual void Update(Models.Repository repository, T @new, T old)
        {
            if (string.IsNullOrEmpty(@new.Name))
            {
                throw new NameIsReqiredException();
            }
            var dbProvider = GetProvider();
            old.Repository = repository;
            @new.Repository = repository;
            if (dbProvider.Get(old) == null)
            {
                throw new ItemDoesNotExistException();
            }
            dbProvider.Update(@new, @old);
        }

        public virtual void Remove(Models.Repository repository, T item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new NameIsReqiredException();
            }
            var dbProvider = GetProvider();
            item.Repository = repository;
            if (dbProvider.Get(item) == null)
            {
                throw new ItemDoesNotExistException();
            }

            dbProvider.Remove(item);
        }

        #endregion
    }
}
