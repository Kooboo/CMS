#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class SiteTemplatesDataSource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            return Kooboo.CMS.Sites.Services.ServiceFactory.SiteTemplateManager.All().Select(it => new SelectListItem()
            {
                Text = it.TemplateName,
                Value = it.TemplateName
            }).EmptyItem("   ");
        }
    }
}