using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Data
{
    public interface IQueryableCollection<T>:ICollection<T>
    {
        IQueryable<T> Queryable
        {
            get;
            set;
        }
    }
}
