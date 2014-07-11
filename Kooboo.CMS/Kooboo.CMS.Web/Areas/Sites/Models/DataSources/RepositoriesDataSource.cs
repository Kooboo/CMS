#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Common.Web.SelectList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class RepositoriesDataSource : ISelectListDataSource
    {
        #region ISelectListDataSource Members

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            IEnumerable<Repository> repositories = new Repository[0];
            if (ServiceFactory.UserManager.IsAdministrator(requestContext.HttpContext.User.Identity.Name))
            {
                repositories = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.All();
            }
            else if (Repository.Current != null)
            {
                repositories = new[] { Repository.Current };
            }

            return repositories.Select(it => it.AsActual()).Select(it => new SelectListItem() { Text = string.IsNullOrEmpty(it.DisplayName) ? it.Name : it.DisplayName, Value = it.Name }).EmptyItem("");
        }

        #endregion
    }
}