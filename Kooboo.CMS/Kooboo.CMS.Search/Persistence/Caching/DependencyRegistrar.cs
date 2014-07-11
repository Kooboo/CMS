#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.Common.ObjectContainer;
using Kooboo.Common.ObjectContainer.Dependency;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Search.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Search.Persistence.Caching
{
    /// <summary>    
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        private class ResolvingObserver : IResolvingObserver
        {
            public int Order
            {
                get { return 1; }
            }

            public object OnResolved(object resolvedObject)
            {
                if (resolvedObject is ISearchSettingProvider)
                {
                    return new SearchSettingProvider((ISearchSettingProvider)resolvedObject);
                }

                return resolvedObject;
            }
        }

        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.AddResolvingObserver(new ResolvingObserver());
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
