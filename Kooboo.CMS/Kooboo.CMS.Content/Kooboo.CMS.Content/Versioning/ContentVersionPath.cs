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
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Content.Models;
using System.IO;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Content.Versioning
{
    public class ContentVersionPath : IPath
    {
        public ContentVersionPath(TextContent content)
        {
            var contentPath = new TextContentPath(content);
            var basePath = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<Kooboo.CMS.Common.IBaseDir>();
            var versionPath = Path.Combine(basePath.Cms_DataPhysicalPath, VersionPathName);
            this.PhysicalPath = contentPath.PhysicalPath.Replace(basePath.Cms_DataPhysicalPath, versionPath);            
        }
        private static string VersionPathName = "Versions";
        #region IPath Members

        public string PhysicalPath
        {
            get;
            private set;
        }

        public string VirtualPath
        {
            get
            { throw new NotImplementedException(); }
        }

        public string SettingFile
        {
            get;
            private set;
        }

        public bool Exists()
        {
            return Directory.Exists(this.PhysicalPath);
        }

        public void Rename(string newName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
