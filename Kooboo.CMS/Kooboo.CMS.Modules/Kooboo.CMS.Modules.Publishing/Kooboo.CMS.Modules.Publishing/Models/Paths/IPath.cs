using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Models.Paths
{
    public interface IPath<T>
    {
        string PhysicalPath { get; }
        string VirtualPath { get; }
        string DataFile { get; }
    }
}
