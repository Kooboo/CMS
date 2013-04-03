
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Web.Areas.Contents.Models;
using Kooboo.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Broadcasting", Order = 1)]
    public class ReceivingSettingController : ManagerControllerBase
    {
        //
        // GET: /Contents/ReceiveSetting/

        private ReceivingSettingManager Manager
        {
            get
            {
                return ServiceFactory.ReceiveSettingManager;
            }
        }

        public virtual ActionResult Index(string search)
        {
            var model = Manager.All(Repository, search).Select(o => Manager.Get(Repository, o.Name));
            return View(model);
        }


        public virtual ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Create(ReceivingSetting model, string repository)
        {
            var entry = new JsonResultEntry();
            if (ModelState.IsValid)
            {
                try
                {
                    Manager.Add(Repository, model);
                    entry.SetSuccess();
                }
                catch (Exception e)
                {
                    entry.AddException(e);
                }
            }
            else
            {
                entry.AddModelState(ModelState);
            }

            return Json(entry);
        }

        public virtual ActionResult Edit(string name)
        {
            var model = Manager.Get(Repository, name);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Edit(ReceivingSetting model)
        {
            var entry = new JsonResultEntry();
            if (ModelState.IsValid)
            {
                try
                {
                    model.Repository = Repository;
                    Manager.Remove(Repository, model);
                    Manager.Add(Repository, model);
                    entry.SetSuccess();
                }
                catch (Exception e)
                {
                    entry.AddException(e);
                }
            }
            else
            {
                entry.AddModelState(ModelState);
            }
            return Json(entry);
        }

        public virtual ActionResult Delete(ReceivingSetting[] Model)
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
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }

        public virtual ActionResult GetFolderDataSource(string repository)
        {
            var sourceRepository = new Repository(repository);

            var settingFolders = ServiceFactory.SendingSettingManager.All(sourceRepository, "");

            var items = settingFolders.Select(it => ServiceFactory.SendingSettingManager.Get(it.Repository, it.Name))
                .Select(it => new SelectListItemTree()
                {
                    Text = it.FolderName,
                    Value = it.FolderName
                }).ToArray();

            return Json(items);
        }

    }
}
