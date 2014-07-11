#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

using Kooboo.CMS.Sites.Services;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class MovePageDataSource : PagesDataSource
    {
        protected override void CreateSelectItemTreeNode(RequestContext requestContext, Page page, List<System.Web.Mvc.SelectListItem> list)
        {
            string uuid = requestContext.GetRequestValue("UUID");
            var sourcePage = new Page(page.Site, uuid);

            var item = new SelectListItem();

            if (sourcePage != page && sourcePage.Parent != page)
            {              
                item.Text = page.FriendlyName;
                item.Value = page.FullName;
                list.Add(item);
            }          

            var children = ServiceFactory.PageManager.ChildPages(Site.Current, page.FullName, null);

            children.ForEach((p, index) =>
            {
                CreateSelectItemTreeNode(requestContext, p, list);
            });
        }
    }
}