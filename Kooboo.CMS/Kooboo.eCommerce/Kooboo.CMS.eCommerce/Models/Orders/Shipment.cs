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
    public partial class Shipment : IValueObject
    {
        /// <summary>
        /// Gets or sets the shipping status.
        /// </summary>
        public virtual ShippingStatus ShippingStatus { get; set; }

        /// <summary>
        /// Gets or sets the tracking number of this shipment
        /// </summary>
        public virtual string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or sets the total weight of this shipment       
        /// </summary>
        public virtual decimal? TotalWeight { get; set; }

        /// <summary>
        /// Gets or sets the shipped date and time
        /// </summary>        
        public virtual DateTime? UtcShippedDate { get; set; }

        /// <summary>
        /// Gets or sets the delivery date and time
        /// </summary>
        public virtual DateTime? UtcDeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>   
        public virtual DateTime UtcCreationDate { get; set; }
    }
}
