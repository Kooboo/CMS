#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Member.Models;
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
    public class MembershipController : AreaControllerBase
    {
        #region .ctor
        MembershipManager _manager = null;
        public MembershipController(MembershipManager manager)
        {
            this._manager = manager;
        }
        #endregion

        #region Index
        public ActionResult Index(string search)
        {
            var memberships = _manager.All(search);
            return View(memberships);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Membership membership, string @return)
        {
            JsonResultData data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    _manager.Add(membership);
                    resultData.RedirectUrl = @return;
                });
            }

            return Json(data);
        }
        #endregion

    }
}
