#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Member.Services;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Web.Authorizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Member.Controllers
{
    [RequiredLogOn(RequiredAdministrator = true)]
    public class HomeController : AreaControllerBase
    {
        #region .ctor
        MembershipManager _manager = null;
        public HomeController(MembershipManager manager)
        {
            this._manager = manager;
        }
        #endregion
        #region Index
        public ActionResult Index()
        {
            return View();
        }
        #endregion

    }
}
