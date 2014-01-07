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
using Kooboo.Globalization;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.RequiredLogOnAttribute(RequiredAdministrator = true, Order = 99)]
    public class ModuleManagementController : Kooboo.CMS.Sites.AreaControllerBase
    {
        #region .ctor
        IModuleInstaller _moduleInstaller;
        IModuleUninstaller _moduleUninstaller;
        IModuleReinstaller _moduleReinstaller;
        IInstallationFileManager _moduleVersioning;
        public ModuleManagementController(ModuleManager moduleManager, IModuleInstaller moduleInstaller, IModuleUninstaller moduleUninstaller,
            IModuleReinstaller moduleReinstaller, IInstallationFileManager moduleVersioning)
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
        public virtual ActionResult Install(string moduleName, string @return)
        {
            if (this.Request.Files.Count > 0)
            {
                var moduleFile = this.Request.Files["ModuleFile"];

                moduleName = System.IO.Path.GetFileNameWithoutExtension(moduleFile.FileName);
                var result = _moduleInstaller.Upload(moduleName, moduleFile.InputStream, User.Identity.Name);
                if (result.IsValid == false)
                {
                    ViewData.ModelState.AddModelError("ModuleFile", "Invalid module file".Localize());
                }
                if (result.ModuleExists == true)
                {
                    ViewData.ModelState.AddModelError("ModuleFile", "The module already exists, please use 'Reinstall' option to upgrade the module.".Localize());
                }
                return View(result);
            }
            else
            {
                var data = new JsonResultData(ModelState);
                if (ModelState.IsValid)
                {
                    data.RunWithTry((resultData) =>
                    {
                        data.RedirectUrl = Url.Action("RunInstallation", ControllerContext.RequestContext.AllRouteValues().Merge("ModuleName", moduleName));
                    });
                }

                return Json(data);
            }
        }

        [HttpPost]
        public virtual ActionResult CopyInstallationFiles(string moduleName)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    _moduleInstaller.CopyAssemblies(moduleName, true);
                });
            }

            return Json(data);
        }
        public virtual ActionResult RunInstallation(string moduleName)
        {
            var tempInstallationPath = _moduleInstaller.GetTempInstallationPath(moduleName);
            return View(new InstallingModule(moduleName, tempInstallationPath));
        }
        [HttpPost]
        public virtual ActionResult RunInstallation(string moduleName, FormCollection form)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    _moduleInstaller.RunInstallation(moduleName, ControllerContext, User.Identity.Name);
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

        public virtual ActionResult GoingToUninstall(string uuid)
        {
            ModuleInfo moduleInfo = ModuleInfo.Get(uuid);

            var sites = Manager.AllSitesInModule(uuid);
            if (sites.Count() > 0)
            {
                return View(sites.Select(it => new SiteModuleRelationModel(it)));
            }
            return RedirectToAction("Uninstall", ControllerContext.RequestContext.AllRouteValues());
        }
        public virtual ActionResult Uninstall(string uuid)
        {
            ModuleInfo moduleInfo = ModuleInfo.Get(uuid);
            return View(moduleInfo);
        }
        [HttpPost]
        public virtual ActionResult Uninstall(string uuid, FormCollection form)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    data.RedirectUrl = Url.Action("RunUninstallation", ControllerContext.RequestContext.AllRouteValues().Merge("ModuleName", uuid));
                });
            }

            return Json(data);
        }
        public virtual ActionResult RunUninstallation(string uuid)
        {
            ModuleInfo moduleInfo = ModuleInfo.Get(uuid);
            return View(moduleInfo);
        }
        [HttpPost]
        public virtual ActionResult RunUninstallation(string uuid, FormCollection form)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    _moduleUninstaller.RunUninstall(uuid, ControllerContext);
                    resultData.RedirectUrl = Url.Action("UninstallComplete", ControllerContext.RequestContext.AllRouteValues());
                });
            }

            return Json(data);
        }
        public virtual ActionResult UninstallComplete(string moduleName, string @return)
        {
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
                    var result = _moduleReinstaller.Upload(uuid, moduleStream, User.Identity.Name);
                    if (result.IsValid == false)
                    {
                        ViewData.ModelState.AddModelError("ModuleFile", "Invalid module file".Localize());
                    }
                    return View(result);
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public virtual ActionResult Reinstall(string uuid, string @return, FormCollection form)
        {
            if (this.Request.Files.Count > 0)
            {
                var moduleFile = this.Request.Files["ModuleFile"];

                uuid = System.IO.Path.GetFileNameWithoutExtension(moduleFile.FileName);
                var result = _moduleReinstaller.Upload(uuid, moduleFile.InputStream, User.Identity.Name);
                if (result.IsValid == false)
                {
                    ViewData.ModelState.AddModelError("ModuleFile", "Invalid module file".Localize());
                }
                return View(result);
            }
            else
            {
                var data = new JsonResultData(ModelState);
                if (ModelState.IsValid)
                {
                    data.RunWithTry((resultData) =>
                    {
                        data.RedirectUrl = Url.Action("RunReinstallation", ControllerContext.RequestContext.AllRouteValues().Merge("ModuleName", uuid));
                    });
                }

                return Json(data);
            }
        }

        [HttpPost]
        public virtual ActionResult CopyReinstallationFiles(string moduleName)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    _moduleReinstaller.CopyAssemblies(moduleName, true);
                });
            }

            return Json(data);

        }

        public virtual ActionResult RunReinstallation(string moduleName)
        {
            var tempInstallationPath = _moduleInstaller.GetTempInstallationPath(moduleName);
            return View(new InstallingModule(moduleName, tempInstallationPath));
        }
        [HttpPost]
        public virtual ActionResult RunReinstallation(string moduleName, FormCollection form)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    _moduleReinstaller.RunReinstallation(moduleName, ControllerContext, User.Identity.Name);
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
