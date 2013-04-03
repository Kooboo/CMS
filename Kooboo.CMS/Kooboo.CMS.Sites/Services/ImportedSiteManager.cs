using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Services
{
    public class ImportedSiteManager : ItemTemplateManager
    {
        protected override string BasePath
        {
            get { return Path.Combine(Settings.BaseDirectory, PathEx.BasePath, "ImportedSites"); }
        }
    }
}
