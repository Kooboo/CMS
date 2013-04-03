using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.CMS.Account.Models
{
    [DataContract]
    public class Role : IPersistable
    {
        public Role()
        {
            Permissions = new List<Permission>();
            //Users = new string[0];
        }

        [DataMember(Order = 1)]
        public string Name { get; set; }
        [DataMember(Order = 3)]
        public List<Permission> Permissions { get; set; }
        //public IEnumerable<string> Users { get; set; }


        //public void AddUser(string userName)
        //{
        //    Users = Users.Concat(new[] { userName });
        //}
        //public void RemoveUser(string userName)
        //{
        //    Users = Users.Where(it => string.Compare(it, userName, true) != 0);
        //}
        //public bool IsUserInRole(string userName)
        //{
        //    return this.Users.Any(it => string.Compare(it, userName, true) == 0);
        //}
        public bool HasPermission(Permission permission)
        {
            return this.Permissions.Any(it => permission == it);
        }


        private bool isDummy = true;
        public bool IsDummy
        {
            get { return isDummy; }
            private set { isDummy = value; }
        }

        public void Init(IPersistable source)
        {
            IsDummy = false;
        }

        public void OnSaved()
        {
            isDummy = false;
        }

        public void OnSaving()
        {

        }
    }
}
