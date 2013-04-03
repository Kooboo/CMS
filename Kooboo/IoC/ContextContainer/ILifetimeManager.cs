using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Kooboo.IoC
{
    public interface ILifetimeManager:IDisposable
    {
        object Resolve(Type type);
        object Resolve(Type type, string name);

        IEnumerable ResolveAll(Type type);
        IEnumerable ResolveAll(Type type, string name);


        T Resolve<T>();
        T Resolve<T>(string name);

        IEnumerable<T> ResolveAll<T>();
        IEnumerable<T> ResolveAll<T>(string name);

        void RegisterInstance(Type type,object instance);
        void RegisterInstance(Type type, string name,object instance);
        void RegisterInstance<T>(T instance);
        void RegisterInstance<T>(string name,T instance);

    }
}
