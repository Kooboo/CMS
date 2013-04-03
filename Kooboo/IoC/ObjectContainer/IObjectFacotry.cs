using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.IoC
{
    public interface IObjectFacotry
    {
        object CreateInstance(Type type);

        //Dictionary<Type, Func<object>> Initializers
        //{
        //    get;
        //}
    }
}
