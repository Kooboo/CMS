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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kooboo.CMS.Member.Models
{
    #region override object
    public partial class Membership
    {
        public static bool operator ==(Membership obj1, Membership obj2)
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
        public static bool operator !=(Membership obj1, Membership obj2)
        {
            return !(obj1 == obj2);
        }
        public override string ToString()
        {
            return this.Name;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is MembershipUser))
            {
                return false;
            }
            if (obj != null)
            {
                Membership membership = (Membership)obj;
                if (membership.Name.EqualsOrNullEmpty(this.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return this.GetHashCode() == obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    #endregion

    #region Interfaces
    public partial class Membership : IPersistable, IIdentifiable, IEntity
    {
        #region IPersistable
        private bool _isDummy = true;
        bool IPersistable.IsDummy
        {
            get { return this._isDummy; }
        }

        void IPersistable.Init(IPersistable source)
        {
            var membership = (Membership)source;
            this.Name = membership.Name;
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
    public partial class Membership
    {
        public Membership()
        { }
        public Membership(string name)
        {
            this.Name = name;
        }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string AuthCookieName { get; set; }
        [DataMember]
        public string AuthCookieDomain { get; set; }
        [DataMember]
        public string HashAlgorithmType { get; set; }
        [DataMember]
        public int MaxInvalidPasswordAttempts { get; set; }
        [DataMember]
        public int MinRequiredPasswordLength { get; set; }
        [DataMember]
        public string PasswordStrengthRegularExpression { get; set; }

    }
    #endregion
}
