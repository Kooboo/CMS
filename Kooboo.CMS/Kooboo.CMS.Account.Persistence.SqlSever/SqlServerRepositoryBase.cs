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
using System.Linq;
using System.Text;
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Account.Persistence.SqlSever
{
    public abstract class SqlServerRepositoryBase<T>
        where T : class,IPersistable
    {
        public SqlServerRepositoryBase()
        {

        }

        protected AccountDBContext GetDBContext()
        {
            AccountDBContext dbContext = null;

            dbContext = new AccountDBContext();

            return dbContext;
        }

        public T Get(T item)
        {
            return Get(GetDBContext(), item);
        }

        public abstract T Get(AccountDBContext dbContext, T item);


        public virtual IEnumerable<T> All()
        {
            return GetDBContext().Set<T>();
        }


        public virtual void Add(T item)
        {
            var dbContext = GetDBContext();

            dbContext.Set<T>().Add(item);

            dbContext.SaveChanges();

        }


        public virtual void Update(T @new, T old)
        {

        }


        public virtual void Remove(T item)
        {
            var dbContext = GetDBContext();
            item = Get(dbContext, item);
            dbContext.Set<T>().Remove(item);
            dbContext.SaveChanges();
        }
    }
}
