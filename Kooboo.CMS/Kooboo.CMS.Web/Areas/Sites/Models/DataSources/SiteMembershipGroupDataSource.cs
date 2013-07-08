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
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class SiteMembershipGroupDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            if (Site.Current != null)
            {
                var membershipGroupManager = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<MembershipGroupManager>();
                var membership = Site.Current.GetMembership();
                return membershipGroupManager.All(membership, "").Select(it => new SelectListItem() { Text = it.Name, Value = it.Name });
            }
            else
            {
                return new SelectListItem[0];
            }

        }
    }
}
