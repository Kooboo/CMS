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
using Ninject;
using Ninject.Syntax;
namespace Kooboo.CMS.Common.Runtime.Dependency.Ninject
{
    /// <summary>
    /// Registers service in the inversion of container upon start.
    /// </summary>
    public class DependencyAttributeRegistrator
    {
        private readonly ITypeFinder _finder;
        private readonly IContainerManager _containerManager;

        public DependencyAttributeRegistrator(ITypeFinder finder, IContainerManager containerManager)
        {
            this._finder = finder;
            this._containerManager = containerManager;
        }

        public virtual IEnumerable<AttributeInfo<DependencyAttribute>> FindServices()
        {
            foreach (Type type in _finder.FindClassesOfType<object>())
            {
                var attributes = type.GetCustomAttributes(typeof(DependencyAttribute), false);
                foreach (DependencyAttribute attribute in attributes)
                {
                    yield return new AttributeInfo<DependencyAttribute> { Attribute = attribute, DecoratedType = type };
                }
            }
        }

        public virtual void RegisterServices()
        {
            this.RegisterServices(this.FindServices());
        }
        public virtual void RegisterServices(IEnumerable<AttributeInfo<DependencyAttribute>> services)
        {
            foreach (var info in services)
            {
                Type serviceType = info.Attribute.ServiceType ?? info.DecoratedType;
                _containerManager.AddComponent(serviceType, info.DecoratedType, info.Attribute.Key, info.Attribute.LifeStyle);
            }
        }

        public virtual IEnumerable<AttributeInfo<DependencyAttribute>> FilterServices(IEnumerable<AttributeInfo<DependencyAttribute>> services, params string[] configurationKeys)
        {
            return services.Where(s => s.Attribute.Configuration == null || configurationKeys.Contains(s.Attribute.Configuration));
        }
    }
}
