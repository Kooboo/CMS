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
using Kooboo.Globalization;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Membership.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Member", Group = "", Name = "Membership", Order = 1)]
    public class MembershipConnectController : ControllerBase
    {
        #region .ctor
        MembershipConnectManager _manager;
        public MembershipConnectController(MembershipConnectManager manager)
        {
            this._manager = manager;
        }
        #endregion

        #region Index
        public virtual ActionResult Index(string search, string sortField, string sortDir)
        {
            return View(_manager.All(Membership, search).AsQueryable().SortBy(sortField, sortDir));
        }
        #endregion

        #region Create
        public virtual ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Create(MembershipConnect model, string @return)
        {
            JsonResultData data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    model.Membership = Membership;
                    _manager.Add(model);
                    resultData.RedirectUrl = @return;
                });
            }
            return Json(data);
        }

        #endregion

        #region Edit
        public virtual ActionResult Edit(string uuid)
        {
            var membershipConnect = new MembershipConnect() { Membership = Membership, UUID = uuid }.AsActual();
            return View(membershipConnect);
        }
        [HttpPost]
        public virtual ActionResult Edit(MembershipConnect model, string @return)
        {
            JsonResultData data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    model.Membership = Membership;
                    _manager.Update(model, model);
                    resultData.RedirectUrl = @return;
                });
            }
            return Json(data);
        }
        #endregion

        #region IsNameAvailable
        public virtual ActionResult IsNameAvailable(string name)
        {
            var membershipConnect = new MembershipConnect() { Membership = Membership, Name = name }.AsActual();
            if (membershipConnect != null)
            {
                return Json(string.Format("{0} is duplicate.".Localize(), name), JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete

        [HttpPost]
        public virtual ActionResult Delete(MembershipConnect[] model)
        {
            ModelState.Clear();
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (model != null)
                {
                    foreach (var o in model)
                    {
                        o.Membership = Membership;
                        _manager.Delete(o);
                    }
                }

                data.ReloadPage = true;
            });

            return Json(data);

        }
        #endregion

    }
}
