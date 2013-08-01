#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Common.Persistence.Relational;
using Kooboo.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kooboo.CMS.Membership.Models
{
    #region override object
    public partial class MembershipConnect
    {
        public static bool operator ==(MembershipConnect obj1, MembershipConnect obj2)
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
        public static bool operator !=(MembershipConnect obj1, MembershipConnect obj2)
        {
            return !(obj1 == obj2);
        }
        public override string ToString()
        {
            return this.Name;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is MembershipConnect))
            {
                return false;
            }
            if (obj != null)
            {
                MembershipConnect membershipUser = (MembershipConnect)obj;
                if (this.Membership == membershipUser.Membership && membershipUser.Name.EqualsOrNullEmpty(this.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            //The base.Equals method was overrided by PathResource
            return this.GetHashCode() == obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    #endregion

    #region Interfaces
    public partial class MembershipConnect : IMemberElement, IPersistable, IIdentifiable, IEntity
    {
        #region IPersistable
        private bool _isDummy = true;
        bool IPersistable.IsDummy
        {
            get { return this._isDummy; }
        }

        void IPersistable.Init(IPersistable source)
        {
            var membershipConnect = (MembershipConnect)source;
            this.Name = membershipConnect.Name;
            this._isDummy = false;
        }

        void IPersistable.OnSaved()
        {

        }

        void IPersistable.OnSaving()
        {
            this._isDummy = false;
        }
        #endregion

        #region IIdentifiable
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

        #endregion

        #region IMemberElement
        public virtual Membership Membership
        {
            get;
            set;
        }
        #endregion

        #region IEntity
        public int Id
        {
            get;
            set;
        } 
        #endregion
    }
    #endregion

    #region Persistence
    [DataContract]
    public partial class MembershipConnect
    {
        [DataMember]
        public virtual string Name { get; set; }

        private string _displayName;
        [DataMember]
        public virtual string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(_displayName))
                {
                    return this.Name;
                }
                return _displayName;
            }
            set
            {
                this._displayName = value;
            }
        }

        [DataMember]
        public virtual string AppId { get; set; }
        [DataMember]
        public virtual string AppSecret { get; set; }
        [DataMember]
        public virtual Dictionary<string, string> Options { get; set; }
        [DataMember]
        public virtual string[] MembershipGroups { get; set; }
        [DataMember]
        public virtual bool Enabled { get; set; }

        #region O/R mapping fields        
        public string OptionsXml
        {
            get
            {
                if (this.Options != null)
                {
                    return DataContractSerializationHelper.SerializeAsXml(this.Options);
                }
                else
                {
                    return "";
                }
            }

            protected set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.Options = DataContractSerializationHelper.DeserializeFromXml<Dictionary<string, string>>(value);
                }
            }
        }

        public string MembershipGroupsXml
        {
            get
            {
                if (this.MembershipGroups != null)
                {
                    return DataContractSerializationHelper.SerializeAsXml(this.MembershipGroups);
                }
                else
                {
                    return "";
                }
            }

            protected set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.MembershipGroups = DataContractSerializationHelper.DeserializeFromXml<string[]>(value);
                }
            }
        }
        #endregion
    }
    #endregion
}
