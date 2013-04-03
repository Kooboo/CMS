using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Kooboo.IoC
{
    public interface IRequestContext
    {
        IDictionary Items
        {
            get;
        }
    }
}
