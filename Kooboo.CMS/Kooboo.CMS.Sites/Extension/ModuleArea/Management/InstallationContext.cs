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
using System.Text;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    public class InstallationContext
    {
        #region .ctor
        public InstallationContext()
        {

        }

        public InstallationContext(string moduleName, string targetVersion, DateTime utcDateTime)
        {
            this.ModuleName = moduleName;
            this.VersionRange = new VersionRange(targetVersion);
            this.UtcDatetime = utcDateTime;
        }
        public InstallationContext(string moduleName, string sourceVersion, string targetVersion, DateTime utcDateTime)
        {
            this.ModuleName = moduleName;
            this.VersionRange = new VersionRange(sourceVersion, targetVersion);
            this.UtcDatetime = utcDateTime;
        }
        #endregion
        public string ModuleName { get; set; }
        public string User { get; set; }
        public DateTime UtcDatetime { get; set; }
        public VersionRange VersionRange { get; set; }

        public string InstallationFileName { get; set; }
    }
}
