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
                        data.RedirectUrl = @return;
                    }
                });
            }

            return Json(data);
        }
        #endregion

        #region Uninstall
        [HttpPost]
        public virtual ActionResult Uninstall(ModuleInfo[] model, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                foreach (var item in model)
                {
                    if (!string.IsNullOrEmpty(item.ModuleName))
                    {
                        Manager.Uninstall(item.ModuleName);
                    }
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
