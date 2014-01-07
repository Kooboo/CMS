using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Extension.Management
{
    public interface IAssemblyReferences
    {
        IEnumerable<ConflictedAssemblyReference> Check(IEnumerable<string> assemblyFiles);

        void AddReference(string assemblyFile, string user);

        /// <summary>
        /// Remove the reference.
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="user"></param>
        /// <returns>No others used,available to remove.</returns>
        bool RemoveReference(string assemblyFile, string user);
    }
}
