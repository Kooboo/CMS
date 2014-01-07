#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Extension.ModuleArea.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management.Events
{
    public interface IModuleReinstallingEvents
    {
        void OnReinstalling(ModuleContext moduleContext, ControllerContext controllerContext, InstallationContext installationContext);
    }
}
