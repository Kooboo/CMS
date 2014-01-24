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
using System.Web;
using Ninject;
using Ninject.Syntax;
using NinjectParameters = Ninject.Parameters;
using Kooboo.CMS.Common.Runtime.Dependency.Ninject.InRequestScope;
namespace Kooboo.CMS.Common.Runtime.Dependency.Ninject
{
    /// <summary>
    /// 加入Container Manager是为了减少其它程序在做注入的时候减少对Ninject的依赖
    /// 有了这个类以后，未来的扩展程序集在注册组件时就不用引用Ninject，对它形成依赖。
    /// </summary>
    public class ContainerManager : IContainerManager
    {
        private class KernelWrapper : StandardKernel
        {

            #region AddResolvingObserver
            private IList<IResolvingObserver> _resolvingObjservers = new List<IResolvingObserver>();
            public void AddResolvingObserver(IResolvingObserver observer)
            {
                _resolvingObjservers.Add(observer);
                _resolvingObjservers = _resolvingObjservers.OrderBy(it => it.Order).ToList();
            }

            private object OnResolved(object resolvedObject)
            {
                if (_resolvingObjservers.Count > 0)
                {
                    foreach (var item in _resolvingObjservers)
                    {
                        resolvedObject = item.OnResolved(resolvedObject);
                    }
                }
                return resolvedObject;
            }
            #endregion
            public override IEnumerable<object> Resolve(global::Ninject.Activation.IRequest request)
            {
                return base.Resolve(request).Select(it => OnResolved(it));
            }
        }

        #region .ctor
        private KernelWrapper _container;

        public ContainerManager()
        {
            _container = new KernelWrapper();

            _container.Settings.Set("InjectAttribute", typeof(InjectAttribute));
        }

        /// <summary>
        /// Ninject
        /// </summary>
        public IKernel Container
        {
            get { return _container; }
        }
        #endregion

        #region AddComponent

        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="lifeStyle">The life style.</param>
        public virtual void AddComponent<TService>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Transient)
        {
            AddComponent<TService, TService>(key, lifeStyle);
        }

        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="lifeStyle">The life style.</param>
        public virtual void AddComponent(Type service, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Transient)
        {
            AddComponent(service, service, key, lifeStyle);
        }

        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="lifeStyle">The life style.</param>
        public virtual void AddComponent<TService, TImplementation>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Transient)
        {
            AddComponent(typeof(TService), typeof(TImplementation), key, lifeStyle);
        }

        public virtual void AddComponent(Type service, Type implementation, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Transient, params Parameter[] parameters)
        {
            var binding = _container.Bind(service).To(implementation);
            if (parameters != null)
            {
                var ninjectParamters = ConvertParameters(parameters);
                foreach (var parameter in ninjectParamters)
                {
                    binding.WithParameter(parameter);
                }
            }
            binding.PerLifeStyle(lifeStyle).MapKey(key).ReplaceExisting(service);
        }

        public virtual void AddComponentInstance<TService>(object instance, string key = "")
        {
            AddComponentInstance(typeof(TService), instance, key);
        }
        public virtual void AddComponentInstance(object instance, string key = "")
        {
            AddComponentInstance(instance.GetType(), instance, key);
        }
        public virtual void AddComponentInstance(Type service, object instance, string key = "")
        {
            _container.Bind(service).ToConstant(instance).MapKey(key).ReplaceExisting(service);
        }
        #endregion

        #region ConvertParameters
        private static NinjectParameters.IParameter[] ConvertParameters(Parameter[] parameters)
        {
            if (parameters == null)
            {
                return null;
            }
            return parameters.Select(it => new NinjectParameters.ConstructorArgument(it.Name, (context) => it.ValueCallback())).ToArray();
        }
        #endregion
        #region Resolve
        public virtual T Resolve<T>(string key = "", params Parameter[] parameters) where T : class
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.Get<T>(ConvertParameters(parameters));
            }
            return _container.Get<T>(key, ConvertParameters(parameters));
        }

        public virtual object Resolve(Type type, string key = "", params Parameter[] parameters)
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.Get(type, ConvertParameters(parameters));
            }
            return _container.Get(type, key, ConvertParameters(parameters));
        }
        #endregion

        #region ResolveAll
        public virtual T[] ResolveAll<T>(string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.GetAll<T>().ToArray();
            }
            return _container.GetAll<T>(key).ToArray();
        }
        public virtual object[] ResolveAll(Type type, string key = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.GetAll(type).ToArray();
            }
            return _container.GetAll(type, key).ToArray();
        }
        #endregion

        #region TryResolve
        public virtual T TryResolve<T>(string key = "", params Parameter[] parameters)
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.TryGet<T>(ConvertParameters(parameters));
            }
            return _container.TryGet<T>(key, ConvertParameters(parameters));
        }

        public virtual object TryResolve(Type type, string key = "", params Parameter[] parameters)
        {
            if (string.IsNullOrEmpty(key))
            {
                return _container.TryGet(type, ConvertParameters(parameters));
            }
            return _container.TryGet(type, key, ConvertParameters(parameters));
        }

        #endregion

        #region ResolveUnregistered
        public virtual T ResolveUnregistered<T>() where T : class
        {
            return ResolveUnregistered(typeof(T)) as T;
        }

        public virtual object ResolveUnregistered(Type type)
        {
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {

                var parameters = constructor.GetParameters();
                var parameterInstances = new List<object>();
                foreach (var parameter in parameters)
                {
                    var service = Resolve(parameter.ParameterType);
                    if (service == null)
                        parameterInstances.Add(service);
                }
                return Activator.CreateInstance(type, parameterInstances.ToArray());


            }
            throw new Exception("No contructor was found that had all the dependencies satisfied.");
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            if (this._container != null && !this._container.IsDisposed)
            {
                this._container.Dispose();
            }

            this._container = null;
        }
        #endregion

        #region AddResolvingObserver
        public void AddResolvingObserver(IResolvingObserver observer)
        {
            _container.AddResolvingObserver(observer);
        }
        #endregion


        #region InjectProperties
        public void InjectProperties(object instance)
        {
            this._container.Inject(instance);
        }
        #endregion
    }

}
