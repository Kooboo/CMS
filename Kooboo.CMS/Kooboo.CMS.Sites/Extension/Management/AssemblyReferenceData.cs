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
        public List<string> UserList { get; set; }
    }
}
