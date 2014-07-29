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
using Kooboo.Common.Globalization;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Web2.Areas.Contents.Models;

using Kooboo.Common.ObjectContainer;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Web2.Areas.Contents.Controllers
{
    [Kooboo.CMS.Web2.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Broadcasting", Order = 1)]
    public class ReceivingSettingController : ManagerControllerBase
    {
        #region .ctor
        private ReceivingSettingManager Manager
        {
            get;
            set;
        }
        public ReceivingSettingController(ReceivingSettingManager manager)
        {
            this.Manager = manager;
        }
        #endregion

        #region Index
        public virtual ActionResult Index(string search, string sortField, string sortDir)
        {
            var model = Manager.All(Repository, search).Select(o => Manager.Get(Repository, o.Name))
                .AsQueryable().SortBy(sortField, sortDir);
            return View(model);
        }
        #endregion


        #region Create
        public virtual ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Create(ReceivingSetting model, string repository, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    Manager.Add(Repository, model);
                    resultData.RedirectUrl = @return;
                }
            });

            return Json(data);
        }
        #endregion

        #region Edit
        public virtual ActionResult Edit(string uuid)
        {
            var model = Manager.Get(Repository, uuid);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Edit(ReceivingSetting model, string @return)
        {
            var data = new JsonResultData(ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    model.Repository = Repository;
                    Manager.Remove(Repository, model);
                    Manager.Add(Repository, model);
                    resultData.RedirectUrl = @return;
                });
            }

            return Json(data);
        }
        #endregion

        #region Delete
        public virtual ActionResult Delete(ReceivingSetting[] Model)
        {
            ModelState.Clear();
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                if (Model != null)
                {
                    foreach (var m in Model)
                    {
                        m.Repository = Repository;
                        Manager.Remove(Repository, m);
                    }
                    resultData.ReloadPage = true;
                }
            });
            return Json(data);
        }
        #endregion
    }
}
