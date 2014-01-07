#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Caching
{
    /// <summary>
    /// 用于注册包装Provider的 CacheProvider
    /// 执行时序会相对较晚，因为需要对任何人的注册进行包装。
    /// 如果开发人员不希望被包装的话，那就装注册时序设置为大于100
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
                if (resolvedObject is IMediaFolderProvider)
                {
                    return new MediaFolderProvider((IMediaFolderProvider)resolvedObject);
                }
                if (resolvedObject is IRepositoryProvider)
                {
                    return new RepositoryProvider((IRepositoryProvider)resolvedObject);
                }
                if (resolvedObject is ISchemaProvider)
                {
                    return new SchemaProvider((ISchemaProvider)resolvedObject);
                }
                if (resolvedObject is ITextFolderProvider)
                {
                    return new TextFolderProvider((ITextFolderProvider)resolvedObject);
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
