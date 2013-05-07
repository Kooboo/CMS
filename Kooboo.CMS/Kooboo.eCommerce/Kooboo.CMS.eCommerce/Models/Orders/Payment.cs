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
    public partial class Payment : IValueObject
    {
        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        public virtual string PaymentMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the payment status.
        /// </summary>
        public virtual PaymentStatus PaymentStatus { get; set; }
    }
}
