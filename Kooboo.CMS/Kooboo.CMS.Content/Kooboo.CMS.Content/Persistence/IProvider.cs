using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence
{
    public interface IProvider<T>
    {
        IQueryable<T> All(Repository repository);

        T Get(T dummy);
        void Add(T item);
        void Update(T @new, T old);
        void Remove(T item);
    }
}
