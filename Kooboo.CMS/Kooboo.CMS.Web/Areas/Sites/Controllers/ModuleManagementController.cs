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
        IModuleReinstaller _moduleReinstaller;
        IModuleVersioning _moduleVersioning;
        public ModuleManagementController(ModuleManager moduleManager, IModuleInstaller moduleInstaller, IModuleUninstaller moduleUninstaller,
            IModuleReinstaller moduleReinstaller, IModuleVersioning moduleVersioning)
        {
            Manager = moduleManager;
            _moduleInstaller = moduleInstaller;
            _moduleUninstaller = moduleUninstaller;
            _moduleReinstaller = moduleReinstaller;
            _moduleVersioning = moduleVersioning;
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
                    var errorMessage = _moduleInstaller.Unzip(ref moduleName, moduleFile.InputStream, User.Identity.Name);

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
        public virtual ActionResult CopyAssemblyFiles(string moduleName, string type)
        {
            var conflictedAssemblies = _moduleInstaller.CheckConflictedAssemblyReferences(moduleName);
            if (conflictedAssemblies.Count() > 0)
            {
                return View(conflictedAssemblies);
            }
            else
            {
                _moduleInstaller.CopyAssemblies(moduleName, false);
                return Redirect(FininshedOrRunCustomInstallingEvent(moduleName, type));
            }
        }
        private string FininshedOrRunCustomInstallingEvent(string moduleName, string type)
        {
            var moduleInfo = ModuleInfo.Get(moduleName);
            if (!string.IsNullOrEmpty(type) && type.ToLower() == "reinstall")
            {
                return Url.Action("OnReinstalling", ControllerContext.RequestContext.AllRouteValues().Merge("ModuleName", moduleName));
            }
            else
            {
                return Url.Action("OnInstalling", ControllerContext.RequestContext.AllRouteValues().Merge("ModuleName", moduleName));
            }
        }
        [HttpPost]
        public virtual ActionResult CopyAssemblyFiles(string moduleName, bool @override, string type)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
               {
                   _moduleInstaller.CopyAssemblies(moduleName, @override);
                   resultData.RedirectUrl = FininshedOrRunCustomInstallingEvent(moduleName, type);
               });

            return Json(data);
        }

        public virtual ActionResult OnInstalling(string moduleName)
        {
            ViewBag.InstallationContext = _moduleVersioning.GetLastestInstallation(moduleName);
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

        #region Reinstall
        [HttpGet]
        public virtual ActionResult Reinstall(string uuid, string installationFileName)
        {
            if (!string.IsNullOrEmpty(installationFileName))
            {
                using (var moduleStream = _moduleVersioning.GetInstallationStream(uuid, installationFileName))
                {
                    var errorMessage = _moduleReinstaller.Unzip(uuid, moduleStream, User.Identity.Name);
                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        return Redirect(Url.Action("CopyAssemblyFiles", ControllerContext.RequestContext.AllRouteValues().Merge("ModuleName", uuid).Merge("type", "Reinstall")));
                    }
                    else
                    {
                        ViewBag.ErrorMessage = errorMessage;
                        return View();
                    }

                }
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public virtual ActionResult Reinstall(string uuid, InstallModuleModel installModel, string @return)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    var moduleFile = this.Request.Files["ModuleFile"];

                    var errorMessage = _moduleReinstaller.Unzip(uuid, moduleFile.InputStream, User.Identity.Name);

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        data.Success = false;
                        data.AddMessage(errorMessage);
                    }
                    else
                    {
                        //copy the assembly files to bin folder.
                        data.RedirectUrl = Url.Action("CopyAssemblyFiles", ControllerContext.RequestContext.AllRouteValues().Merge("ModuleName", uuid).Merge("type", "Reinstall"));
                    }
                });
            }

            return Json(data);
        }

        public virtual ActionResult OnReinstalling(string moduleName)
        {
            ViewBag.InstallationContext = _moduleVersioning.GetLastestInstallation(moduleName);
            ModuleInfo moduleInfo = ModuleInfo.Get(moduleName);
            return View(moduleInfo);
        }
        [HttpPost]
        public virtual ActionResult OnReinstalling(string moduleName, FormCollection form)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    _moduleReinstaller.RunEvent(moduleName, ControllerContext);
                    resultData.RedirectUrl = Url.Action("ReinstallComplete", ControllerContext.RequestContext.AllRouteValues());
                });
            }

            return Json(data);
        }
        public virtual ActionResult ReinstallComplete(string moduleName, string @return)
        {
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
            //if (!string.IsNullOrEmpty(moduleInfo.UninstallingTemplate))
            //{
            return RedirectToAction("OnUninstalling", ControllerContext.RequestContext.AllRouteValues());
            //}
            //else
            //{
            //    return RedirectToAction("DeleteModuleFiles", ControllerContext.RequestContext.AllRouteValues());
            //}
        }

        public virtual ActionResult OnUninstalling(string uuid)
        {
            ViewBag.InstallationContext = _moduleVersioning.GetLastestInstallation(uuid);
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
                    _moduleUninstaller.RemoveVersions(uuid);
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

        #region Exclude
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
        #endregion

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

        #region Versioning
        public ActionResult Versions(string uuid)
        {
            return View(_moduleVersioning.AllInstallationLogs(uuid));
        }
        #endregion
    }
}
