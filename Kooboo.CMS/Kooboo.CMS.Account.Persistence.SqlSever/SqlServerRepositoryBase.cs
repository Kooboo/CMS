using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Account.Models;

namespace Kooboo.CMS.Account.Persistence.SqlSever
{
    public abstract class SqlServerRepositoryBase<T> 
        where T : class,IPersistable
    {
        private string connectionString;

        public SqlServerRepositoryBase()
        {

        }
        public SqlServerRepositoryBase(string connectionString)
        {
            this.connectionString = connectionString;
        }
        protected AccountDBContext GetDBContext()
        {
            AccountDBContext dbContext = null;
            if (!string.IsNullOrEmpty(connectionString))
            {
                dbContext = new AccountDBContext(connectionString);
            }
            else
            {
                dbContext = new AccountDBContext();
            }
            return dbContext;
        }

        public T Get(T item)
        {
            return Get(GetDBContext(), item);
        }

        public abstract T Get(AccountDBContext dbContext, T item);


        public virtual IQueryable<T> All()
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
