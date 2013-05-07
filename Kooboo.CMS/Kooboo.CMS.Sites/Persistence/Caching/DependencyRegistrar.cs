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
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.Caching
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
            var htmlBlockProvider = new HtmlBlockProvider(containerManager.Resolve<IHtmlBlockProvider>());
            containerManager.AddComponentInstance(typeof(IHtmlBlockProvider), htmlBlockProvider);
            containerManager.AddComponentInstance(typeof(IProvider<HtmlBlock>), htmlBlockProvider);

            var layoutProvider = new LayoutProvider(containerManager.Resolve<ILayoutProvider>());
            containerManager.AddComponentInstance(typeof(ILayoutProvider), layoutProvider);
            containerManager.AddComponentInstance(typeof(IProvider<Layout>), layoutProvider);

            var pageProvider = new PageProvider(containerManager.Resolve<IPageProvider>());
            containerManager.AddComponentInstance(typeof(IPageProvider), pageProvider);
            containerManager.AddComponentInstance(typeof(IProvider<Page>), pageProvider);

            var siteProvider = new SiteProvider(containerManager.Resolve<ISiteProvider>());
            containerManager.AddComponentInstance(typeof(ISiteProvider), siteProvider);
            containerManager.AddComponentInstance(typeof(IProvider<Site>), siteProvider);

            var urlKeyMapProvider = new UrlKeyMapProvider(containerManager.Resolve<IUrlKeyMapProvider>());
            containerManager.AddComponentInstance(typeof(IUrlKeyMapProvider), urlKeyMapProvider);
            containerManager.AddComponentInstance(typeof(IProvider<UrlKeyMap>), urlKeyMapProvider);

            var viewProvider = new ViewProvider(containerManager.Resolve<IViewProvider>());
            containerManager.AddComponentInstance(typeof(IViewProvider), viewProvider);
            containerManager.AddComponentInstance(typeof(IProvider<Kooboo.CMS.Sites.Models.View>), viewProvider);


            var visitRuleSettingProvider = new VisitRuleSettingProvider(containerManager.Resolve<IABRuleSettingProvider>());
            containerManager.AddComponentInstance(typeof(IABRuleSettingProvider), visitRuleSettingProvider);
            containerManager.AddComponentInstance(typeof(IProvider<Kooboo.CMS.Sites.ABTest.ABRuleSetting>), visitRuleSettingProvider);

            var siteVisitRuleProvider = new SiteVisitRuleProvider(containerManager.Resolve<IABSiteSettingProvider>());
            containerManager.AddComponentInstance(typeof(IABSiteSettingProvider), siteVisitRuleProvider);
            containerManager.AddComponentInstance(typeof(IProvider<Kooboo.CMS.Sites.ABTest.ABSiteSetting>), siteVisitRuleProvider);

            var pageVisitRuleProvider = new PageVisitRuleProvider(containerManager.Resolve<IABPageSettingProvider>());
            containerManager.AddComponentInstance(typeof(IABPageSettingProvider), pageVisitRuleProvider);
            containerManager.AddComponentInstance(typeof(IProvider<Kooboo.CMS.Sites.ABTest.ABPageSetting>), pageVisitRuleProvider);

        }

        public int Order
        {
            get { return 100; }
        }
    }
}
