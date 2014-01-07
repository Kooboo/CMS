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
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    public class InstallingModule
    {
        public InstallingModule(string moduleName, IPath tempInstallationPath)
        {
            this.ModuleName = moduleName;
            this.TempInstallationPath = tempInstallationPath;
        }
        public string ModuleName { get; private set; }
        public IPath TempInstallationPath { get; private set; }
        public ModuleInfo ModuleInfo
        {
            get
            {
                var moduleCnf = Path.Combine(this.TempInstallationPath.PhysicalPath, Kooboo.CMS.Sites.Extension.ModuleArea.ModuleInfo.ModuleInfoFileName);
                using (var fs = new FileStream(moduleCnf, FileMode.Open))
                {
                    return Kooboo.CMS.Sites.Extension.ModuleArea.ModuleInfo.Get(fs);
                }
            }
        }
    }
}
