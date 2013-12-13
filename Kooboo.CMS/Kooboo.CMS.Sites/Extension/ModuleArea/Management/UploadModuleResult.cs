#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Extension.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    public class UploadModuleResult
    {
        public UploadModuleResult()
        {
            this.IsValid = true;
        }
        public bool IsValid { get; set; }
        public string ModuleName { get; set; }
        public ModuleInfo SourceModuleInfo { get; set; }
        public ModuleInfo TargetModuleInfo { get; set; }
        public IEnumerable<ConflictedAssemblyReference> ConflictedAssemblies { get; set; }
        public IPath TempInstallationPath { get; set; }
    }
}
