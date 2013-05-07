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
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Models.Orders
{
    /// <summary>
    /// 
    /// </summary>
    public enum ShippingStatus : int
    {
        /// <summary>
        /// Shipping not required
        /// </summary>
        ShippingNotRequired = 0,
        /// <summary>
        /// Not yet shipped
        /// </summary>
        NotYetShipped = 10,
        /// <summary>
        /// Partially shipped
        /// </summary>
        PartiallyShipped = 20,
        /// <summary>
        /// Shipped
        /// </summary>
        Shipped = 30,
        /// <summary>
        /// Delivered
        /// </summary>
        Delivered = 40,
    }
}
