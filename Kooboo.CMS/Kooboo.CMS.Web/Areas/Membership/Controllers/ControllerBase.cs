#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Web.Mvc;
using Kooboo.CMS.Member.Models;
using Kooboo.CMS.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web.Areas.Membership.Controllers
{
    public class ControllerBase : AreaControllerBase
    {
        public Kooboo.CMS.Member.Models.Membership Membership
        {
            get
            {
                return new Kooboo.CMS.Member.Models.Membership(this.ControllerContext.RequestContext.GetRequestValue("membershipName"));
            }
        }
    }
}
