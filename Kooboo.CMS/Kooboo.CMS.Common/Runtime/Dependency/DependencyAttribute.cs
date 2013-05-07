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
    /// Markes a service that is registered in automatically registered in inversion of control container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependencyAttribute : Attribute
    {
        public DependencyAttribute(ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            LifeStyle = lifeStyle;
        }

        public DependencyAttribute(Type serviceType, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
        {
            LifeStyle = lifeStyle;
            ServiceType = serviceType;
        }

        /// <summary>The type of service the attributed class represents.</summary>
        public Type ServiceType { get; protected set; }

        public ComponentLifeStyle LifeStyle { get; protected set; }

        /// <summary>Optional key to associate with the service.</summary>
        public string Key { get; set; }

        /// <summary>Configurations for which this service is registered.</summary>
        public string Configuration { get; set; }
    }
}
