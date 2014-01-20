#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;

namespace Kooboo.CMS.Common.Runtime
{
    public interface IEngine
    {
        #region Initialize
        void Initialize();
        #endregion

        #region ContainerManager
        IContainerManager ContainerManager { get; }
        #endregion

        #region Resolve
        T Resolve<T>(params Parameter[] parameters) where T : class;

        T Resolve<T>(string name, params Parameter[] parameters) where T : class;

        object Resolve(Type type, params Parameter[] parameters);

        object Resolve(Type type, string name, params Parameter[] parameters);
        #endregion

        #region TryResolve
        T TryResolve<T>(params Parameter[] parameters) where T : class;

        T TryResolve<T>(string name, params Parameter[] parameters) where T : class;

        object TryResolve(Type type, params Parameter[] parameters);

        object TryResolve(Type type, string name, params Parameter[] parameters);
        #endregion

        #region ResolveAll
        IEnumerable<object> ResolveAll(Type serviceType);

        IEnumerable<T> ResolveAll<T>();
        #endregion

        #region InjectProperties
        void InjectProperties(object instance);
        #endregion
    }
}
