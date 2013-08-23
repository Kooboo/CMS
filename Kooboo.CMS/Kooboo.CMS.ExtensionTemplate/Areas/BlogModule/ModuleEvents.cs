#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Extension.ModuleArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.ViewModels;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Persistence;
namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IModuleEvents), Key = ModuleAreaRegistration.ModuleName)]
    public class ModuleEvents : IModuleEvents
    {
        IDbInitializer _dbInitializer;
        public ModuleEvents(IDbInitializer dbInitializer)
        {
            _dbInitializer = dbInitializer;
        }

        public void OnExcluded(Site site)
        {
            // Add code here that will be executed when the module was excluded to the site.
        }

        public void OnIncluded(Site site)
        {
            // Add code here that will be executed when the module was included to the site.

            // API access to the local or public file path
           // var privatedatafolder = new ModulePathHelper(ModuleAreaRegistration.ModuleName, site).GetModuleLocalFilePath();

        }


        public void OnInstalling(ControllerContext controllerContext)
        {

            var installationModel = new InstallationModel();
            var connectString = controllerContext.HttpContext.Request.Form["ConnectionString"];
            if (Kooboo.CMS.Sites.Extension.ModelBindHelper.BindModel(installationModel, controllerContext))
            {
                try
                {
                    _dbInitializer.InitializeDb(installationModel.ConnectionString);
                    ModuleInfo moduleInfo = ModuleInfo.Get(ModuleAreaRegistration.ModuleName);
                    moduleInfo.DefaultSettings.CustomSettings["ConnectionString"] = installationModel.ConnectionString;
                    ModuleInfo.Save(moduleInfo);
                }
                catch (Exception e)
                {
                    controllerContext.Controller.ViewData.ModelState.AddModelError("ConnectionString", e);
                }
            }
        }

        public void OnUninstalling(ControllerContext controllerContext)
        {
            var installationModel = new InstallationModel();
            if (Kooboo.CMS.Sites.Extension.ModelBindHelper.BindModel(installationModel, controllerContext))
            {
                try
                {
                    _dbInitializer.DeleteDb(installationModel.ConnectionString);
                }
                catch (Exception e)
                {
                    controllerContext.Controller.ViewData.ModelState.AddModelError("ConnectionString", e);
                }
            }
        }
    }
}
