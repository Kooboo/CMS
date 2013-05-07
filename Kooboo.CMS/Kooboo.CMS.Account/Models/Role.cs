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
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Account.Models
{
    [DataContract]
    public class Role : IPersistable,IIdentifiable
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

        public bool HasPermission(Permission permission)
        {
            return this.Permissions.Any(it => permission == it);
        }


        #region IPersistable
        public string UUID
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
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
        #endregion


    }
}
