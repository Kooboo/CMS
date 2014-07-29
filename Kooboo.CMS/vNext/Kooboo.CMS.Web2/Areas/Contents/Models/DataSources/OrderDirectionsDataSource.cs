#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.Common.Web.SelectList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web2.Areas.Contents.Models.DataSources
{
    public class OrderDirectionsDataSource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            yield return new SelectListItem() { Text = "Ascending", Value = "0" };
            yield return new SelectListItem() { Text = "Descending", Value = "1", Selected = true };
        }
    }
}