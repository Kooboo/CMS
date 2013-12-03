#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public interface IModuleEvents
    {
        /// <summary>
        /// Called when [installing].
        /// Will only enabled when the module have install template(The InstallingTemplate property in ModuleInfo)
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        void OnInstalling(ControllerContext controllerContext);

        /// <summary>
        /// Called when [uninstalling].
        /// Will only enabled when the module have install template(The InstallingTemplate property in ModuleInfo)
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        void OnUninstalling(ControllerContext controllerContext);

        void OnIncluded(Site site);
        void OnExcluded(Site site);
    }
}
