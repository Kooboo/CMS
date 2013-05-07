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
using System.Threading.Tasks;

namespace Kooboo
{
    public class TypeActivator
    {
        public static Func<Type, object> CreateInstanceMethod = (type) =>
        {
            return Activator.CreateInstance(type);
        };
        public static object CreateInstance(Type type)
        {
            return CreateInstanceMethod(type);
        }
    }
}
