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
using System.Data.Entity;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Sites.Globalization;
namespace Kooboo.CMS.Sites.Providers.SqlServer
{
    public class AssemblyInitializer : Kooboo.CMS.Common.Runtime.Dependency.IDependencyRegistrar
    {

        //public static void ResetProviders()
        //{
        //    var dbContext = SiteDbContext.CreateDbContext();
        //    dbContext.Database.CreateIfNotExists();

        //    Kooboo.CMS.Sites.Globalization.DefaultRepositoryFactory.Instance = new LabelProvider.RepositoryFactory();

        //    var providerFactory = Kooboo.CMS.Sites.Persistence.Providers.ProviderFactory;
        //    providerFactory.RegisterProvider<IPageProvider>(new PageProvider.PageProvider());
        //    providerFactory.RegisterProvider<IHtmlBlockProvider>(new HtmlBlockProvider.HtmlBlockProvider());
        //    providerFactory.RegisterProvider<IUserProvider>(new UserProvider.UserProvider());

        //    // VersionManager.RegisterVersionLogger<Page>(new PageProvider.PageProvider.PageVersionLogger());
        //    //VersionManager.RegisterVersionLogger<HtmlBlock>(new HtmlBlockProvider.HtmlBlockProvider.HtmlBlockVersionLogger());
        //}

        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            //Kooboo.CMS.Sites.Globalization.DefaultRepositoryFactory.Instance = new LabelProvider.RepositoryFactory();
            containerManager.AddComponent<IElementRepositoryFactory, LabelProvider.RepositoryFactory>();
            containerManager.AddComponent<IPageProvider, PageProvider.PageProvider>();
            containerManager.AddComponent<IHtmlBlockProvider, HtmlBlockProvider.HtmlBlockProvider>();
            containerManager.AddComponent<IUserProvider, UserProvider.UserProvider>();

            var dbContext = SiteDbContext.CreateDbContext();
            dbContext.Database.CreateIfNotExists();

        }

        public int Order
        {
            get { return 101; }
        }
    }
}
