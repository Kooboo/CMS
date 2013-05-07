#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;

namespace Kooboo.CMS.Common.Runtime.Dependency
{
    /// <summary>
    /// Maps an attribute to the type it decorates.
    /// </summary>
    /// <typeparam name="T">The type of attribute.</typeparam>
    public class AttributeInfo<T>
    {
        /// <summary>An attribute retrieved from a type descriptor.</summary>
        public T Attribute { get; set; }

        /// <summary>The particular type the attribute was describing.</summary>
        public Type DecoratedType { get; set; }
    }
}
