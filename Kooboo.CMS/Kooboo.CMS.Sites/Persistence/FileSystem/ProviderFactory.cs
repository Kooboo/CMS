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
using System.Collections;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProviderFactory))]
    public class ProviderFactory : IProviderFactory
    {

        public virtual T GetProvider<T>()
            where T : class
        {
            return EngineContext.Current.Resolve<T>();
        }

        public virtual void RegisterProvider<T>(T provider)
            where T : class
        {
            (EngineContext.Current).ContainerManager.AddComponentInstance<T>(provider);
        }
    }
}
