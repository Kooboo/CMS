#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Member.Models;
using Kooboo.CMS.Member.Services;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Member.Models.DataSources
{
    public class MembershipGroupDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            var membershipGroupManager = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<MembershipGroupManager>();
            var membership = new Membership(requestContext.GetRequestValue("membershipName"));
            return membershipGroupManager.All(membership, "").Select(it => new SelectListItem() { Text = it.Name, Value = it.Name });
        }
    }
}
