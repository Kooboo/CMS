using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Kooboo.CMS.Sites.Models
{
    public class Profile : Dynamic.DynamicDictionary
    {
        public Profile()
        {

        }
    }
    [DataContract]
    public partial class User : IPersistable
    {
        public class DataFilePath
        {
            public DataFilePath(User user)
            {
                this.PhysicalPath = Path.Combine(GetBasePath(user.Site), user.UserName + ".config");
            }
            public static string GetBasePath(Site site)
            {
                return Path.Combine(site.PhysicalPath, "Users");
            }
            public string PhysicalPath { get; private set; }
        }
        public Site Site { get; set; }
        [DataMember(Order = 1)]
        public string UserName { get; set; }
        [DataMember(Order = 3)]
        public List<string> Roles { get; set; }
        [DataMember(Order = 5)]
        public Profile Profile { get; set; }        
    }

    public partial class User : IPersistable
    {
        #region IPersistable

        private bool isDummy = true;
        public bool IsDummy
        {
            get
            {
                return isDummy;
            }
            set
            {
                isDummy = value;
            }
        }

        public void Init(IPersistable source)
        {
            isDummy = false;
            this.Site = ((User)source).Site;
        }

        public void OnSaved()
        {
            isDummy = false;
        }

        public void OnSaving()
        {

        }

        public string DataFile
        {
            get { return new DataFilePath(this).PhysicalPath; }
        }
        #endregion
    }
}
