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
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Persistence.FileSystem;
using System.IO;

namespace Kooboo.CMS.Sites.Services
{
    public class CustomFileBaseDirectory : DirectoryResource
    {
        public CustomFileBaseDirectory(Site site)
            : base(site, CustomFile.PATH_NAME)
        {

        }
        protected CustomFileBaseDirectory()
            : base()
        { }
        protected CustomFileBaseDirectory(string physicalPath)
            : base(physicalPath)
        {
        }
        protected CustomFileBaseDirectory(Site site, string name)
            : base(site, name)
        {
        }
        public override IEnumerable<string> RelativePaths
        {
            get { return new string[0]; }
        }

        public override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            return relativePaths;
        }
    }
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(FileManager), Key = "customfiles")]
    public class CustomFileManager : FileManager
    {
        protected override DirectoryResource GetRootDir(Site site)
        {
            return new CustomFileBaseDirectory(site);
        }

        public override string Type
        {
            get { return "CustomFiles"; }
        }
    }
}
