using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kooboo.Connect
{
    [DataContract]
    public class Profile
    {
        [DataMember(Order = 1)]
        public string FirstName
        {
            get;
            set;
        }
        [DataMember(Order = 2)]
        public string MiddleName
        {
            get;
            set;
        }
        [DataMember(Order = 3)]
        public string LastName
        {
            get;
            set;
        }
        [DataMember(Order = 4)]
        public string Country
        {
            get;
            set;
        }
        [DataMember(Order = 5)]
        public string City
        {
            get;
            set;
        }
        [DataMember(Order = 6)]
        public string Address
        {
            get;
            set;
        }
        [DataMember(Order = 7)]
        public string Postcode
        {
            get;
            set;
        }
        [DataMember(Order = 8)]
        public short Gender
        {
            get;
            set;
        }
        [DataMember(Order = 9)]
        public DateTime? Birthday
        {
            get;
            set;
        }
        [DataMember(Order = 10)]
        public string Telphone
        {
            get;
            set;
        }
        [DataMember(Order = 11)]
        public string Mobile
        {
            get;
            set;
        }
    }
}
