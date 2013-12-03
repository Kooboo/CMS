using Kooboo.CMS.Modules.Publishing.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Services
{

    public abstract class ManagerBase<T>
        where T : class
    {
        #region .ctor
        IPublishingProvider<T> _provider;
        public ManagerBase(IPublishingProvider<T> provider)
        {
            _provider = provider;
        }
        #endregion

        #region CreateQuery
        public IQueryable<T> CreateQuery()
        {
            return this._provider.CreateQuery();
        }

        #endregion

        #region CreateQuery
        public IQueryable<T> CreateQuery(string siteName)
        {
            return this._provider.CreateQuery(siteName);
        }
        #endregion

        #region Add
        public virtual void Add(T obj)
        {
            _provider.Add(obj);
        }
        #endregion

        #region Update
        public virtual void Update(T @new, T old)
        {
            _provider.Update(@new, old);
        }
        #endregion

        #region Delete
        public virtual void Delete(T obj)
        {
            _provider.Remove(obj);
        }
        #endregion
    }
}
