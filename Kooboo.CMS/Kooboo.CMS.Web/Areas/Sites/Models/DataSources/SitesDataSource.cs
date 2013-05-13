#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class SitesDataSource : ISelectListDataSource
    {
        #region Methods

        private static void CreateItem(SiteNode siteNode, RequestContext requestContext, List<SelectListItem> list)
        {
            var selectListItem = new SelectListItem();
            list.Add(selectListItem);
            if (siteNode != null)
            {
                var site = (siteNode.Site).AsActual();
                selectListItem.Text = site.FriendlyName;
                selectListItem.Value = site.FullName;

                siteNode.Children.ForEach((node, index) =>
                {
                    CreateItem(node, requestContext, list);
                });
            }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            var sites = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<SiteManager>().SiteTrees(requestContext.HttpContext.User.Identity.Name);
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { });
            foreach (var s in sites)
            {
                CreateItem(s.Root, requestContext, list);
            }
            return list;
        }
        #endregion
    }
}