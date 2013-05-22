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
using System.IO;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public class ModuleItemPath : IPath
    {
        #region .ctor
        public ModuleItemPath(string moduleName, string entryName)
        {
            ModulePath modulePath = new ModulePath(moduleName);

            EntryName = entryName;
            PhysicalPath = Path.Combine(modulePath.PhysicalPath, EntryName);
            VirtualPath = UrlUtility.Combine(modulePath.VirtualPath, EntryName);
        }
        public ModuleItemPath(ModuleItemPath parent, string entryName)
        {
            EntryName = entryName;
            PhysicalPath = Path.Combine(parent.PhysicalPath, EntryName);
            VirtualPath = UrlUtility.Combine(parent.VirtualPath, EntryName);
        }
        #endregion

        #region Properties
        public string EntryName { get; private set; }
        public string PhysicalPath { get; private set; }
        public string VirtualPath { get; private set; }
        #endregion
    }
}
