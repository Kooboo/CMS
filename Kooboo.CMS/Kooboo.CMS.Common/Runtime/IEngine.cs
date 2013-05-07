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
        T Resolve<T>() where T : class;

        T Resolve<T>(string name) where T : class;

        object Resolve(Type type);

        object Resolve(Type type, string name);
        #endregion

        #region TryResolve
        T TryResolve<T>() where T : class;

        T TryResolve<T>(string name) where T : class;

        object TryResolve(Type type);

        object TryResolve(Type type, string name);
        #endregion

        #region ResolveAll
        IEnumerable<object> ResolveAll(Type serviceType);

        IEnumerable<T> ResolveAll<T>();
        #endregion
    }
}
