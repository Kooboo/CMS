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
using Kooboo.Web.Url;

namespace Kooboo.CMS.Content.Models.Paths
{
    public class MediaContentPath : IPath
    {
        public MediaContentPath(MediaContent content)
        {
            var mediaFolder = (MediaFolder)(content.GetFolder());
            var folderPath = new FolderPath(mediaFolder);
            this.PhysicalPath = Path.Combine(folderPath.PhysicalPath, content.FileName);
            this.SettingFile = this.PhysicalPath;
            this.VirtualPath = UrlUtility.Combine(folderPath.VirtualPath, content.FileName);
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

        public bool Exists()
        {
           return File.Exists(this.PhysicalPath);
        }

        public void Rename(string newName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
