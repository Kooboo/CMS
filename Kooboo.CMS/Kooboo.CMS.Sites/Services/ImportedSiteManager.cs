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
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Sites.Services
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ImportedSiteManager))]
    public class ImportedSiteManager : ItemTemplateManager
    {
        protected override string BasePath
        {
            get
            {
                var baseDir = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IBaseDir>();
                return Path.Combine(baseDir.Cms_DataPhysicalPath, "ImportedSites");
            }
        }
    }
}
