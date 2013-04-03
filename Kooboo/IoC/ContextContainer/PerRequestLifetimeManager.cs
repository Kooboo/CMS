using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Web;
using System.Collections;

namespace Kooboo.IoC
{
    public class PerRequestLifetimeManager : ILifetimeManager
    {

        public PerRequestLifetimeManager()
            : this(null)
        {

        }

        public PerRequestLifetimeManager(IRequestContext requestContext)
        {
            this.RequestContext = requestContext ?? new WebReqeustContext();
        }


        IRequestContext RequestContext
        {
            get;
            set;
        }

        static readonly object ObjectPollId = new object();
      
        ObjectPool ObjectPool
        {
            get
            {
                if (this.RequestContext.Items.Contains(ObjectPollId))
                {
                    return this.RequestContext.Items[ObjectPollId] as ObjectPool;
                }
                else
                {
                    var pool = new ObjectPool();
                    this.RequestContext.Items.Add(ObjectPollId, pool);
                    return pool;
                }
            }
        }

        private IEnumerable<object> ResolveInstances(ObjectKey key)
        {
            var list = this.ObjectPool[key];

            if (list == null)
            {
                list = new List<object>();
                foreach (var instance in ObjectContainer.CreateInstances(key.Type))
                {
                    list.Add(instance);
                }
                this.ObjectPool.AddRange(key, list);
            }


            return list;
        }

        #region ILifetimeManager Members

        public object Resolve(Type type)
        {
            var key = new ObjectKey(type,null);

            return ResolveInstances(key).FirstOrDefault();
        }

        public object Resolve(Type type, string name)
        {
            var key = new ObjectKey(type, name );

            return ResolveInstances(key).FirstOrDefault();
        }

        public IEnumerable ResolveAll(Type type)
        {
            var key = new ObjectKey(type,null);

            return ResolveInstances(key);
        }


        public IEnumerable ResolveAll(Type type, string name)
        {
            var key = new ObjectKey(type, name);

            return this.ResolveInstances(key);
        }

        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }

        public T Resolve<T>(string name)
        {
            return (T)this.Resolve(typeof(T), name);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            foreach (var i in this.ResolveAll(typeof(T)))
            {
                yield return (T)i;
            }
        }

        public IEnumerable<T> ResolveAll<T>(string name)
        {
            foreach (var i in this.ResolveAll(typeof(T)))
            {
                yield return (T)i;
            }
        }

        public void RegisterInstance(Type type, object instance)
        {
            var key = new ObjectKey(type,null);
            this.ObjectPool.Add(key, instance);
        }

        public void RegisterInstance(Type type, string name, object instance)
        {
            var key = new ObjectKey(type,name);
            this.ObjectPool.Add(key, instance);
        }

        public void RegisterInstance<T>(T instance)
        {
            this.RegisterInstance(typeof(T), instance);
        }

        public void RegisterInstance<T>(string name, T instance)
        {
            this.RegisterInstance(typeof(T), name, instance);
        }

        #endregion

        #region Dispose
        ~PerRequestLifetimeManager()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ObjectPool.Dispose();
            }

            //free unmanaged objects

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
