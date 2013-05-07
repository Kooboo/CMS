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
    public class LinkTargetsDataSource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(LinkTarget)))
            {
                list.Add(new SelectListItem() { Text = item.ToString(), Value = ((int)item).ToString() });
            }
            return list.EmptyItem("");
        }
    }
}