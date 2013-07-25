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
using System.Security.Policy;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Collections;

namespace Kooboo.CMS.Account.Models
{
    [DataContract]
    public partial class User
    {
        public string UserName { get; set; }

        [DataMember()]
        public string Email { get; set; }

        [DataMember()]
        public string Password { get; set; }

        [DataMember()]
        public bool IsAdministrator { get; set; }

        [DataMember()]
        public string UICulture { get; set; }

        [DataMember()]
        public string PasswordSalt
        {
            get;
            set;
        }

        private bool? isApproved;
        [DataMember()]
        public bool IsApproved
        {
            get
            {
                if (!isApproved.HasValue)
                {
                    isApproved = true;
                }
                return isApproved.Value;
            }
            set
            {
                isApproved = value;
            }
        }

        [DataMember()]
        public bool IsLockedOut
        {
            get;
            set;
        }

        [DataMember()]
        public int FailedPasswordAttemptCount
        {
            get;
            set;
        }

        [DataMember()]
        public DateTime? UtcLastLockoutDate
        {
            get;
            set;
        }

        [DataMember()]
        public DateTime? UtcLastLoginDate
        {
            get;
            set;
        }
        [DataMember]
        public virtual DateTime? UtcLastPasswordChangedDate { get; set; }
        [DataMember]
        public virtual string ActivateCode { get; set; }

        private DynamicDictionary customFields = null;
        [DataMember(Order = 7)]
        public DynamicDictionary CustomFields
        {
            get
            {
                if (customFields == null)
                {
                    customFields = new DynamicDictionary();
                }
                return customFields;
            }
            set
            {
                customFields = value;
            }
        }

        [XmlIgnore]
        public string CustomFieldsXml
        {
            get
            {
                string xml = "";
                if (CustomFields != null)
                {
                    xml = Kooboo.Runtime.Serialization.DataContractSerializationHelper.SerializeAsXml(this.CustomFields);
                }
                return xml;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    CustomFields = Kooboo.Runtime.Serialization.DataContractSerializationHelper.DeserializeFromXml<DynamicDictionary>(value);
                }
            }
        }


    }

    public partial class User : IPersistable, IIdentifiable
    {
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

        #region IPersistable

        private bool isDummy = true;
        public bool IsDummy
        {
            get { return isDummy; }
            set { isDummy = value; }
        }

        public void Init(IPersistable source)
        {
            this.UserName = ((User)source).UserName;
            isDummy = false;
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
