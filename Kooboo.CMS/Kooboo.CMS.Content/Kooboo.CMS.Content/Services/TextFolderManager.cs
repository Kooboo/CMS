using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Services
{
    public class TextFolderManager : FolderManager<TextFolder>
    {
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
        //public IEnumerable<TextFolder> AllTextFolder(Repository repository, string selfFullName = "")
        //{
        //    var all = this.All(repository, "");
        //    return all.Where(o => o.AsActual() is TextFolder && o.FullName != selfFullName).Select(o => (TextFolder)o.AsActual());
        //}
    }
}
