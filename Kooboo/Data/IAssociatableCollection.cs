using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo
{
    public interface IAssociatableCollection<T>:ICollection<T>
    {
        void Add(T item, bool isAssociatable);

        void Remove(T item, bool isAssociatable);

        IEnumerable<T> Removed
        {
            get;
        }

        IEnumerable<T> Added
        {
            get;
        }

        //event Func<T, bool> Adding;

        //event Func<T, bool> Removing;

    }
}
