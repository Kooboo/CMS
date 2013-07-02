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

namespace Kooboo.CMS.Web.Areas.Member.Controllers
{
    public class ControllerBase : AreaControllerBase
    {
        public Membership Membership
        {
            get
            {
                return new Membership(this.ControllerContext.RequestContext.GetRequestValue("membershipName"));
            }
        }
    }
}
