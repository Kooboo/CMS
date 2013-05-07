#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class VisitRuleSettingDataSource : ISelectListDataSource
    {
        #region Properties
        [Inject]
        public VisitRuleSettingManager Manager { get; set; }
        #endregion

        #region Methods


        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            var list = Manager.All(Site.Current, "");
            return list.Select(it => new SelectListItem() { Text = it.Name, Value = it.Name });
        }
        #endregion
    }
}