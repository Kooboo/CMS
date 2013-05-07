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
    public enum OrderStatus : int
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 0,
        /// <summary>
        /// Processing
        /// </summary>
        Processing = 10,
        /// <summary>
        /// Complete
        /// </summary>
        Complete = 20,
        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 30
    }
}
