using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Data
{
    public class QueryableCollection<T> :Kooboo.Data.Collection<T>, IQueryableCollection<T>
    {
        protected QueryableCollection()
        {
        }

        public QueryableCollection(IQueryable<T> queryable)
        {
            this.Queryable = queryable;
        }

        public IQueryable<T> Queryable
        {
            get;
            set;
        }
    }
}
