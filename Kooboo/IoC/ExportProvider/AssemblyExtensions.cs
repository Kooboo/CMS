using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Kooboo.IoC
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> TryGetTypes(this Assembly assembly)
        {
            Type[] typesInAsm;
            try
            {
                typesInAsm = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                typesInAsm = ex.Types;
            }

            foreach (var type in typesInAsm)
            {
                yield return type;
            }
        }
    }
}
