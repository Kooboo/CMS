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

namespace Kooboo.CMS.Web.Areas.Contents.Models.DataSources
{
    public class RepositoriesDataSource : ISelectListDataSource
    {
        #region ISelectListDataSource Members

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            IEnumerable<Repository> repositories = new Repository[] { Repository.Current };
            if (ServiceFactory.UserManager.IsAdministrator(requestContext.HttpContext.User.Identity.Name))
            {
                repositories = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.All();
            }


            return repositories.Select(it => it.AsActual()).Select(it => new SelectListItem() { Text = string.IsNullOrEmpty(it.DisplayName) ? it.Name : it.DisplayName, Value = it.Name }).EmptyItem("");
        }

        #endregion
    }
}