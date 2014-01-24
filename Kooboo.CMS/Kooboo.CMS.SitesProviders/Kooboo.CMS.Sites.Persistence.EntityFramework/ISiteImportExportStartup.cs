using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework
{
    public interface ISiteImportExportStartup
    {
        void ExportToDisk(Kooboo.CMS.Sites.Models.Site site);
    }
}
