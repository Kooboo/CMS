#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class ElementCategoryCulturesSelectListDataSource : ISelectListDataSource
    {
        #region ISelectListDataSource Members

        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var site = new Site(requestContext.GetRequestValue("siteName"));
            var categories = Kooboo.CMS.Sites.Services.ServiceFactory.LabelManager.GetCategories(site).Where(it => !string.IsNullOrEmpty(it.Category));
            if (!string.IsNullOrEmpty(filter))
            {
                categories = categories.Where(it => it.Category.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase));
            }
            return categories.Select(it => new SelectListItem() { Text = it.Category, Value = it.Category });
        }

        #endregion
    }
}