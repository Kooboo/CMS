using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.IoC
{
    public class CreateHandler
    {
        public Type Type
        {
            get;
            set;
        }
        public Func<Object> Invoker
        {
            get;
            set;
        }
    }
}
