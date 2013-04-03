using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Account.Models;

namespace Kooboo.CMS.Account.Persistence
{
    public interface IRepository<T>
    {
        IQueryable<T> All();

        T Get(T dummy);
        void Add(T item);
        void Update(T @new, T old);
        void Remove(T item);
    }
}
