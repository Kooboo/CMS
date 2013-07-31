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

namespace Kooboo.CMS.Membership.Models
{
    #region override object
    public partial class MembershipGroup
    {
        public static bool operator ==(MembershipGroup obj1, MembershipGroup obj2)
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
        public static bool operator !=(MembershipGroup obj1, MembershipGroup obj2)
        {
            return !(obj1 == obj2);
        }
        public override string ToString()
        {
            return this.Name;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is MembershipGroup))
            {
                return false;
            }
            if (obj != null)
            {
                MembershipGroup membershipGroup = (MembershipGroup)obj;
                if (this.Membership == membershipGroup.Membership && membershipGroup.Name.EqualsOrNullEmpty(this.Name, StringComparison.CurrentCultureIgnoreCase))
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
    public partial class MembershipGroup : IMemberElement, IPersistable, IIdentifiable, IEntity
    {
        #region IPersistable
        private bool _isDummy = true;
        bool IPersistable.IsDummy
        {
            get { return this._isDummy; }
        }

        void IPersistable.Init(IPersistable source)
        {
            var membershipConnect = (MembershipGroup)source;
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
    public partial class MembershipGroup
    {
        [DataMember]
        public virtual string Name { get; set; }
    }
    #endregion
}
