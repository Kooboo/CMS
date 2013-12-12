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
namespace Kooboo.CMS.VSExtensionTemplate.Areas.HelloWorld
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IModuleEvents), Key = ModuleAreaRegistration.ModuleName)]
    public class ModuleEvents : IModuleEvents
    {
        public void OnInstalling(ModuleContext moduleContext, ControllerContext controllerContext)
        {
            // Add code here that will be executed when the module installing.
            // Installing UI template is defined in the module.config
        }

        public void OnUninstalling(ModuleContext moduleContext, ControllerContext controllerContext)
        {
            // Add code here that will be executed when the module uninstalling.
            // To use custom UI during uninstalling, define the view location in the module.config
        }

        public void OnExcluded(ModuleContext moduleContext)
        {
            // Add code here that will be executed when the module was excluded to the site.
        }

        public void OnIncluded(ModuleContext moduleContext)
        {
            // Add code here that will be executed when the module was included to the site.
        }

        public void OnReinstalling(ModuleContext moduleContext, ControllerContext controllerContext, Sites.Extension.ModuleArea.Management.InstallationContext installationContext)
        {
            // Add code here that will be executed when the module reinstalling.
            // To use custom UI during reinstalling, define the view location in the module.config
        }
    }
}
