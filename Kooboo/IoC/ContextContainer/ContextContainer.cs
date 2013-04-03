using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Kooboo.IoC
{
    public class ContextContainer:IDisposable
    {
        ContextContainer(ILifetimeManager liftetime)
        {
            this.Lifetime = liftetime;
        }
        
        ILifetimeManager Lifetime
        {
            get;
            set;
        }

        public static ContextContainer Current
        {
            get
            {
                return new ContextContainer(LifetimeManagers.Default());
            }
        }

        static ContextContainer _Shared = new ContextContainer(LifetimeManagers.Shared);
        public static ContextContainer Shared
        {
            get
            {
                return _Shared;
            }
        }

        public T Resolve<T>()
        {
            var instance = this.Lifetime.Resolve<T>();

            if (instance == null)
            {
                instance = ObjectContainer.CreateInstance<T>();
                this.Lifetime.RegisterInstance<T>(instance);
            }

            return instance;
        }

        public T Resolve<T>(string name)
        {
            var instance = this.Lifetime.Resolve<T>(name);

            if (instance == null)
            {
                instance = ObjectContainer.CreateInstance<T>();
                this.Lifetime.RegisterInstance<T>(name,instance);
            }

            return instance;
        }

        public T Resolve<T>(string name, string contractName)
        {
            var instance = this.Lifetime.Resolve<T>(name);

            if (instance == null)
            {
                instance = ObjectContainer.CreateInstance<T>(contractName);
                this.Lifetime.RegisterInstance<T>(name, instance);
            }

            return instance;
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            IEnumerable<T> enumerator = this.Lifetime.ResolveAll<T>();

            if (enumerator == null)
            {

                enumerator = ObjectContainer.CreateInstances<T>();
                foreach (var i in enumerator)
                {
                    this.Lifetime.RegisterInstance<T>(i);
                }
            }

            return enumerator;
        }

        public IEnumerable<T> ResolveAll<T>(string name)
        {
            IEnumerable<T> enumerator = this.Lifetime.ResolveAll<T>(name);

            if (enumerator == null)
            {
                enumerator = ObjectContainer.CreateInstances<T>();
                foreach (var i in enumerator)
                {
                    this.Lifetime.RegisterInstance<T>(name,i);
                }
            }

            return enumerator;
        }

        public IEnumerable<T> ResolveAll<T>(string name, string contractName)
        {
            IEnumerable<T> enumerator = this.Lifetime.ResolveAll<T>(name);

            if (enumerator == null)
            {
                enumerator = ObjectContainer.CreateInstances<T>(contractName);
                foreach (var i in enumerator)
                {
                    this.Lifetime.RegisterInstance<T>(name, i);
                }
            }

            return enumerator;
        }

        public object Resolve(Type type)
        {
            var instance = this.Lifetime.Resolve(type);

            if (instance == null)
            {
                instance = ObjectContainer.CreateInstance(type);
                this.Lifetime.RegisterInstance(type,instance);
            }

            return instance;
        }

        public object Resolve(Type type,string name)
        {
            var instance = this.Lifetime.Resolve(type, name);

            if (instance == null)
            {
                instance = ObjectContainer.CreateInstance(type);
                this.Lifetime.RegisterInstance(type,name,instance);
            }

            return instance;
        }

        public object Resolve(Type type, string name,string contractName)
        {
            var instance = this.Lifetime.Resolve(type, name);

            if (instance == null)
            {
                instance = ObjectContainer.CreateInstance(type, contractName);
                this.Lifetime.RegisterInstance(type, name, instance);
            }

            return instance;
        }

        public IEnumerable ResolveAll(Type type)
        {
            IEnumerable enumerator = this.Lifetime.ResolveAll(type);

            if (enumerator == null)
            {
                enumerator = ObjectContainer.CreateInstances(type);
                foreach (var i in enumerator)
                {
                    this.Lifetime.RegisterInstance(type,i);
                }
            }

            return enumerator;
        }

        public IEnumerable ResolveAll(Type type, string name)
        {
            IEnumerable enumerator = this.Lifetime.ResolveAll(type,name);

            if (enumerator == null)
            {
                enumerator = ObjectContainer.CreateInstances(type);
                foreach (var i in enumerator)
                {
                    this.Lifetime.RegisterInstance(type, name, i);
                }
            }

            return enumerator;
        }

        public IEnumerable ResolveAll(Type type, string name, string contractName)
        {
            IEnumerable enumerator = this.Lifetime.ResolveAll(type, name);

            if (enumerator == null)
            {
                enumerator = ObjectContainer.CreateInstances(type, contractName);
                foreach (var i in enumerator)
                {
                    this.Lifetime.RegisterInstance(type, name, i);
                }
            }

            return enumerator;
        }

        public void RegisterInstance(Type type, object instance)
        {
            this.Lifetime.RegisterInstance(type, instance);
        }

        public void RegisterInstance(Type type, string name, object instance)
        {
            this.Lifetime.RegisterInstance(type, name, instance);
        }
        
        public void RegisterInstance<T>(T instance)
        {
            this.Lifetime.RegisterInstance<T>(instance);
        }
        public void RegisterInstance<T>(string name, T instance)
        {
            this.Lifetime.RegisterInstance<T>(name, instance);
        }

        #region Dispose


        ~ContextContainer()
        {
            Dispose(false); 
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //free managed objects
                if (this.Lifetime != null)
                {
                    this.Lifetime.Dispose();
                    this.Lifetime = null;
                }
            }

            //free unmanaged objectss
         
            if (disposing)
            {
                //remove me from finalization list
                GC.SuppressFinalize(this);   
            }
        }

 
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
