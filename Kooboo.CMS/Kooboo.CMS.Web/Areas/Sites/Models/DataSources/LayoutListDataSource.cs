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
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class LayoutListDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var site = Site.Current;
            var layoutList = Kooboo.CMS.Sites.Persistence.Providers.LayoutProvider.All(site);
            foreach (var l in layoutList)
            {
                yield return new System.Web.Mvc.SelectListItem() { Value = l.Name, Text = l.Name };
            }
        }
    }
}