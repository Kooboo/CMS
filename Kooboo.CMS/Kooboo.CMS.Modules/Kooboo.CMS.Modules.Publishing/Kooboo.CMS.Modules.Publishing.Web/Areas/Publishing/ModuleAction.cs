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
namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IModuleEvents), Key = PublishingAreaRegistration.ModuleName)]
    public class ModuleAction : IModuleEvents
    {
        public void OnExcluded(Sites.Models.Site site)
        {
            // Add code here that will be executed when the module was excluded to the site.
        }

        public void OnIncluded(Sites.Models.Site site)
        {
            // Add code here that will be executed when the module was included to the site.
        }


        public void OnInstalling(ControllerContext controllerContext)
        {
            // Add code here that will be executed when the module installing.
        }

        public void OnUninstalling(ControllerContext controllerContext)
        {
            // Add code here that will be executed when the module uninstalling.
        }
    }
}
