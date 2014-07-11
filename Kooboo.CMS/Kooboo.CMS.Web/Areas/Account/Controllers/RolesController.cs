#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Account.Services;
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Web.Areas.Account.Models;
using Kooboo.CMS.Web.Models;
using Kooboo.Common.Globalization;
using Kooboo.CMS.Sites;
using Kooboo.Common.ObjectContainer;
using Kooboo.Common.Web;
namespace Kooboo.CMS.Web.Areas.Account.Controllers
{
    [Kooboo.CMS.Web.Authorizations.RequiredLogOnAttribute(RequiredAdministrator = true, Order = 1)]
    public class RolesController : ControllerBase
    {
        #region .ctor
        public RoleManager RoleManager { get; private set; }
        public RolesController(RoleManager roleManager)
        {
            RoleManager = roleManager;
        }
        #endregion

        #region Index
        [HttpGet]
        public virtual ActionResult Index(string sortField, string sortDir)
        {
            return View(RoleManager.All().AsQueryable().SortBy(sortField, sortDir));
        }
        #endregion

        #region Create
        [HttpGet]
        public virtual ActionResult Create()
        {
            return View(new CreateRoleModel(new Role(), ServiceFactory.RoleManager.AllPermissions()));
        }
        [HttpPost]
        public virtual ActionResult Create(CreateRoleModel model, string @return)
        {
            JsonResultData data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                var role = RoleManager.Get(model.Name);
                if (ModelState.IsValid)
                {
                    RoleManager.Add(model.ToRole());
                    data.RedirectUrl = @return;
                }
            });

            return Json(data);
        }
        #endregion

        #region Edit
        [HttpGet]
        public virtual ActionResult Edit(string uuid)
        {
            var role = ServiceFactory.RoleManager.Get(uuid);
            return View(new CreateRoleModel(role, ServiceFactory.RoleManager.AllPermissions()));
        }
        [HttpPost]
        public virtual ActionResult Edit(CreateRoleModel model, string @return)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    RoleManager.Update(model.Name, model.ToRole());
                    resultData.RedirectUrl = @return;
                });
            }

            return Json(data);
        }
        #endregion

        #region Delete
        public virtual ActionResult Delete(Role[] model)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                foreach (var role in model)
                {
                    RoleManager.Delete(role.Name);
                }
                resultData.ReloadPage = true;
            });
            return Json(data);
        }
        #endregion

        #region IsNameAvailable
        public virtual ActionResult IsNameAvailable(string name)
        {
            if (RoleManager.Get(name) != null)
            {
                return Json(string.Format("{0} is not available.".Localize(), name), JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
