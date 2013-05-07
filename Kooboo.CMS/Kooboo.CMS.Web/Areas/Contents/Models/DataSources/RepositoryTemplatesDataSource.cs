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

namespace Kooboo.CMS.Web.Areas.Contents.Models.DataSources
{
    public class RepositoryTemplatesDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            return Kooboo.CMS.Content.Services.ServiceFactory.RepositoryTemplateManager.All().Select(it => new SelectListItem()
            {
                Text = it.TemplateName,
                Value = it.TemplateName
            }).EmptyItem("   ");
        }
    }
}