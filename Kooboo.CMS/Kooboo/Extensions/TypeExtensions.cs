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

namespace Kooboo.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeExtensions
    {
        #region Fields
        readonly static List<Type> numericalTypes = new List<Type>(){            
            typeof(Int16),
            typeof(Int32),
            typeof(Int64),
            typeof(float),
            typeof(long),
            typeof(double),
            typeof(decimal)
        }; 
        #endregion
        #region Methods
        /// <summary>
        /// Gets the type name without version.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string AssemblyQualifiedNameWithoutVersion(this Type type)
        {
            string[] str = type.AssemblyQualifiedName.Split(',');
            return string.Format("{0},{1}", str[0], str[1]);
        }

        /// <summary>
        /// Gets all child types.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllChildTypes(this Type type)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.GlobalAssemblyCache)
                .ToList();
            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                try
                {
                    types.AddRange(assembly.GetTypes());
                }
                catch { }
            }
            var targetTypes = types.Where(p => type.IsAssignableFrom(p) && type != p);
            return targetTypes;
        }


        /// <summary>
        /// Determines whether [is numerical type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if [is numerical type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumericalType(this Type type)
        {
            return numericalTypes.Contains(type);
        }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }
            else
            {
                return null;
            }
        } 
        #endregion
    }
}

