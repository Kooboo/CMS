#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Common.Globalization;
using Kooboo.Common.Web.SelectList;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class UrlRedirectTypesDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Normal".Localize(), Value = "false" });
            list.Add(new SelectListItem() { Text = "Regular expression".Localize(), Value = "true" });

            return list;
        }
    }
}