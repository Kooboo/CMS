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
using System.IO;
using System.Reflection;

namespace Kooboo.CMS.Common.Runtime
{
    /// <summary>
    /// Classes implementing this interface provide information about types 
    /// to various services in the engine.
    /// </summary>
    public interface ITypeFinder
    {
        IList<Assembly> GetAssemblies();

        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType<T, TAssemblyAttribute>(bool onlyConcreteClasses = true) where TAssemblyAttribute : Attribute;

        IEnumerable<Assembly> FindAssembliesWithAttribute<T>();

        IEnumerable<Assembly> FindAssembliesWithAttribute<T>(IEnumerable<Assembly> assemblies);

        IEnumerable<Assembly> FindAssembliesWithAttribute<T>(DirectoryInfo assemblyPath);


    }
}
