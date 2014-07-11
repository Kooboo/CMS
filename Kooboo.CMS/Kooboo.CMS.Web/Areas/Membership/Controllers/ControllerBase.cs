#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;

using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

using Kooboo.Common.Globalization;
namespace Kooboo.CMS.Web.Areas.Membership.Controllers
{
    public class ControllerBase : AreaControllerBase
    {
        public Kooboo.CMS.Membership.Models.Membership Membership
        {
            get
            {
                var membership = new Kooboo.CMS.Membership.Models.Membership(this.ControllerContext.RequestContext.GetRequestValue("membershipName")).AsActual();
                if (membership == null)
                {
                    throw new ArgumentNullException("The membership doesn't exists".Localize());
                }
                return membership;
            }
        }
    }
}
