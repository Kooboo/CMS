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

namespace Kooboo.Connect
{
    [DataContract]
    public class Membership
    {
        public Membership()
        {
            this.IsApproved = true;
            this.IsLockedOut = false;
            this.CreateDate = DateTime.Now;
        }
        [DataMember(Order = 1)]
        public string Password
        {
            get;
            set;
        }
        [DataMember(Order = 2)]
        public string PasswordSalt
        {
            get;
            set;
        }
        [DataMember(Order = 3)]
        public string PasswordQuestion
        {
            get;
            set;
        }
        [DataMember(Order = 4)]
        public string PasswordAnswer
        {
            get;
            set;
        }
        [DataMember(Order = 5)]
        public bool IsApproved
        {
            get;
            set;
        }
        [DataMember(Order = 6)]
        public bool IsLockedOut
        {
            get;
            set;
        }
        [DataMember(Order = 7)]
        public DateTime CreateDate
        {
            get;
            set;
        }
        [DataMember(Order = 8)]
        public DateTime? LastLoginDate
        {
            get;
            set;
        }
        [DataMember(Order = 9)]
        public DateTime? LastPasswordChangedDate
        {
            get;
            set;
        }
        [DataMember(Order = 10)]
        public DateTime? LastLockoutDate
        {
            get;
            set;
        }
        [DataMember(Order = 11)]
        public int FailedPasswordAttemptCount
        {
            get;
            set;
        }
        [DataMember(Order = 12)]
        public DateTime? FailedPasswordAttemptWindowStart
        {
            get;
            set;
        }
        [DataMember(Order = 13)]
        public int FailedPasswordAnswerAttemptCount
        {
            get;
            set;
        }
        [DataMember(Order = 14)]
        public DateTime? FailedPasswordAnswerAttemptWindowStart
        {
            get;
            set;
        }
        [DataMember(Order = 15)]
        public string Comment
        {
            get;
            set;
        }

    }
}
