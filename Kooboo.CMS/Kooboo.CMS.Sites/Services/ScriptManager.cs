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

namespace Kooboo.CMS.Sites.Services
{
    public class ScriptBaseDirectory : DirectoryResource
    {
        public ScriptBaseDirectory(Site site)
            : base(site, ScriptFile.PATH_NAME)
        {

        }
        protected ScriptBaseDirectory()
            : base()
        { }
        protected ScriptBaseDirectory(string physicalPath)
            : base(physicalPath)
        {
        }
        protected ScriptBaseDirectory(Site site, string name)
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
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(FileManager), Key = "scripts")]
    public class ScriptManager : FileManager
    {
        protected override Models.DirectoryResource GetRootDir(Site site)
        {
            return new ScriptBaseDirectory(site);
        }

        public override string Type
        {
            get { return "Scripts"; }
        }

        public virtual IEnumerable<FileEntry> AllScripts(Site site, string dirRelativePath)
        {
            List<FileEntry> list = new List<FileEntry>();
            while (site != null)
            {
                var files = GetFiles(site, dirRelativePath);
                foreach (var item in files)
                {
                    if (!list.Contains(item, new FileEntry.FileNameEqualityComparer()))
                    {
                        list.Add(item);
                    }
                }
                site = site.Parent;
            }

            return list;
        }
    }
}
