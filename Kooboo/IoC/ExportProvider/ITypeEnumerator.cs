using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Kooboo.IoC
{
    public interface ITypeEnumerator
    {
        IEnumerable<Type> GetTypes();

        //event Action<Type> OnEnumerate;
    }
}
