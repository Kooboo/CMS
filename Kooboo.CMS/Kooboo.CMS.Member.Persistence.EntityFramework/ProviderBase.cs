#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Member.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Member.Persistence.EntityFramework
{
    public abstract class ProviderBase<T> : IMemberElementProvider<T>
        where T : class, IMemberElement
    {
        #region .ctor
        MemberDBContext _dbContext = null;
        public ProviderBase(MemberDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        #endregion

        #region All
        public virtual IEnumerable<T> All()
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<T> All(Membership membership)
        {
            return _dbContext.Set<T>().Where(it => it.Membership.Name == membership.Name);
        }
        #endregion

        #region IMemberElementProvider
        IEnumerable<T> IMemberElementProvider<T>.All(Membership membership)
        {
            return All(membership);
        }
        #endregion

        #region Get
        public abstract T Get(T dummy);
        #endregion

        #region Add
        public virtual void Add(T item)
        {
            Remove(item);

            _dbContext.Set<Membership>().Attach(item.Membership);
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

