#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Extensions;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class PluginsDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var site = Site.Current;
            var types = ServiceFactory.GetService<AssemblyManager>().GetTypes(site, typeof(IPagePlugin));
            return types.Select(o => new SelectListItem { Text = o.Name, Value = o.AssemblyQualifiedNameWithoutVersion() });
        }
    }
}