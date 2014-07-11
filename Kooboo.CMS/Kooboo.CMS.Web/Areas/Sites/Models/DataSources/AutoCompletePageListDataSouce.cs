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
using Kooboo.Common.Web.SelectList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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


            if (string.IsNullOrEmpty(filter))
            {
                return null;
            }
            var prefix = "";
            var slashIndex = filter.IndexOf('/');
            if (slashIndex != -1)
            {
                if (slashIndex == filter.Length - 1)
                {
                    prefix = filter.Substring(0, slashIndex + 1);
                    filter = "";
                }
                else
                {
                    prefix = filter.Substring(0, slashIndex + 1);
                    filter = filter.Substring(slashIndex + 1);
                }
            }
            if (string.IsNullOrEmpty(prefix))
            {
                prefix = "~/";
            }

            var result = pageList.Where(o => o.VirtualPath.Contains(filter, StringComparison.OrdinalIgnoreCase))
                .Select(o => new SelectListItem { Text = prefix + o.VirtualPath.TrimStart('/'), Value = prefix + o.VirtualPath.TrimStart('/') });

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