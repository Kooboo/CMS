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
using Kooboo.CMS.Sites.Extension.ModuleArea.Management;
using System.IO;
namespace KoobooModule.Areas.KoobooModule
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IModuleEvents), Key = ModuleAreaRegistration.ModuleName)]
    public class ModuleEvents : IModuleEvents
    {
        private void WriteText(ModuleContext moduleContext, string eventName)
        {
            var logFile = moduleContext.ModulePath.GetModuleSharedFilePath(@"logs.txt");
            var dir = Path.GetDirectoryName(logFile.PhysicalPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            System.IO.File.AppendAllLines(logFile.PhysicalPath, new[] { string.Format("Run '{0}' at {1}", eventName, DateTime.Now) });
        }
        public void OnInstalling(ModuleContext moduleContext, ControllerContext controllerContext)
        {
            // Add code here that will be executed when the module installing.
            // Installing UI template is defined in the module.config
            WriteText(moduleContext, "OnInstalling");
        }

        public void OnUninstalling(ModuleContext moduleContext, ControllerContext controllerContext)
        {
            // Add code here that will be executed when the module uninstalling.
            // To use custom UI during uninstalling, define the view location in the module.config
            WriteText(moduleContext, "OnUninstalling");
        }

        public void OnExcluded(ModuleContext moduleContext)
        {
            // Add code here that will be executed when the module was excluded to the site.
            WriteText(moduleContext, "OnExcluded");
        }

        public void OnIncluded(ModuleContext moduleContext)
        {
            // Add code here that will be executed when the module was included to the site.
            WriteText(moduleContext, "OnIncluded");
        }

        public void OnReinstalling(ModuleContext moduleContext, ControllerContext controllerContext, InstallationContext installationContext)
        {
            // Add code here that will be executed when the module reinstalling.
            // To use custom UI during reinstalling, define the view location in the module.config
            WriteText(moduleContext, "OnReinstalling");
        }
    }
}
