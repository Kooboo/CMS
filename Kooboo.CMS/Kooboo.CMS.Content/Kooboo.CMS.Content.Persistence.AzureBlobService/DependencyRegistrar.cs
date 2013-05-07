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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.AzureBlobService
{
    public class DependencyRegistrar : Kooboo.CMS.Common.Runtime.Dependency.IDependencyRegistrar
    {
        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.AddComponentInstance<IRepositoryProvider>(new RepositoryProvider(containerManager.Resolve<IRepositoryProvider>()));
            containerManager.AddComponent<IMediaFolderProvider, MediaFolderProvider>();
            containerManager.AddComponent<IMediaContentProvider, MediaContentProvider>();
            containerManager.AddComponent<ITextContentFileProvider, TextContentFileProvider>();
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
