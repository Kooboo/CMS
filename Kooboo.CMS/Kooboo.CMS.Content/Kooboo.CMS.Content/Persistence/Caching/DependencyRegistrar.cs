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
        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            //
            var mediaFolderProvider = new MediaFolderProvider(containerManager.Resolve<IMediaFolderProvider>());
            containerManager.AddComponentInstance(typeof(IMediaFolderProvider), mediaFolderProvider);
            containerManager.AddComponentInstance(typeof(IProvider<MediaFolder>), mediaFolderProvider);

            var repositoryProvider = new RepositoryProvider(containerManager.Resolve<IRepositoryProvider>());
            containerManager.AddComponentInstance(typeof(IRepositoryProvider), repositoryProvider);
            containerManager.AddComponentInstance(typeof(IProvider<Repository>), repositoryProvider);

            var schemaProvider = new SchemaProvider(containerManager.Resolve<ISchemaProvider>());
            containerManager.AddComponentInstance(typeof(ISchemaProvider), schemaProvider);
            containerManager.AddComponentInstance(typeof(IProvider<Schema>), schemaProvider);

            var textFolderProvider = new TextFolderProvider(containerManager.Resolve<ITextFolderProvider>());
            containerManager.AddComponentInstance(typeof(ITextFolderProvider), textFolderProvider);
            containerManager.AddComponentInstance(typeof(IProvider<TextFolder>), textFolderProvider);


        }

        public int Order
        {
            get { return 100; }
        }
    }
}
