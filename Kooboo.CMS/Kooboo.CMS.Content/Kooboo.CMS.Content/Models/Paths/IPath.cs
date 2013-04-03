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
