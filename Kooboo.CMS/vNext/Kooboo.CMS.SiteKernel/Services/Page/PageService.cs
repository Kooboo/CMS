#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.CMS.SiteKernel.Persistence;
using Kooboo.Common.ObjectContainer.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Services
{
    [Dependency(typeof(IPageService))]
    public class PageService : ServiceBase<Page, IPageProvider>, IPageService
    {
        #region .ctor
        public PageService(IPageProvider provider) :
            base(provider)
        {

        }
        #endregion

        public virtual Page GetDefaultPage(Site site)
        {
            var pages = Provider.All(site);
            var defaultPage = pages.Select(it => Get(it)).Where(it => it.IsDefault == true).FirstOrDefault();
            if (defaultPage == null)
            {
                defaultPage = pages.FirstOrDefault();
            }
            return defaultPage;
        }

        public virtual SiteMap GetSiteMap(Site site)
        {
            var rootPage = GetDefaultPage(site);

            if (rootPage == null)
            {
                return new SiteMap();
            }

            var rootNode = new SiteMapNode() { Page = Get(rootPage) };

            rootNode.Children = RootPages(site)
                .Select(it => GetPageNode(site, it))
                .OrderBy(it => it.Page.Navigation.Order);

            return new SiteMap() { Root = rootNode };
        }

        private SiteMapNode GetPageNode(Site site, Page page)
        {
            SiteMapNode node = new SiteMapNode() { Page = Get(page) };

            node.Children = ChildPages(page).Select(it => GetPageNode(site, it)).OrderBy(it => it.Page.Navigation.Order);

            return node;
        }

        public IEnumerable<Page> RootPages(Site site)
        {
            return Provider.All(site);
        }

        public IEnumerable<Page> ChildPages(Page parentPage)
        {
            return Provider.ChildPages(parentPage);
        }

        public Page Copy(Site site, string sourcePageFullName, string newPageFullName)
        {
            throw new NotImplementedException();
        }

        public void Move(IEnumerable<Page> pages, Page newParent)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Page> GetUnsyncedPages(Site currentSite, Page parentPage)
        {
            return new Page[0];
        }

        public void Clear(Site site)
        {
            throw new NotImplementedException();
        }



    }
}
