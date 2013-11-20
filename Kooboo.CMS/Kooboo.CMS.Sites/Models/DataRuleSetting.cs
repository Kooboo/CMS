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
using System.Collections;
using Kooboo.CMS.Sites.DataRule;

namespace Kooboo.CMS.Sites.Models
{
    public enum TakeOperation
    {
        List,
        First,
        Count
    }
    [DataContract]
    public class DataRuleSetting
    {
        [DataMember(Order = 1)]
        public string DataName { get; set; }
        [DataMember(Order = 3)]
        public IDataRule DataRule { get; set; }
        [DataMember(Order = 5)]
        public TakeOperation TakeOperation { get; set; }

        /// <summary>
        /// The time, in seconds, that the data rule is cached. 
        /// </summary>
        [DataMember(Order = 7)]
        public int CachingDuration { get; set; }

    }
}
