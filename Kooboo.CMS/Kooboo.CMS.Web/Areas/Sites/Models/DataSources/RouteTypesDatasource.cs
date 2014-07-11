#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.ABTest;
using Kooboo.Common.Web.SelectList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class RouteTypesDatasource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            return  Kooboo.Common.ObjectContainer.EngineContext.Current.ResolveAll<IVisitRule>()
                .Select(o => new SelectListItem
                {
                    Text = o.RuleTypeDisplayName,
                    Value = o.RuleType
                });
        }
    }

}