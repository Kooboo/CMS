#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Kooboo.CMS.Sites.View
{
    /// <summary>
    /// 是与菜单和面包屑相关的辅助方法
    /// </summary>
    public class MenuHelper
    {
        public static IEnumerable<Page> Top()
        {
            return ServiceFactory.PageManager.All(Site.Current, "")
                .Select(it => it.AsActual())
                .Where(it => it.Navigation.Show)
                .Where(it => it.Published.HasValue && it.Published.Value == true)
                .OrderBy(it => it.Navigation.Order);
        }
        public static IEnumerable<Page> Sibling(Page page)
        {
            if (page.Parent == null)
            {
                return Top();
            }
            else
            {
                return ServiceFactory.PageManager.ChildPages(Site.Current, page.Parent.FullName, "")
                    .Select(it => it.AsActual())
                    .Where(it => it.Navigation.Show)
                    .Where(it => it.Published.HasValue && it.Published.Value == true)
                    .OrderBy(it => it.Navigation.Order);
            }
        }
        public static IEnumerable<Page> Sibling()
        {
            return Sibling(Page_Context.Current.PageRequestContext.Page);
        }
        public static IEnumerable<Page> Sub(Page page)
        {
            return ServiceFactory.PageManager.ChildPages(Site.Current, page.FullName, "")
                .Select(it => it.AsActual())
                .Where(it => it.Navigation.Show)
                .Where(it => it.Published.HasValue && it.Published.Value == true)
                .OrderBy(it => it.Navigation.Order);
        }
        public static IEnumerable<Page> Sub()
        {
            return Sub(Page_Context.Current.PageRequestContext.Page);
        }
        public static Page Parent(Page page)
        {
            return page.Parent.AsActual();
        }
        public static Page Parent()
        {
            return Page_Context.Current.PageRequestContext.Page.Parent.AsActual();
        }
        public static Page Current()
        {
            return Page_Context.Current.PageRequestContext.Page.AsActual();
        }
        public static bool IsCurrent(Page page)
        {
            var isCurrent = false;
            var currentPage = Page_Context.Current.PageRequestContext.Page;
            while (isCurrent == false && currentPage != null)
            {
                isCurrent = currentPage == page;
                if (isCurrent == true)
                {
                    return isCurrent;
                }
                currentPage = currentPage.Parent;
            }
            return isCurrent;
        }

        public static IEnumerable<Page> Breadcrumb()
        {
            List<Page> breadcrumbPages = new List<Page>();
            var defaultPage = ServiceFactory.PageManager.GetDefaultPage(Page_Context.Current.PageRequestContext.Page.Site);
            if (defaultPage == Page_Context.Current.PageRequestContext.Page)
            {
                return breadcrumbPages;
            }
            var currentPage = Page_Context.Current.PageRequestContext.Page.Parent;
            while (currentPage != null)
            {
                currentPage = currentPage.LastVersion(Page_Context.Current.PageRequestContext.Site).AsActual();

                if (currentPage.Navigation != null
                    && currentPage.Navigation.ShowInCrumb.Value == true && currentPage.Published.HasValue && currentPage.Published.Value == true)
                {
                    breadcrumbPages.Add(currentPage);
                }

                currentPage = currentPage.Parent;
            }
            if (breadcrumbPages.LastOrDefault() != defaultPage)
            {
                defaultPage = defaultPage.AsActual();

                if (defaultPage.Navigation != null
                && defaultPage.Navigation.ShowInCrumb.Value == true && defaultPage.Published.HasValue && defaultPage.Published.Value == true)
                {
                    breadcrumbPages.Add(defaultPage);
                }
            }

            breadcrumbPages.Reverse();
            return breadcrumbPages;
        }
    }
}
