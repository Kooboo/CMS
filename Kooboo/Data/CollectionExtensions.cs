using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Data
{
    public static class CollectionExtensions
    {
        public static AssociatableQueryableCollection<T> AsAssociatableQueryableCollection<T>(this IQueryable<T> queryable)
        {
            return new AssociatableQueryableCollection<T>(queryable);
        }

        public static QueryableCollection<T> AsQueryableCollection<T>(this IQueryable<T> queryable)
        {
            return new QueryableCollection<T>(queryable);
        }

        public static CascadableCollection<T> AsCascadableCollection<T>(this IEnumerable<T> enumerator)
        {
            CascadableCollection<T> collection = new CascadableCollection<T>();
            using (var reader = enumerator.GetEnumerator())
            {
                while (reader.MoveNext())
                {
                    collection.Add(reader.Current);
                }
            }

            return collection;

        }
    }
}
