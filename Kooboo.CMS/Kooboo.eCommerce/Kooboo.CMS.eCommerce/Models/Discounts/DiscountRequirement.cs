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

namespace Kooboo.CMS.eCommerce.Models.Discounts
{
    /// <summary>
    /// Represents a discount requirement
    /// </summary>
    public partial class DiscountRequirement : IRelationEntity
    {
        /// <summary>
        /// Gets or sets the id.
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
        /// Gets or sets the the discount requirement spent amount
        /// </summary>
        public virtual decimal SpentAmount { get; set; }
        
        /// <summary>
        /// Gets or sets the restricted product variant identifiers。以逗号分隔
        /// </summary>
        public virtual string RestrictedProductVariantUUIDs { get; set; }

        /// <summary>
        /// Gets or sets the discount
        /// </summary>
        public virtual Discount Discount { get; set; }      
    }
}
