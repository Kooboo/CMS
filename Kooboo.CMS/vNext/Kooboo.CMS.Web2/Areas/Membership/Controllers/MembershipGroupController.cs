#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Membership.Services;
using Kooboo.Common.Globalization;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Web2.Areas.Membership.Controllers
{
    [Kooboo.CMS.Web2.Authorizations.Authorization(AreaName = "Membership", Group = "", Name = "Group", Order = 1)]
    public class MembershipGroupController : ControllerBase
    {
        #region .ctor
        private MembershipGroupManager _manager;
        public MembershipGroupController(MembershipGroupManager manager)
        {
            _manager = manager;
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
        public virtual ActionResult Create(MembershipGroup model, string @return)
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

        #region IsNameAvailable
        public virtual ActionResult IsNameAvailable(string name)
        {
            var membershipGroup = new MembershipGroup() { Membership = Membership, Name = name }.AsActual();
            if (membershipGroup != null)
            {
                return Json("The name is duplicate.".Localize(), JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete

        [HttpPost]
        public virtual ActionResult Delete(MembershipGroup[] model)
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
