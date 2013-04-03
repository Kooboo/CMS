using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Collections
{
    public class ComparedResult<T>
    {
        public ComparedResult()
        {
            this.Added = new List<T>();
            this.Deleted = new List<T>();
            this.Updated = new List<T>();
        }

        public List<T> Added
        {
            get;
            set;
        }

        public List<T> Deleted
        {
            get;
            set;
        }

        public List<T> Updated
        {
            get;
            set;
        }

    
    }
}
