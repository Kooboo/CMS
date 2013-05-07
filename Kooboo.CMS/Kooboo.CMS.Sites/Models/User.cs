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
using System.IO;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Collections;

namespace Kooboo.CMS.Sites.Models
{
    public class Profile : DynamicDictionary
    {
        public Profile()
        {

        }
    }
    [DataContract]
    public partial class User 
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

    public partial class User : ISiteObject, IFilePersistable, IPersistable, IIdentifiable
    {
        #region IPersistable
        public string UUID
        {
            get
            {
                return this.UserName;
            }
            set
            {
                this.UserName = value;
            }
        }

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
