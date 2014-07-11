#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.Web.SelectList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class LayoutPositionsDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var site = Site.Current;

            var fullName = requestContext.GetRequestValue("fullName");
            var page = Kooboo.CMS.Sites.Services.ServiceFactory.PageManager.Get(site, fullName);
            Layout layout = Kooboo.CMS.Sites.Services.ServiceFactory.LayoutManager.Get(site, page.Layout);

            var positionList = layout.Positions;
            foreach (var position in positionList)
            {
                yield return new System.Web.Mvc.SelectListItem() { Value = position.ID, Text = position.ID };
            }
        }
    }
}