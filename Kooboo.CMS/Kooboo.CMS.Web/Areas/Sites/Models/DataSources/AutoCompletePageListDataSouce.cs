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
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class AutoCompletePageListDataSouce : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {

            var siteName = requestContext.GetRequestValue("siteName");

            var site = new Site(siteName).AsActual();


            IEnumerable<Page> pageList = new List<Page>();

            var rootPages = Kooboo.CMS.Sites.Services.ServiceFactory.PageManager.All(site, null);

            pageList = rootPages.ToList();

            foreach (var r in rootPages)
            {
                this.GenerateList(site, r, ref pageList);
            }

            if (filter == null)
            {
                return null;
            }

            var result = pageList.Where(o => o.VirtualPath.Contains(filter, StringComparison.OrdinalIgnoreCase)).Select(o => new SelectListItem { Text = o.VirtualPath, Value = o.VirtualPath });

            return result;

        }

        private void GenerateList(Site site, Page page, ref IEnumerable<Page> pageList)
        {
            var children = Kooboo.CMS.Sites.Services.ServiceFactory.PageManager.ChildPages(site, page.FullName, null);

            pageList = pageList.Concat(children);

            foreach (var s in children)
            {
                this.GenerateList(site, s, ref pageList);
            }


        }
    }
}