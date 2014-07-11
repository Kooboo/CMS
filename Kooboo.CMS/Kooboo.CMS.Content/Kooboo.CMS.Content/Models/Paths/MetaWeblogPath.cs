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
using Kooboo.Common.Web;


namespace Kooboo.CMS.Content.Models.Paths
{
    public class MetaWeblogPath : IPath
    {
        public MetaWeblogPath(Folder folder)
        {
            var folderPath = new FolderPath(folder);
            this.PhysicalPath = Path.Combine(folderPath.PhysicalPath, PATH_NAME);
            this.VirtualPath = UrlUtility.Combine(folderPath.VirtualPath, PATH_NAME);            
        }
        private static string PATH_NAME = "--metaweblog";
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
