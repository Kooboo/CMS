using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo
{
    public interface ICascadableCollection<T> : IAssociatableCollection<T>
    {

        IEnumerable<T> Updated
        {
            get;
        }

    }
}
