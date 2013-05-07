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
using Kooboo.CMS.Content.Models;
using System.IO;
using Kooboo.Web.Url;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Content.Models.Paths
{
    public class RepositoryPath : IPath
    {
        public static string PATH_NAME = "Contents";
        public static string BasePhysicalPath { get; private set; }
        public static string BaseVirtualPath { get; private set; }
        static RepositoryPath()
        {
            var baseDir = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IBaseDir>();
            BasePhysicalPath = Path.Combine(baseDir.Cms_DataPhysicalPath, PATH_NAME);
            BaseVirtualPath = UrlUtility.Combine(baseDir.Cms_DataVirtualPath, PATH_NAME);
        }
        public RepositoryPath(Repository repository)
        {
            this.PhysicalPath = Path.Combine(BasePhysicalPath, repository.Name);
            this.VirtualPath = UrlUtility.Combine(BaseVirtualPath, repository.Name);
            this.SettingFile = Path.Combine(PhysicalPath, PathHelper.SettingFileName);

        }

        #region IPath Members

        public string PhysicalPath
        {
            get;
            private set;
        }

        public string VirtualPath
        {
            get;
            private set;
        }
        public string SettingFile
        {
            get;
            private set;
        }

        #endregion

        #region IPath Members


        public bool Exists()
        {
            return Directory.Exists(this.PhysicalPath);
        }

        #endregion

        #region IPath Members


        public void Rename(string newName)
        {
            IO.IOUtility.RenameDirectory(this.PhysicalPath, @newName);
        }

        #endregion
    }
}
