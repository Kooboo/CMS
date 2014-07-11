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
    public class DataPath : IPath
    {
        static string DirName = "Data";
        public DataPath(string repositoryName)
        {
            var repository = new Repository(repositoryName);
            var repositoryPath = new RepositoryPath(repository);
            this.PhysicalPath = Path.Combine(repositoryPath.PhysicalPath, DirName);
            this.VirtualPath = UrlUtility.Combine(repositoryPath.VirtualPath, DirName);
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
            get { throw new NotImplementedException(); }
        }

        public bool Exists()
        {
            throw new NotImplementedException();
        }

        public void Rename(string newName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
