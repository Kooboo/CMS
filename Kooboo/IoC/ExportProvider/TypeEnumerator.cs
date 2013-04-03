using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;

namespace Kooboo.IoC
{
    internal class TypeEnumerator:ITypeEnumerator
    {

        public IEnumerable<Type> GetTypes()
        {
            IEnumerable<Type> typesSoFar = Type.EmptyTypes;
            ICollection assemblies = System.Web.Compilation.BuildManager.GetReferencedAssemblies();
            foreach (Assembly assembly in assemblies)
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
                typesSoFar = typesSoFar.Concat(typesInAsm);
            }

            return typesSoFar;
        }
    }
}
