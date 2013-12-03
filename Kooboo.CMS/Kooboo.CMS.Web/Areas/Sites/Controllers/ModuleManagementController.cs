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
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Extension.ModuleArea.Management;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.RequiredLogOnAttribute(RequiredAdministrator = true, Order = 99)]
    public class ModuleManagementController : Kooboo.CMS.Sites.AreaControllerBase
    {
        #region .ctor
        IModuleInstaller _moduleInstaller;
        IModuleUninstaller _moduleUninstaller;
        public ModuleManagementController(ModuleManager moduleManager, IModuleInstaller moduleInstaller, IModuleUninstaller moduleUninstaller)
        {
            Manager = moduleManager;
            _moduleInstaller = moduleInstaller;
            _moduleUninstaller = moduleUninstaller;
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

                    var moduleName = System.IO.Path.GetFileNameWithoutExtension(moduleFile.FileName);
                    var errorMessage = _moduleInstaller.Unzip(ref moduleName, moduleFile.InputStream);

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        data.Success = false;
                        data.AddMessage(errorMessage);
                    }
                    else
                    {
                        //copy the assembly files to bin folder.
                        data.RedirectUrl = Url.Action("CopyAssemblyFiles", ControllerContext.RequestContext.AllRouteValues().Merge("ModuleName", moduleName));

                    }
                });
            }

            return Json(data);
        }
        public virtual ActionResult CopyAssemblyFiles(string moduleName)
        {
            var conflictedAssemblies = _moduleInstaller.CheckConflictedAssemblyReferences(moduleName);
            if (conflictedAssemblies.Count() > 0)
            {
                return View(conflictedAssemblies);
            }
            else
            {
                _moduleInstaller.CopyAssemblies(moduleName, false);
                return Redirect(FininshedOrRunCustomInstallingEvent(moduleName));
            }
        }
        private string FininshedOrRunCustomInstallingEvent(string moduleName)
        {
            var moduleInfo = ModuleInfo.Get(moduleName);
            if (!string.IsNullOrEmpty(moduleInfo.InstallingTemplate))
            {
                return Url.Action("OnInstalling", ControllerContext.RequestContext.AllRouteValues().Merge("ModuleName", moduleName));
            }
            else
            {
                return Url.Action("InstallComplete", ControllerContext.RequestContext.AllRouteValues());
            }
        }
        [HttpPost]
        public virtual ActionResult CopyAssemblyFiles(string moduleName, bool @override)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
               {
                   _moduleInstaller.CopyAssemblies(moduleName, @override);
                   resultData.RedirectUrl = FininshedOrRunCustomInstallingEvent(moduleName);
               });

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
                    _moduleInstaller.RunEvent(moduleName, ControllerContext);
                    resultData.RedirectUrl = Url.Action("InstallComplete", ControllerContext.RequestContext.AllRouteValues());
                });
            }

            return Json(data);
        }
        public virtual ActionResult InstallComplete(string moduleName, string @return)
        {
            return View();
        }

        public virtual ActionResult AbortInstallation(string moduleName)
        {
            // to stop the installation. Do the clear job, remove the unziped module area folder.
            if (!string.IsNullOrEmpty(moduleName))
            {
                _moduleUninstaller.RemoveAssemblies(moduleName);
                _moduleUninstaller.DeleteModuleArea(moduleName);
            }
            return View();
        }
        #endregion

        #region Uninstall
        public virtual ActionResult GoingToUninstall(string uuid)
        {
            ModuleInfo moduleInfo = ModuleInfo.Get(uuid);

            var sites = Manager.AllSitesInModule(uuid);
            if (sites.Count() > 0)
            {
                return View(sites.Select(it => new SiteModuleRelationModel(it)));
            }
            if (!string.IsNullOrEmpty(moduleInfo.UninstallingTemplate))
            {
                return RedirectToAction("OnUninstalling", ControllerContext.RequestContext.AllRouteValues());
            }
            else
            {
                return RedirectToAction("DeleteModuleFiles", ControllerContext.RequestContext.AllRouteValues());
            }
        }

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
                    _moduleUninstaller.RunEvent(uuid, ControllerContext);
                    resultData.RedirectUrl = Url.Action("DeleteModuleFiles", ControllerContext.RequestContext.AllRouteValues());
                });
            }

            return Json(data);
        }
        public virtual ActionResult DeleteModuleFiles(string uuid)
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult DeleteModuleFiles(string uuid, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (!string.IsNullOrEmpty(uuid))
                {
                    _moduleUninstaller.RemoveAssemblies(uuid);
                    _moduleUninstaller.DeleteModuleArea(uuid);
                }
                resultData.RedirectUrl = Url.Action("UninstallComplete", ControllerContext.RequestContext.AllRouteValues());
            });
            return Json(data);
        }

        public virtual ActionResult UninstallComplete(string moduleName, string @return)
        {
            return View();
        }
        #endregion

        [HttpPost]
        public virtual ActionResult Exclude(string uuid, SiteModuleRelationModel[] model, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model)
                    {
                        Manager.RemoveSiteFromModule(uuid, item.UUID);
                    }

                    resultData.RedirectUrl = @return;
                }
            });
            return Json(data);
        }
        #region Relation
        public virtual ActionResult Relation(string uuid)
        {
            var model = Manager.AllSitesInModule(uuid).Select(site => new RelationModel()
            {
                DisplayName = site.FriendlyName,
                ObjectUUID = site.FullName,
                RelationObject = site,
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
