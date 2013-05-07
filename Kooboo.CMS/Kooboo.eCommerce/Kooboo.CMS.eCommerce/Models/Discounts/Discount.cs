#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Relational;
using Kooboo.CMS.eCommerce.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Models.Discounts
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Discount : IRelationEntity, ISiteObject
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
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
        /// Gets or sets the name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the discount type
        /// </summary>
        public virtual DiscountType DiscountType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use percentage
        /// </summary>
        public virtual bool UsePercentage { get; set; }

        /// <summary>
        /// Gets or sets the discount percentage
        /// </summary>
        public virtual decimal DiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets the discount amount
        /// </summary>
        public virtual decimal DiscountAmount { get; set; }

        /// <summary>
        /// Gets or sets the discount start date and time
        /// </summary>
        public virtual DateTime? StartDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the discount end date and time
        /// </summary>
        public virtual DateTime? EndDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether discount requires coupon code
        /// </summary>
        public virtual bool RequiresCouponCode { get; set; }

        /// <summary>
        /// Gets or sets the coupon code
        /// </summary>
        public virtual string CouponCode { get; set; }

        /// <summary>
        /// Gets or sets the discount limitation
        /// </summary>
        public virtual DiscountLimitationType DiscountLimitationType { get; set; }

        /// <summary>
        /// Gets or sets the discount limitation times (used when Limitation is set to "N Times Only" or "N Times Per Customer")
        /// </summary>
        public virtual int LimitationTimes { get; set; }

        /// <summary>
        /// Gets or sets the applied to product variants.以逗号分隔
        /// </summary>
        public virtual string AppliedToProductVariantUUIDs { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public virtual string Site { get; set; }

        #region Relation
        private ICollection<DiscountRequirement> _discountRequirements;
        /// <summary>
        /// Gets or sets the discount requirement
        /// </summary>
        public virtual ICollection<DiscountRequirement> DiscountRequirements
        {
            get { return _discountRequirements ?? (_discountRequirements = new List<DiscountRequirement>()); }
            protected set { _discountRequirements = value; }
        }

        private ICollection<Category> _appliedToCategories;
        /// <summary>
        /// Gets or sets the categories
        /// </summary>
        public virtual ICollection<Category> AppliedToCategories
        {
            get { return _appliedToCategories ?? (_appliedToCategories = new List<Category>()); }
            protected set { _appliedToCategories = value; }
        }    
        #endregion
    }
}
