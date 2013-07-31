#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Web;

using System.Diagnostics.Contracts;
using System.Reflection;
using System.Security;

namespace Kooboo
{
    //[SecurityCritical]
    public class CallContext : IDisposable
    {
        #region Fields
        [ThreadStatic]
        private static CallContext _current;
        internal readonly static string CONTEXT_CONTAINER = "__CONTEXT_CONTAINER__"; 
        #endregion

        #region .ctor
        /// <summary>
        /// Prevents a default instance of the <see cref="CallContext" /> class from being created.
        /// </summary>
        private CallContext()
        {
            this.RegisteredObjects = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
        }

        #endregion


        #region Properties
        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static CallContext Current
        {
            get
            {

                if (HttpContext.Current != null)
                {
                    var context = HttpContext.Current.Items[CONTEXT_CONTAINER] as CallContext;

                    if (context == null)
                    {
                        context = new CallContext();
                        HttpContext.Current.Items.Add(CONTEXT_CONTAINER, context);
                    }

                    return context;
                }
                else
                {
                    //var context = System.Runtime.Remoting.Messaging.CallContext.GetData(CONTEXT_CONTAINER) as CallContext;

                    //if (context == null)
                    //{
                    //    context = new CallContext();
                    //    System.Runtime.Remoting.Messaging.CallContext.SetData(CONTEXT_CONTAINER, context);
                    //}

                    //return context;

                    if (_current == null)
                    {
                        _current = new CallContext();
                    }
                    return _current;
                }
            }

        }

        #endregion

        #region Methods
        #region Context object
        IDictionary<string, object> RegisteredObjects
        {
            get;
            set;
        }

        public T GetObject<T>(string key)
        {
            object value = null;
            if (RegisteredObjects.TryGetValue(key, out value))
            {
                return (T)value;
            }
            return default(T);
        }
        public T GetObject<T>()
        {
            var key = GenerateKey(string.Empty, typeof(T));
            return GetObject<T>(key);
        }

        public void RegisterObject<T>(T obj)
        {
            var key = GenerateKey(string.Empty, typeof(T));
            RegisteredObjects.Add(key, obj);
        }

        public void RegisterObject<T>(string key, T obj)
        {
            RegisteredObjects[key] = obj;
        }

        public void RemoveObject(string key)
        {
            RegisteredObjects.Remove(key);
        }

        #endregion

        #region Resolve
        /// <summary>
        /// Resolves this instance.
        /// The object will be cached in the same call context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            string key = GenerateKey(string.Empty, typeof(T));
            var obj = GetObject<T>(key);
            if (obj == null)
            {
                obj = Activator.CreateInstance<T>();
                //obj = ComponentContainer.Container.GetExportedValue<T>();

                RegisterObject(key, obj);
            }

            return (T)obj;

        }

        ///// <summary>
        ///// Resolves the specified contract name.
        ///// The object will be cached in the same call context.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="contractName">Name of the contract.</param>
        ///// <returns></returns>
        //public T Resolve<T>(string contractName)
        //{
        //    string key = GenerateKey(contractName, typeof(T));
        //    var obj = GetObject(key);
        //    if (obj == null)
        //    {
        //        obj = ComponentContainer.Container.GetExportedValue<T>(contractName);
        //        SetObject(key, obj);
        //    }

        //    return (T)obj;
        //}

        //public void RegisterInstance<T>(T instance, string contractName = "")
        //{
        //    string key = GenerateKey(contractName, typeof(T));
        //    this.SetObject(key, instance);
        //}

        #endregion

        /// <summary>
        /// Generates the key.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string GenerateKey(string name, Type type)
        {
            if (type == null) throw new ArgumentNullException("Type");
            if (name == null)
            {
                name = "NULL";
            }

            return name + "_" + type.ToString();
        }


        #region IDisposable Members

        public void Dispose()
        {
            if (this.RegisteredObjects == null)
            {
                foreach (var o in this.RegisteredObjects.Values)
                {
                    if (o is IDisposable)
                    {
                        ((IDisposable)o).Dispose();
                    }
                }
            }
            this.RegisteredObjects = null;
        }

        #endregion 
        #endregion
    }

}
