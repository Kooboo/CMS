#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence
{
    public static class Providers
    {
        public static IProviderFactory DefaultProviderFactory
        {
            get
            {
                return EngineContext.Current.Resolve<IProviderFactory>();
            }
            set
            {
                EngineContext.Current.ContainerManager.AddComponentInstance<IProviderFactory>(value);
            }
        }

        public static IRepositoryProvider RepositoryProvider
        {
            get
            {
                return DefaultProviderFactory.GetProvider<IRepositoryProvider>();
            }
        }
    }
}
