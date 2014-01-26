#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Ninject;
using Ninject.Activation.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.CMS.Common.Runtime.Dependency.Ninject.InRequestScope
{
    public sealed class OnePerRequestHttpModule : IHttpModule
    {
        // Methods
        public OnePerRequestHttpModule()
        {
            this.ReleaseScopeAtRequestEnd = true;
        }

        public void DeactivateInstancesForCurrentHttpRequest()
        {
            if (this.ReleaseScopeAtRequestEnd)
            {
                HttpContext context = HttpContext.Current;
                var kernel = GetKernel();
                if (kernel != null)
                {
                    kernel.Components.Get<ICache>().Clear(context);
                }
            }
        }

        private IKernel GetKernel()
        {
            if (Kooboo.CMS.Common.Runtime.EngineContext.Current is NinjectEngine)
            {
                return (IKernel)((ContainerManager)((NinjectEngine)Kooboo.CMS.Common.Runtime.EngineContext.Current).ContainerManager).Container;
            }
            return null;
        }

        public void Dispose()
        {
        }

        public void Init(HttpApplication application)
        {
            application.EndRequest += delegate(object o, EventArgs e)
            {
                this.DeactivateInstancesForCurrentHttpRequest();
            };
        }

        // Properties
        public bool ReleaseScopeAtRequestEnd { get; set; }
    }
}
