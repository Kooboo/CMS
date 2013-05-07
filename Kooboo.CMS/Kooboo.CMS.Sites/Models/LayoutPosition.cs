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

namespace Kooboo.CMS.Sites.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class LayoutPosition
    {
        [DataMember(Order = 1)]
        public string ID { get; set; }
    }
}
