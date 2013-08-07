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

namespace Kooboo.CMS.Content.Services
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(RepositoryTemplateManager))]
    public class RepositoryTemplateManager : ItemTemplateManager
    {
        private IBaseDir _baseDir;
        public RepositoryTemplateManager(IBaseDir baseDir)
        {
            _baseDir = baseDir;
        }
        protected override string TemplatePath
        {
            get { return Path.Combine(_baseDir.Cms_DataPhysicalPath, "ImportedContents"); }
        }
    }
}
