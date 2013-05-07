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
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Common.Infrastructure;

namespace Kooboo.CMS.Search.Persistence.Default
{
    [Kooboo.CMS.Common.Infrastructure.DependencyManagement.Dependency(typeof(IProviderFactory), Key = "Kooboo.CMS.Search")]
    public class ProviderFactory : IProviderFactory
    {
        #region IProviderFactory Members

        public string Name
        {
            get { return "Default"; }
        }

        public T GetProvider<T>()
            where T : class
        {
            return EngineContext.Current.Resolve<T>();
        }

        #endregion
        public void RegisterProvider<T>(T provider)
            where T : class
        {
            ((DefaultEngine)EngineContext.Current).ContainerManager.AddComponentInstance<T>(provider);
        }
    }
}
