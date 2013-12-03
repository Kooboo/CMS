using Kooboo.IO;
using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    public interface IModuleVersioning
    {
        void SaveModuleVersion(ModuleStreamEntry moduleStreamEntry);
        Stream GetModuleByVersion(string moduleName, string verion);
    }
}
