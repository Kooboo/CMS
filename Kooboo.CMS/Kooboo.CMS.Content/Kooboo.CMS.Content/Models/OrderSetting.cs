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

namespace Kooboo.CMS.Content.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class OrderSetting
    {
        public static readonly OrderSetting Default = new OrderSetting() { FieldName = "UtcCreationDate", Direction = OrderDirection.Descending };
        [DataMember(Order = 1)]
        public string FieldName { get; set; }
        [DataMember(Order = 2)]
        public OrderDirection Direction { get; set; }
    }
}
