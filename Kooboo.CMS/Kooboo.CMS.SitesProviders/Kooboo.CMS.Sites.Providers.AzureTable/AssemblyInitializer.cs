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
using System.IO;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.ObjectContainer.Dependency;
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Sites.Globalization;
namespace Kooboo.CMS.Sites.Providers.AzureTable
{
    public class AssemblyInitializer : Kooboo.Common.ObjectContainer.Dependency.IDependencyRegistrar
    {

        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.AddComponent<IElementRepositoryFactory, LabelProvider.RepositoryFactory>();
            containerManager.AddComponent<IPageProvider, PageProvider.PageProvider>();
            containerManager.AddComponent<IHtmlBlockProvider, HtmlBlockProvider.HtmlBlockProvider>();
            containerManager.AddComponent<IUserProvider, UserProvider.UserProvider>();
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
