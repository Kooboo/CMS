#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

namespace Kooboo.CMS.eCommerce.Models.Orders
{
    /// <summary>
    /// 
    /// </summary>
    public enum PaymentStatus : int
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 0,
        /// <summary>
        /// Authorized
        /// </summary>
        Authorized = 10,
        /// <summary>
        /// Paid
        /// </summary>
        Paid = 20,
        /// <summary>
        /// Partially Refunded
        /// </summary>
        PartiallyRefunded = 30,
        /// <summary>
        /// Refunded
        /// </summary>
        Refunded = 40,
        /// <summary>
        /// Voided
        /// </summary>
        Voided = 50,
    }
}
