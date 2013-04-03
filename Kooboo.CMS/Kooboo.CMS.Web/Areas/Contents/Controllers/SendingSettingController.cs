using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Sites;
using Kooboo.Web.Script.Serialization;
using Kooboo.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    public class SendingSettingController : ManagerControllerBase
    {
        //
        // GET: /Contents/SendingSetting/

        private SendingSettingManager Manager
        {
            get
            {
                return ServiceFactory.SendingSettingManager;
            }
        }

        public virtual ActionResult Index(string search)
        {
            var model = Manager.All(Repository, search).Select(o => Manager.Get(Repository, o.Name));
            return View(model);
        }

        public virtual ActionResult Create()
        {
            return View(new SendingSetting());
        }
        [HttpPost]
        public virtual ActionResult Create(SendingSetting sendingSetting)
        {

            var entry = new JsonResultEntry();
            try
            {
                if (ModelState.IsValid)
                {
                    Manager.Add(Repository, sendingSetting);

                    entry.SetSuccess();
                }
                else
                {
                    entry.AddModelState(ModelState);
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }

        public virtual ActionResult Edit(string Name)
        {
            var model = Manager.Get(Repository, Name);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Edit(SendingSetting @new)
        {
            var entry = new JsonResultEntry();
            try
            {
                if (ModelState.IsValid)
                {
                    @new.Repository = Repository;
                    Manager.Update(Repository, @new, @new);
                    entry.SetSuccess();
                }
                else
                {
                    entry.AddModelState(ModelState);
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }

        public virtual ActionResult Delete(SendingSetting[] Model)
        {
            var entry = new JsonResultEntry();
            try
            {
                if (Model != null)
                {
                    foreach (var m in Model)
                    {
                        m.Repository = Repository;
                        Manager.Remove(Repository, m);
                    }
                }
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }

        [HttpPost]
        public virtual ActionResult ChangeSendReceived(string name)
        {
            var entry = new JsonResultEntry();
            try
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
                entry.RedirectUrl = Url.Action("Index", this.ControllerContext.RequestContext.AllRouteValues());
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }

        public string FolderDataSource(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                term = term.Replace("+", "").Trim();
            }
            var folder = ServiceFactory.TextFolderManager.All(Repository, term);
            return folder.Select(o => new { label = o.Name, value = o.Name }).ToJSON();
        }

        public string SchemaDataSource(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                term = term.Replace("+", "").Trim();
            }
            var schema = ServiceFactory.SchemaManager.All(Repository, term);
            return schema.Select(o => new { label = o.Name, value = o.Name }).ToJSON();
        }

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

    }
}
