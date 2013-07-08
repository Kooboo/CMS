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
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Membership.Controllers
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
        public ActionResult Index(string search, string sortField, string sortDir)
        {
            var memberships = _manager.All(search).AsQueryable().SortBy(sortField, sortDir);
            return View(memberships);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Kooboo.CMS.Member.Models.Membership membership, string @return)
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

        #region Edit
        public virtual ActionResult Edit(string membershipName)
        {
            var membership = new Kooboo.CMS.Member.Models.Membership() { Name = membershipName }.AsActual();

            return View(membership);
        }
        [HttpPost]
        public virtual ActionResult Edit(Kooboo.CMS.Member.Models.Membership model, string @return)
        {
            JsonResultData data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    _manager.Update(model, model);
                    resultData.RedirectUrl = @return;
                });
            }
            return Json(data);
        }
        #endregion

        #region Delete
        [HttpPost]
        public virtual ActionResult Delete(string membershipName)
        {
            ModelState.Clear();
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                _manager.Delete(new Kooboo.CMS.Member.Models.Membership() { Name = membershipName });

                data.ReloadPage = true;
            });

            return Json(data);

        }
        #endregion

    }
}
