#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Services
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(SiteTemplateManager))]
    public class SiteTemplateManager : ItemTemplateManager
    {
        string basePath;
        public SiteTemplateManager(IBaseDir baseDir)
        {
            basePath = Path.Combine(baseDir.Cms_DataPhysicalPath, "SiteTemplates");
        }
        protected override string BasePath
        {
            get { return basePath; }
        }
    }
}
