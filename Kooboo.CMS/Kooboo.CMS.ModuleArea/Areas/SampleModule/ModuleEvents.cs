﻿#region License
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
using Kooboo.CMS.ModuleArea.Areas.SampleModule.Models;
using Kooboo.CMS.Sites.Models;
namespace Kooboo.CMS.ModuleArea.Areas.SampleModule
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IModuleEvents), Key = ModuleAreaRegistration.ModuleName)]
    public class ModuleEvents : IModuleEvents
    {
        public void OnExcluded(Site site)
        {
            // Add code here that will be executed when the module was excluded to the site.
        }

        public void OnIncluded(Site site)
        {
            // Add code here that will be executed when the module was included to the site.
        }


        public void OnInstalling(ControllerContext controllerContext)
        {
            var moduleInfo = ModuleInfo.Get(ModuleAreaRegistration.ModuleName);
            var installModel = new InstallModel();
            Kooboo.CMS.Sites.Extension.ModelBindHelper.BindModel<InstallModel>(installModel, controllerContext);

            moduleInfo.DefaultSettings.CustomSettings["DatabaseServer"] = installModel.DatabaseServer;
            moduleInfo.DefaultSettings.CustomSettings["UserName"] = installModel.UserName;
            moduleInfo.DefaultSettings.CustomSettings["Password"] = installModel.Password;
            ModuleInfo.Save(moduleInfo);

            // Add code here that will be executed when the module installing.
        }

        public void OnUninstalling(ControllerContext controllerContext)
        {
            // Add code here that will be executed when the module uninstalling.
        }
    }
}
