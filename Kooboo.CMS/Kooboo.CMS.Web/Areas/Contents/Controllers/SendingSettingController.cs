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
using System.Web.Routing;
using System.Web.Script.Serialization;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Sites;


using Kooboo.Common.ObjectContainer;
using Kooboo.Common.Globalization;
using Kooboo.Common.Web;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    public class SendingSettingController : ManagerControllerBase
    {
        #region .ctor
        private SendingSettingManager Manager
        {
            get;
            set;
        }

        public SendingSettingController(SendingSettingManager manager)
        {
            Manager = manager;
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
            return View(new SendingSetting());
        }
        [HttpPost]
        public virtual ActionResult Create(string[] folderName, SendingSetting sendingSetting, string @return)
        {
            if (folderName == null || folderName.Length == 0)
            {
                ModelState.AddModelError("FolderName", "Folder name is required.".Localize());
            }
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in folderName)
                    {
                        if (Manager.Get(Repository, item) == null)
                        {
                            sendingSetting.FolderName = item;
                            Manager.Add(Repository, sendingSetting);
                        }

                    }
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
        public virtual ActionResult Edit(SendingSetting @new, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    @new.Repository = Repository;
                    Manager.Update(Repository, @new, @new);
                    resultData.RedirectUrl = @return;
                }
            });


            return Json(data);
        }

        #endregion

        #region Delete
        public virtual ActionResult Delete(SendingSetting[] Model)
        {
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
                }
                resultData.ReloadPage = true;
            });
            return Json(data);
        }
        #endregion

        #region ChangeSendReceived
        [HttpPost]
        public virtual ActionResult ChangeSendReceived(string name)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                var model = Manager.Get(Repository, name);
                if (model.SendReceived.HasValue && model.SendReceived.Value == true)
                {
                    model.SendReceived = false;
                }
                else
                {
                    model.SendReceived = true;
                }
                Manager.Update(Repository, model, model);
                resultData.RedirectUrl = Url.Action("Index", this.ControllerContext.RequestContext.AllRouteValues());
            });
            return Json(data);
        }

        #endregion

        #region IsNameAvailable
        public virtual ActionResult IsNameAvailable(string folderName)
        {
            if (!string.IsNullOrEmpty(folderName))
            {
                if (ServiceFactory.SendingSettingManager.Get(Repository, folderName) != null)
                {
                    return Json("The sending setting already exists.", JsonRequestBehavior.AllowGet);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
