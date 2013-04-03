using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Kooboo.IoC
{
    class SharedLifetimeManager:ILifetimeManager
    {
        static ObjectPool Items = new ObjectPool();

        #region ILifetimeManager Members

        public object Resolve(Type type)
        {
            var key = new ObjectKey(type, null);
            return ResolveInstances(key).FirstOrDefault();
        }

        public object Resolve(Type type, string name)
        {
            var key = new ObjectKey(type, name);
            return ResolveInstances(key).FirstOrDefault();
        }

        public IEnumerable ResolveAll(Type type)
        {
            var key = new ObjectKey(type, null);
            return ResolveInstances(key);
        }

        public IEnumerable ResolveAll(Type type, string name)
        {
            var key = new ObjectKey(type, name);
            return ResolveInstances(key);
        }

        public T Resolve<T>()
        {

            var key = new ObjectKey(typeof(T), null);
            return (T)ResolveInstances(key).FirstOrDefault();
        }

        public T Resolve<T>(string name)
        {

            var key = new ObjectKey(typeof(T), name);
            return (T)ResolveInstances(key).FirstOrDefault();
        }

        public IEnumerable<T> ResolveAll<T>()
        {

            var key = new ObjectKey(typeof(T), null);
            foreach (var item in ResolveInstances(key))
            {
                yield return (T)item;
            }
        }

        public IEnumerable<T> ResolveAll<T>(string name)
        {
            var key = new ObjectKey(typeof(T), name);
            foreach (var item in ResolveInstances(key))
            {
                yield return (T)item;
            }
        }

        public void RegisterInstance(Type type, object instance)
        {
            this.RegisterInstance(type,null, instance);
        }

        public void RegisterInstance(Type type, string name, object instance)
        {
            var key = new ObjectKey(type, name);
            this.RegisterInstance(key, instance);
        }

        public void RegisterInstance<T>(T instance)
        {

            this.RegisterInstance<T>(null, instance);
        }

        public void RegisterInstance<T>(string name, T instance)
        {
            this.RegisterInstance(typeof(T),name,instance);
        }

        #endregion

        private List<object> ResolveInstances(ObjectKey key)
        {
            var list = Items[key];

            if (list == null)
            {
                lock (key)
                {
                    list = Items[key];
                    if (list == null)
                    {
                        list = new List<object>();
                        foreach (var instance in ObjectContainer.CreateInstances(key.Type))
                        {
                            list.Add(instance);
                        }

                        Items.AddRange(key, list);
                    }
                }

            }

            return Items[key];
        }

        private void RegisterInstance(ObjectKey key, object instance)
        {
            var list = Items[key];
            if (list == null)
            {
                lock (key)
                {
                    list = Items[key];
                    if (list == null)
                    {
                        list = new List<object>();
                        list.Add(instance);
                        Items.AddRange(key, list);
                    }
                }
            }

            list.Add(instance);
        }

              
        #region Dispose


        ~SharedLifetimeManager()
        {
            Dispose(false); 
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //free managed objects               
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
