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

namespace Kooboo.CMS.Content.Models.Paths
{
    public class BroadcastingPath : IPath
    {
        static string DIR = "Broadcasting";

        public BroadcastingPath(Repository repository)
        {
            var repositoryPath = new RepositoryPath(repository);
            this.PhysicalPath = Path.Combine(repositoryPath.PhysicalPath, DIR);
        }
        public string PhysicalPath
        {
            get;
            private set;

        }

        public string VirtualPath
        {
            get { throw new NotImplementedException(); }
        }

        public string SettingFile
        {
            get { throw new NotImplementedException(); }
        }

        public bool Exists()
        {
            return Directory.Exists(this.PhysicalPath);
        }

        public void Rename(string newName)
        {
            throw new NotImplementedException();
        }
    }
}
