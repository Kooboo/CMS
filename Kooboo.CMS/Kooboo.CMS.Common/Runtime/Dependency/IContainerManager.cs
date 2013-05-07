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

namespace Kooboo.CMS.Common.Runtime.Dependency
{
    public interface IContainerManager : IDisposable
    {
        #region AddComponent
        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="lifeStyle">The life style.</param>
        void AddComponent<TService>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton);

        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="key">The key.</param>
        /// <param name="lifeStyle">The life style.</param>
        void AddComponent(Type service, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton);

        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="lifeStyle">The life style.</param>
        void AddComponent<TService, TImplementation>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton);

        void AddComponent(Type service, Type implementation, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton);

        void AddComponentInstance<TService>(object instance, string key = "");

        void AddComponentInstance(object instance, string key = "");

        void AddComponentInstance(Type service, object instance, string key = ""); 
        #endregion

        #region Resolve
        T Resolve<T>(string key = "") where T : class;

        object Resolve(Type type, string key = ""); 
        #endregion

        #region ResolveAll
        T[] ResolveAll<T>(string key = "");
        object[] ResolveAll(Type type, string key = "");
        #endregion

        #region TryResolve
        T TryResolve<T>(string key = "");

        object TryResolve(Type type, string key = "");

        #endregion

        #region ResolveUnregistered
        T ResolveUnregistered<T>() where T : class;

        object ResolveUnregistered(Type type); 
        #endregion
    }
}
