using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Content.Services;

using Kooboo.CMS.Content.Query;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Workflow", Order = 1)]
    public class WorkflowController : ManagerControllerBase
    {
        private WorkflowManager Manager;

        public WorkflowController()
        {
            Manager = ServiceFactory.WorkflowManager;
        }

        #region CRUD

        //
        // GET: /Contents/Workflow/
        public virtual ActionResult Index()
        {
            return View(Manager.All(Repository));
        }

        public virtual ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Create(Workflow model)
        {
            var entry = new JsonResultEntry();
            try
            {
                int index = 1;
                foreach (var item in model.Items)
                {
                    item.Sequence = index;
                    index++;
                }
                Manager.Add(Repository, model);
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }

        public virtual ActionResult Edit(string name)
        {
            return View(Manager.Get(Repository, name));
        }

        [HttpPost]
        public virtual ActionResult Edit(Workflow model)
        {
            var entry = new JsonResultEntry();
            try
            {
                int index = 1;
                foreach (var item in model.Items)
                {
                    item.Sequence = index;
                    index++;
                }
                Manager.Update(Repository, model, model);
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }

        [HttpPost]
        public virtual ActionResult Delete(Workflow[] model)
        {
            var entry = new JsonResultEntry();
            try
            {
                Manager.Delete(model, Repository);
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }

        #endregion
    }
}
