using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Extension.Management
{
    public class AssemblyReferenceData
    {
        public AssemblyReferenceData()
        {

        }
        public AssemblyReferenceData(string assemblyName, string version, params string[] users)
        {
            this.AssemblyName = assemblyName;
            this.Version = version;
            if (users != null)
            {
                this.UserList = new List<string>(users);
            }
            else
            {
                this.UserList = new List<string>();
            }
        }
        public string AssemblyName { get; set; }
        public string Version { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [system assembly].
        /// The assembly is built-in Kooboo CMS, can be updated via Kooboo CMS upgradation only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [system assembly]; otherwise, <c>false</c>.
        /// </value>
        public bool IsSystemAssembly { get; set; }
        public List<string> UserList { get; set; }
    }
}
