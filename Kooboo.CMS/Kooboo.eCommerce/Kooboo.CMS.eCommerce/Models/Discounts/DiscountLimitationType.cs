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

namespace Kooboo.CMS.eCommerce.Models.Discounts
{
    /// <summary>
    /// Represents a discount limitation type
    /// </summary>
    public enum DiscountLimitationType
    {
        /// <summary>
        /// None
        /// </summary>
        Unlimited = 0,
        /// <summary>
        /// N Times Only
        /// </summary>
        NTimesOnly = 15,
        /// <summary>
        /// N Times Per Customer
        /// </summary>
        NTimesPerCustomer = 25,
    }
}
