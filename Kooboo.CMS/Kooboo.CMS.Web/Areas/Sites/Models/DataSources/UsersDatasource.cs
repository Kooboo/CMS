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

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class UsersDatasource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var users = Kooboo.CMS.Account.Services.ServiceFactory.UserManager.All();
            if (!string.IsNullOrEmpty(filter))
            {
                users = users.Where(it => it.UserName.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase));
            }
            return users.Select(it => new SelectListItem() { Text = it.UserName, Value = it.UserName });
        }
    }
}