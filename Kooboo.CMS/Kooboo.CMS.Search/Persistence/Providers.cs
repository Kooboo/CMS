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
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Common.Infrastructure;

namespace Kooboo.CMS.Search.Persistence
{
    public static class Providers
    {


        private static IProviderFactory defaultProviderFactory;
        public static IProviderFactory DefaultProviderFactory
        {
            get
            {
                return EngineContext.Current.Resolve<IProviderFactory>("Kooboo.CMS.Search");
            }
            set
            {
                ((DefaultEngine)EngineContext.Current).ContainerManager.AddComponentInstance<IProviderFactory>(value, "Kooboo.CMS.Search");
            }
        }

        public static ISearchSettingProvider SearchSettingProvider
        {
            get
            {
                return DefaultProviderFactory.GetProvider<ISearchSettingProvider>();
            }
        }
        public static ILastActionProvider LastActionProvider
        {
            get
            {
                return DefaultProviderFactory.GetProvider<ILastActionProvider>();
            }
        }
    }
}
