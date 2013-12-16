using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.DataSources
{
    public class PagesDataSource:ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            List<PageItem> list = new List<PageItem>() { new PageItem() { } };

            var rootPages = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<PageManager>().All(Site.Current, null);

            rootPages.ForEach((page, index) =>
            {
                CreateSelectItemTreeNode(requestContext, page, list);
            });

            return list;
        }
        protected virtual void CreateSelectItemTreeNode(RequestContext requestContext, Page page, List<PageItem> list)
        {
            var item = new PageItem();
            item.Text = page.FriendlyName;
            item.Value = page.FullName;
            item.IsPublished = page.Published.HasValue ? page.Published.Value : false;
            list.Add(item);

            var children = ServiceFactory.PageManager.ChildPages(Site.Current, page.FullName, null);

            children.ForEach((p, index) =>
            {
                CreateSelectItemTreeNode(requestContext, p, list);
            });
        }
    }

    public class PageItem : SelectListItem
    {
        public bool IsPublished { get; set; }
    }
}