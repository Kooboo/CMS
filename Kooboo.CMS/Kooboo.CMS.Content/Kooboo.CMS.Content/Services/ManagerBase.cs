#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Extensions;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Kooboo.CMS.Content.Services
{
    public abstract class ManagerBase<T, P> : IManager<T, P>
        where T : class, IRepositoryElement
        where P : class,Kooboo.CMS.Content.Persistence.IContentElementProvider<T>
    {
        public P Provider { get; private set; }
        private ManagerBase() { }
        public ManagerBase(P provider)
        {
            this.Provider = provider;
        }
        #region IManager<T> Members

        public virtual IEnumerable<T> All(Models.Repository repository, string filterName)
        {
            var r = Provider.All(repository);
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

            var dbProvider = Provider;
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
            var dbProvider = Provider;
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
            var dbProvider = Provider;
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
