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
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [Authorizations.RequiredLogOn]
    public class PendingWorkflowController : ManagerControllerBase
    {
        #region .ctor
        WorkflowManager Manager { get; set; }

        public PendingWorkflowController(WorkflowManager manager)
        {
            Manager = manager;
        }
        #endregion

        #region Index
        public virtual ActionResult Index()
        {
            var items = Manager.GetPendingWorkflowItems(Repository, User.Identity.Name);

            return View(items);
        }
        #endregion

        #region Process
        public virtual ActionResult Process(string roleName, string UUID)
        {
            var item = Manager.GetPendingWorkflowItem(Repository, roleName, UUID);

            return View(item);
        }

        [HttpPost]
        public virtual ActionResult Process(string workflowName, string roleName, string UUID, string comment, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                Manager.ProcessPendingWorkflowItem(Repository, workflowName, roleName, UUID, User.Identity.Name, true, comment);
                resultData.RedirectUrl = @return;
            });

            return Json(data);
        }
        [HttpPost]
        public virtual ActionResult Reject(string workflowName, string roleName, string UUID, string comment, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                Manager.ProcessPendingWorkflowItem(Repository, workflowName, roleName, UUID, User.Identity.Name, false, comment);
                resultData.RedirectUrl = @return;
            });

            return Json(data);
        }
        #endregion

        #region WorkflowHistory
        public virtual ActionResult WorkflowHistory(string UUID, string folderName)
        {

            var textContent = new TextFolder(Repository, folderName).CreateQuery().WhereEquals("UUID", UUID).FirstOrDefault();

            var history = Manager.GetWorkflowHistory(Repository, textContent);

            return View(history);
        }
        #endregion
    }
}
