using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.IoC
{
    public class ObjectFactory : IObjectFacotry
    {

        public object CreateInstance(Type type)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch (MissingMethodException ex)//Can not find a public constructor without parameters for the type
            {
                throw new NotSupportedException(type.ToString(), ex);
            }
        }
    }
}
