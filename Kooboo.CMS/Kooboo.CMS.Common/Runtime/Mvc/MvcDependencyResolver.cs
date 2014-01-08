#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Common.Runtime.Mvc
{
    public class MvcDependencyResolver : IDependencyResolver
    {
        IEngine _engine;
        IDependencyResolver _innerResolver;

        public MvcDependencyResolver(IEngine engine, IDependencyResolver innerResolver)
        {
            _engine = engine;
            _innerResolver = innerResolver;
        }

        public object GetService(Type serviceType)
        {
            var service = _engine.TryResolve(serviceType);
            if (service == null)
            {
                service = _innerResolver.GetService(serviceType);
            }
            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var services = _engine.ResolveAll(serviceType);
            if (services == null)
            {
                services = _innerResolver.GetServices(serviceType);
            }
            return services;
        }
    }
}
