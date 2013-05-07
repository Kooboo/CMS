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
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Content.Services
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(TextFolderManager))]
    public class TextFolderManager : FolderManager<TextFolder>
    {
        public TextFolderManager(ITextFolderProvider provider) : base(provider) { }


        public IEnumerable<TextFolder> ChildFoldersWithSameSchema(TextFolder parentFolder)
        {
            parentFolder = parentFolder.AsActual();
            return ChildFolders(parentFolder)
                .Select(it => it.AsActual())
                .Where(it => it.SchemaName.EqualsOrNullEmpty(parentFolder.SchemaName, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        public IEnumerable<TextFolder> AllChildFoldersWithSameSchema(TextFolder parentFolder, List<TextFolder> results = null)
        {
            if (results == null)
            {
                results = new List<TextFolder>();
            }
            var children = ChildFoldersWithSameSchema(parentFolder);
            foreach (var c in children)
            {
                results.Add(c);
                AllChildFoldersWithSameSchema(c, results);
            }
            return results;
        }
    }
}
