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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Models
{
    [DataContract]
    public class HtmlMeta
    {
        [DataMember(Order = 1)]
        public string Author { get; set; }
        [DataMember(Order = 3)]
        public string Keywords { get; set; }
        [DataMember(Order = 5)]
        public string Description { get; set; }

        [DataMember(Order = 7)]
        public Dictionary<string, string> Customs
        {
            get;
            set;
        }
        [DataMember(Order = 8)]
        public string HtmlTitle { get; set; }

        [DataMember(Order = 10)]
        public string Canonical { get; set; }
    }
}
