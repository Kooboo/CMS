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

namespace Kooboo.CMS.Web.Areas.Contents.Models.DataSources
{
    public class RolesDatasource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var roles = Kooboo.CMS.Account.Services.ServiceFactory.RoleManager.All();
            return roles.Select(it => new SelectListItem() { Text = it.Name, Value = it.Name });
        }
    }
}