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
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.CMS.Web.Models;
using System.Text;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.RequiredLogOnAttribute(RequiredAdministrator = true, Order = 99)]
    public class ModuleManagementController : Kooboo.CMS.Sites.AreaControllerBase
    {
        #region .ctor
        public ModuleManagementController(ModuleManager moduleManager)
        {
            Manager = moduleManager;
        }
        public ModuleManager Manager { get; set; }
        #endregion

        #region Index
        public virtual ActionResult Index()
        {
            return View(Manager.AllModuleInfo());
        }
        #endregion

        #region Install
        [HttpGet]
        public virtual ActionResult Install()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Install(InstallModuleModel installModel, string @return)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    var moduleFile = this.Request.Files["ModuleFile"];

                    StringBuilder log = new StringBuilder();
                    var moduleName = System.IO.Path.GetFileNameWithoutExtension(moduleFile.FileName);

                    var moduleInfo = Manager.Install(moduleName, moduleFile.InputStream, ref log);

                    if (moduleInfo == null && log.Length != 0)
                    {
                        data.Success = false;
                        data.AddMessage(log.ToString());
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(moduleInfo.InstallingTemplate))
                        {
                            data.RedirectUrl = Url.Action("OnInstalling", ControllerContext.RequestContext.AllRouteValues().Merge("ModuleName", moduleName));
                        }
                        else
                        {
                            data.RedirectUrl = @return;
                        }
                    }
                });
            }

            return Json(data);
        }

        public virtual ActionResult OnInstalling(string moduleName)
        {
            ModuleInfo moduleInfo = ModuleInfo.Get(moduleName);
            return View(moduleInfo);
        }
        [HttpPost]
        public virtual ActionResult OnInstalling(string moduleName, FormCollection form)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    Manager.OnInstalling(moduleName, ControllerContext);
                    resultData.RedirectUrl = Url.Action("InstallComplete", ControllerContext.RequestContext.AllRouteValues());
                });
            }

            return Json(data);
        }
        public virtual ActionResult InstallComplete(string moduleName, string @return)
        {
            return View();
        }
        #endregion

        #region Uninstall
        public virtual ActionResult OnUninstalling(string uuid)
        {
            ModuleInfo moduleInfo = ModuleInfo.Get(uuid);
            return View(moduleInfo);
        }
        [HttpPost]
        public virtual ActionResult OnUninstalling(string uuid, FormCollection form)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    Manager.OnUnistalling(uuid, ControllerContext);
                    resultData.RedirectUrl = Url.Action("Uninstall", ControllerContext.RequestContext.AllRouteValues());
                });
            }

            return Json(data);
        }
        public virtual ActionResult GoingToUninstall(string uuid)
        {
            ModuleInfo moduleInfo = ModuleInfo.Get(uuid);

            var sites = Manager.AllSitesInModule(uuid);
            if (sites.Count() > 0)
            {
                return View();
            }
            if (!string.IsNullOrEmpty(moduleInfo.UninstallingTemplate))
            {
                return RedirectToAction("OnUninstalling", ControllerContext.RequestContext.AllRouteValues());
            }
            else
            {
                return RedirectToAction("Uninstall", ControllerContext.RequestContext.AllRouteValues());
            }
        }
        public virtual ActionResult Uninstall(string uuid)
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Uninstall(string uuid, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {

                if (!string.IsNullOrEmpty(uuid))
                {
                    Manager.Uninstall(uuid);
                }

                resultData.RedirectUrl = @return;
            });
            return Json(data);
        }
        #endregion

        #region Relations
        public virtual ActionResult Relations(string uuid)
        {
            var model = Manager.AllSitesInModule(uuid).Select(siteName => new RelationModel()
            {
                RelationName = siteName,
                RelationType = "Site"
            });
            return View("Relations", model);
        }
        #endregion

        #region IsNameAvailable
        public virtual ActionResult IsNameAvailable(string moduleName)
        {
            if (Manager.Get(moduleName) != null)
            {
                return Json("The name already exists.", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
