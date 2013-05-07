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
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Models
{
    [DataContract]
    public partial class UrlKeyMap
    {
        public UrlKeyMap()
        { }
        public UrlKeyMap(Site site, string key)
        {
            this.Site = site;
            this.Key = key;
        }
        [DataMember(Order = 1)]
        public string Key { get; set; }

        public string Name
        {
            get
            {
                return this.Key;
            }
            set
            {
                this.Key = value;
            }
        }

        [DataMember(Order = 3)]
        public string PageFullName { get; set; }

    }


    public partial class UrlKeyMap : ISiteObject, IFilePersistable, IPersistable, IIdentifiable
    {
        public Site Site { get; set; }

        public string DataFile
        {
            get { return new UrlKeyMapsFile(this.Site).PhysicalPath; }
        }
        #region override object
        public static bool operator ==(UrlKeyMap obj1, UrlKeyMap obj2)
        {
            if (object.Equals(obj1, obj2) == true)
            {
                return true;
            }
            if (object.Equals(obj1, null) == true || object.Equals(obj2, null) == true)
            {
                return false;
            }
            return obj1.Equals(obj2);
        }
        public static bool operator !=(UrlKeyMap obj1, UrlKeyMap obj2)
        {
            return !(obj1 == obj2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                var urlKey = (UrlKeyMap)obj;
                if (this.Key.EqualsOrNullEmpty(urlKey.Key, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return base.Equals(obj);
        }
        public override string ToString()
        {
            return Key;
        }
        #endregion
        public string UUID
        {
            get
            {
                return this.Key;
            }
            set
            {
                this.Key = value;
            }
        }
        private bool isDummy = true;
        public bool IsDummy
        {
            get { return isDummy; }
            set { isDummy = value; }
        }

        public void Init(IPersistable source)
        {
            isDummy = false;
            this.Site = ((UrlKeyMap)source).Site;
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
