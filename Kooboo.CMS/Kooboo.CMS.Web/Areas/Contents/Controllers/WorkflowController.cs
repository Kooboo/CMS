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
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Content.Services;

using Kooboo.CMS.Content.Query;
using Kooboo.Common.ObjectContainer;
using Kooboo.Common.Globalization;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Workflow", Order = 1)]
    public class WorkflowController : ManagerControllerBase
    {
        #region .ctor

        private WorkflowManager Manager;

        public WorkflowController(WorkflowManager manager)
        {
            Manager = manager;
        }
        #endregion

        #region Index

        public virtual ActionResult Index(string sortField, string sortDir)
        {
            return View(Manager.All(Repository).AsQueryable().SortBy(sortField, sortDir));
        }


        #endregion

        #region Create

        public virtual ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Create(Workflow model, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                if (model.Items != null)
                {
                    int index = 1;
                    foreach (var item in model.Items)
                    {
                        item.Sequence = index;
                        index++;
                    }
                }

                Manager.Add(Repository, model);

                resultData.RedirectUrl = @return;
            });
            return Json(data);
        }
        #endregion

        #region Edit
        public virtual ActionResult Edit(string uuid)
        {
            return View(Manager.Get(Repository, uuid));
        }

        [HttpPost]
        public virtual ActionResult Edit(Workflow model, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                int index = 1;
                foreach (var item in model.Items)
                {
                    item.Sequence = index;
                    index++;
                }
                Manager.Update(Repository, model, model);
                resultData.RedirectUrl = @return;
            });
            return Json(data);
        }
        #endregion

        #region Delete
        [HttpPost]
        public virtual ActionResult Delete(Workflow[] model)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                Manager.Delete(model, Repository);
                resultData.ReloadPage = true;
            });

            return Json(data);
        }
        #endregion

        #region IsNameAvailable
        /// <summary>
        /// for remote validation
        /// </summary>
        /// <param name="name"></param>
        /// <param name="old_Key"></param>
        /// <returns></returns>
        public virtual ActionResult IsNameAvailable(string name, string old_Key)
        {
            if (old_Key == null || !name.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                if (Manager.Get(Repository, name) != null)
                {
                    return Json("The name already exists.".Localize(), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion        
    }
}
