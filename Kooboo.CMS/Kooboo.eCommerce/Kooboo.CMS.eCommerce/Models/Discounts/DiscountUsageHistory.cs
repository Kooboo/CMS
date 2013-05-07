#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models.Orders;
using System;

namespace Kooboo.CMS.eCommerce.Models.Discounts
{
    /// <summary>
    /// Represents a discount usage history entry
    /// </summary>
    public partial class DiscountUsageHistory : IRelationEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual int Id
        {
            get;
            set;
        }

        /// <summary>
        /// is Deleted?
        /// </summary>
        public virtual bool Deleted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the discount identifier
        /// </summary>
        public virtual int DiscountId { get; set; }

        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public virtual int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the UTC creation date.
        /// </summary>
        public virtual DateTime UtcCreationDate { get; set; }


        /// <summary>
        /// Gets or sets the discount
        /// </summary>
        public virtual Discount Discount { get; set; }

        /// <summary>
        /// Gets or sets the order
        /// </summary>
        public virtual Order Order { get; set; }


    }
}
