#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Contents.Models.DataSources
{
    public class WorkflowsDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            return Kooboo.CMS.Content.Services.ServiceFactory
                .WorkflowManager
                .All(Repository.Current)
                .Select(o => new SelectListItem
                {
                    Value = o.Name,
                    Text = o.Name
                });
        }
    }
}