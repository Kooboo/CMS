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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.AzureBlobService
{
    public class DependencyRegistrar : Kooboo.Common.ObjectContainer.Dependency.IDependencyRegistrar
    {
        private class ResolvingObserver : IResolvingObserver
        {
            public int Order
            {
                get { return 1; }
            }

            public object OnResolved(object resolvedObject)
            {
                if (resolvedObject is IRepositoryProvider)
                {
                    return new RepositoryProvider((IRepositoryProvider)resolvedObject);
                }


                return resolvedObject;
            }
        }
        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.AddResolvingObserver(new ResolvingObserver());
        }

        /// <summary>
        /// 在TextContent provider初始化之后再做初始化
        /// </summary>
        public int Order
        {
            get { return 2; }
        }
    }
}
