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

namespace Kooboo.CMS.Content.Models.Paths
{
    public interface IPath
    {
        string PhysicalPath { get; }
        string VirtualPath { get; }
        string SettingFile { get; }

        bool Exists();
        void Rename(string newName);
    }
}
