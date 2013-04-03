using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Content.Query;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [Authorizations.RequiredLogOn]
    public class PendingWorkflowController : ManagerControllerBase
    {
        private WorkflowManager Manager;

        public PendingWorkflowController()
        {
            Manager = ServiceFactory.WorkflowManager;
        }
        public virtual ActionResult Index()
        {
            var items = Manager.GetPendingWorkflowItems(Repository, User.Identity.Name);

            return View(items);
        }

        public virtual ActionResult Process(string roleName, string name)
        {
            var item = Manager.GetPendingWorkflowItem(Repository, roleName, name);

            return View(item);
        }

        [HttpPost]
        public virtual ActionResult Process(string workflowName,string roleName, string name, bool passed, string comment)
        {
            JsonResultEntry entry = new JsonResultEntry();

            try
            {
                Manager.ProcessPendingWorkflowItem(Repository, workflowName,roleName, name, User.Identity.Name, passed, comment);
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }


            return Json(entry);
        }

        public virtual ActionResult WorkflowHistory(string UUID, string folderName)
        {

            var textContent = new TextFolder(Repository, folderName).CreateQuery().WhereEquals("UUID", UUID).FirstOrDefault();

            var history = Manager.GetWorkflowHistory(Repository, textContent);

            return View(history);
        }
    }
}
