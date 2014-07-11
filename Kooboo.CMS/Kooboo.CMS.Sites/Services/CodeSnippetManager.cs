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
using System.IO;
using Kooboo.Common.Web;
using Kooboo.Common.IO;

namespace Kooboo.CMS.Sites.Services
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(CodeSnippetManager))]
    public class CodeSnippetManager
    {
        protected string BasePath
        {
            get
            {
                return AreaHelpers.CombineAreaFilePhysicalPath("Sites", "CodeSnippets");
            }
        }
        protected virtual string FileExtension
        {
            get
            {
                return ".txt";
            }
        }

        public virtual CodeSnippetGroup All(string viewEngine)
        {
            var physicalPath = Path.Combine(BasePath, viewEngine);
            CodeSnippetGroup root = new CodeSnippetGroup() { Name = "root", Parent = null, ViewEngine = viewEngine };
            if (Directory.Exists(physicalPath))
            {
                root.ChildGroups = AllGroups(viewEngine, root, physicalPath);
                root.CodeSnippets = AllCodeSnippet(root, physicalPath);
            }
            return root;
        }
        private IEnumerable<CodeSnippetGroup> AllGroups(string viewEngine, CodeSnippetGroup parent, string physicalPath)
        {
            var dir = Directory.GetDirectories(physicalPath);
            if (dir.Length != 0)
            {
                foreach (var item in dir.ExcludeSvn())
                {
                    var group = new CodeSnippetGroup()
                    {
                        ViewEngine = viewEngine,
                        Name = Path.GetFileNameWithoutExtension(item),
                        Parent = parent
                    };
                    group.ChildGroups = AllGroups(viewEngine, parent, item);
                    group.CodeSnippets = AllCodeSnippet(group, item);
                    yield return group;
                }
            }
        }

        private IEnumerable<CodeSnippet> AllCodeSnippet(CodeSnippetGroup group, string physicalPath)
        {
            return Directory.EnumerateFiles(physicalPath, "*" + FileExtension).Select(it =>
                new CodeSnippet()
                {
                    Group = group,
                    ViewEngine = group.ViewEngine,
                    Name = Path.GetFileNameWithoutExtension(it),
                    Code = IOUtility.ReadAsString(it)
                });
        }       
    }
}
