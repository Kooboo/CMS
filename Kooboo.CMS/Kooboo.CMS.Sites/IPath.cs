using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites
{
    public interface IPath
    {
        string PhysicalPath { get; }
        string VirtualPath { get; }
    }

    public static class PathEx
    {
        public static string BasePath = "Cms_Data";
    }
}
