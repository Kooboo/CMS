#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Common.Web.SelectList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class PagesDataSource : ISelectListDataSource
    {
              #region Methods
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            List<SelectListItem> list = new List<SelectListItem>() { new SelectListItem() { } };

            var rootPages = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<PageManager>().All(Site.Current, null);

            rootPages.ForEach((page, index) =>
            {
                CreateSelectItemTreeNode(requestContext, page, list);
            });

            return list;
        }
        protected virtual void CreateSelectItemTreeNode(RequestContext requestContext, Page page, List<SelectListItem> list)
        {
            var item = new SelectListItem();
            item.Text = page.FriendlyName;
            item.Value = page.FullName;
            list.Add(item);

            var children = ServiceFactory.PageManager.ChildPages(Site.Current, page.FullName, null);

            children.ForEach((p, index) =>
            {
                CreateSelectItemTreeNode(requestContext, p, list);
            });
        }
        #endregion
    }
}