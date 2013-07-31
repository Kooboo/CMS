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
using Kooboo.Globalization;
using Kooboo.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Member.Persistence;
using Kooboo.CMS.Web.Areas.Membership.Models;

namespace Kooboo.CMS.Web.Areas.Membership.Controllers
{
    [RequiredLogOn(RequiredAdministrator = true)]
    public class MembershipController : AreaControllerBase
    {
        #region .ctor
        MembershipManager _manager = null;
        IMembershipProvider _provider = null;
        public MembershipController(MembershipManager manager, IMembershipProvider provider)
        {
            this._manager = manager;
            this._provider = provider;
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

        #region IsUserNameAvailable
        public virtual ActionResult IsNameAvailable(string name)
        {
            var membership = new Kooboo.CMS.Member.Models.Membership(name).AsActual();
            if (membership != null)
            {
                return Json("The name is duplicate.".Localize(), JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
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
        public virtual ActionResult Delete(Kooboo.CMS.Member.Models.Membership[] model, string @return)
        {
            ModelState.Clear();
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        _manager.Delete(item);
                    }
                    resultData.RedirectUrl = @return;
                }
            });
            return Json(data);

        }
        #endregion

        #region Export
        [HttpPost]
        public virtual ActionResult Export(Kooboo.CMS.Member.Models.Membership[] model)
        {

            if (model != null || model.Length > 1)
            {
                string fileName = model.First().Name + ".zip";
                Response.AttachmentHeader(fileName);
                var membership = model.First().AsActual();
                if (membership != null)
                {
                    _provider.Export(membership, Response.OutputStream);
                }

            }
            return null;

        }

        #endregion

        #region Import
        public ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Import(string name, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    _provider.Import(name, Request.Files[0].InputStream);
                }
                data.RedirectUrl = @return;
            });
            return Json(data, "text/plain", System.Text.Encoding.UTF8);
        }
        #endregion

        #region Go
        public ActionResult Go(string membershipName, string uuid)
        {
            if (string.IsNullOrEmpty(membershipName) && !string.IsNullOrEmpty(uuid))
            {
                membershipName = uuid;
            }
            return RedirectToAction("Index", "MembershipUser", new { membershipName = membershipName });
        }
        #endregion

    }
}
