using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Data
{
    public interface IRepository<T>
    {
        IQueryable<T> AsQueryable();

        bool Add(T item);
        bool Update(T item);
        bool Remove(T item);
    } 
}
