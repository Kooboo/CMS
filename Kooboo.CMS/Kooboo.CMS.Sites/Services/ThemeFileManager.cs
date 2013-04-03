using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Services
{
    public class ThemeFileManager : FileManager
    {
        protected override Models.DirectoryResource GetRootDir(Site site)
        {
            return new ThemeBaseDirectory(site);
        }
    }
}
