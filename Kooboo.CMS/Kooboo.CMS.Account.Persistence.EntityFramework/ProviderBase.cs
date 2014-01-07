#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Account.Persistence.EntityFramework
{
    public abstract class ProviderBase<T>
        where T : class
    {
        #region .ctor
        AccountDBContext _dbContext = null;
        public ProviderBase(AccountDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        #endregion

        #region All
        public virtual IEnumerable<T> All()
        {
            return _dbContext.Set<T>();
        }
        #endregion

        #region Get
        public abstract T Get(T dummy);
        #endregion

        #region Add
        public virtual void Add(T item)
        {
            Remove(item);

            _dbContext.Set<T>().Add(item);
            _dbContext.SaveChanges();
        }
        #endregion

        #region Update
        public abstract void Update(T @new, T old);
        #endregion

        #region Remove
        public void Remove(T item)
        {
            var entity = Get(item);
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
                _dbContext.SaveChanges();
            }
        }
        #endregion
    }

}

