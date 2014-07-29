#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Membership.OAuthClients;
using Kooboo.Common.Web.SelectList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Web2.Areas.Membership.Models.DataSources
{
    public class AuthClientDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            var authClients = Kooboo.Common.ObjectContainer.EngineContext.Current.ResolveAll<IAuthClient>();

            return authClients.Select(it => new SelectListItem() { Text = it.ProviderName, Value = it.ProviderName });
        }
    }
}
