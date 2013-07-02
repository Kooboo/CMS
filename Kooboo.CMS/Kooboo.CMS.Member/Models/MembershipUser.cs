#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kooboo.CMS.Member.Models
{

    #region override object
    public partial class MembershipUser
    {
        public static bool operator ==(MembershipUser obj1, MembershipUser obj2)
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
        public static bool operator !=(MembershipUser obj1, MembershipUser obj2)
        {
            return !(obj1 == obj2);
        }
        public override string ToString()
        {
            return this.UserName;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is MembershipUser))
            {
                return false;
            }
            if (obj != null)
            {
                MembershipUser membershipUser = (MembershipUser)obj;
                if (this.Membership == membershipUser.Membership && membershipUser.UserName.EqualsOrNullEmpty(this.UserName, StringComparison.CurrentCultureIgnoreCase))
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
    public partial class MembershipUser : IMemberElement, IPersistable, IIdentifiable
    {
        #region IPersistable
        private bool _isDummy = true;
        bool IPersistable.IsDummy
        {
            get { return this._isDummy; }
        }

        void IPersistable.Init(IPersistable source)
        {
            var membershipConnect = (MembershipUser)source;
            this.UserName = membershipConnect.UserName;
            this._isDummy = false;
        }

        void IPersistable.OnSaved()
        {
            this._isDummy = false;
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
                return this.UserName;
            }
            set
            {
                this.UserName = value;
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
    }
    #endregion

    #region Persistence
    [DataContract]
    public partial class MembershipUser
    {
        [DataMember]
        public virtual string UserName { get; set; }
        [DataMember]
        public virtual string Email { get; set; }
        [DataMember]
        public virtual string Password { get; set; }
        [DataMember]
        public virtual string PasswordSalt { get; set; }
        [DataMember]
        public virtual DateTime UtcCreationDate { get; set; }
        [DataMember]
        public virtual bool IsApproved { get; set; }
        [DataMember]
        public virtual bool IsLockedOut { get; set; }
        [DataMember]
        public virtual DateTime UtcLastLockoutDate { get; set; }
        [DataMember]
        public virtual DateTime UtcLastLoginDate { get; set; }
        [DataMember]
        public virtual DateTime UtcLastPasswordChangedDate { get; set; }
        [DataMember]
        public virtual string PasswordQuestion { get; set; }
        [DataMember]
        public virtual string PasswordAnswer { get; set; }
        [DataMember]
        public virtual string Culture { get; set; }
        [DataMember]
        public virtual string TimeZoneId { get; set; }
        [DataMember]
        public virtual string Comment { get; set; }
        [DataMember]
        public virtual Dictionary<string, string> Profiles { get; set; }

        [DataMember]
        public virtual string[] MembershipGroups { get; set; }
    }
    #endregion
}
