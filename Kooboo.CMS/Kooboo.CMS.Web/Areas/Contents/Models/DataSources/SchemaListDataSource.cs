#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Kooboo.CMS.Web.Areas.Contents.Models.DataSources
{
    public class SchemaListDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            yield return new SelectListItem() { Text = "None".Localize(), Value = "", Selected = true };
            var repository = Repository.Current;
            foreach (var item in Kooboo.CMS.Content.Services.ServiceFactory.SchemaManager.All(repository, ""))
            {
                yield return new SelectListItem() { Text = item.Name, Value = item.Name };
            }
        }
    }
}