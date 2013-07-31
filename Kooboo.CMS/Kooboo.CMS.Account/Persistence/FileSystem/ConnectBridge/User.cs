﻿#region License
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
using System.Security.Cryptography;
using System.Runtime.Serialization;

namespace Kooboo.Connect
{
    [DataContract]
    public class User
    {
        public User()
        {         
            this.Membership = new Membership();
        }
    
        [DataMember(Order = 2)]
        public string Name
        {
            get;
            set;
        }
       
        [DataMember(Order = 4)]
        public Membership Membership
        {
            get;
            set;
        }
        [DataMember(Order =5)]
        public string Email
        {
            get;
            set;
        }        

        public bool ValidatePassword(string encodedPassword)
        {
            return this.Membership.Password == encodedPassword;
        }


    }
}
