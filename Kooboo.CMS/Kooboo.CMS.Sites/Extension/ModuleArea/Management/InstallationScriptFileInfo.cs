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

using Kooboo.Globalization;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    public class InstallationScriptFileInfo
    {
        #region .ctor
        public InstallationScriptFileInfo(string fullPath)
        {
            this.FullPath = fullPath;
            this.FileName = Path.GetFileNameWithoutExtension(FileName);
            this.FileType = Path.GetExtension(FileName);
            string[] fromToArr = FileName.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            if (fromToArr.Length == 1)
            {
                this.VersionRange = new VersionRange(fromToArr[0]);
            }
            else
            {
                this.VersionRange = new VersionRange(fromToArr[0], fromToArr[1]);
            }
        }
        #endregion

        #region Properties
        public string FullPath { get; private set; }
        public string FileName { get; private set; }
        public string FileType { get; private set; }
        public VersionRange VersionRange { get; private set; }

        #endregion
    }
}
