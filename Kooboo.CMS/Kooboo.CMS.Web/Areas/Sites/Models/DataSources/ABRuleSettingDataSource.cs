#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Common.ObjectContainer.Dependency;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Common.Web.SelectList;
namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class ABRuleSettingDataSource : ISelectListDataSource
    {

        #region Methods


        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            var list = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<ABRuleSettingManager>().All(Site.Current, "");
            return list.Select(it => new SelectListItem() { Text = it.Name, Value = it.Name });
        }
        #endregion
    }
}