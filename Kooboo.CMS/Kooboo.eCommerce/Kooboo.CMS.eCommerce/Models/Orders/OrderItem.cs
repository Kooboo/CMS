#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models.Catalog;
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
    public partial class OrderItem : IRelationEntity
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
        /// Gets or sets the order id.
        /// </summary>
        /// <value>
        /// The order id.
        /// </value>
        public virtual int OrderId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the product id.
        /// </summary>
        /// <value>
        /// The product id.
        /// </value>
        public virtual int ProductId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the product variant UUID.
        /// </summary>
        /// <value>
        /// The product variant UUID.
        /// </value>
        public virtual string ProductVariantUUID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public virtual int Quantity
        {
            get;
            set;
        }

        #region Price && Subtotal
        /// <summary>
        /// Gets or sets the unit price in primary store currency (incl tax)
        /// </summary>
        public virtual decimal UnitPriceInclTax { get; set; }

        /// <summary>
        /// Gets or sets the unit price in primary store currency (excl tax)
        /// </summary>
        public virtual decimal UnitPriceExclTax { get; set; }

        /// <summary>
        /// Gets or sets the subtotal incl tax.
        /// </summary>
        /// <value>
        /// The subtotal incl tax.
        /// </value>
        public virtual decimal SubtotalInclTax
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the subtoal excl tax.
        /// </summary>
        /// <value>
        /// The subtoal excl tax.
        /// </value>
        public virtual decimal SubtoalExclTax { get; set; }

        /// <summary>
        /// Gets or sets the subtotal discount (incl tax)
        /// 折扣金额（含税）
        /// </summary>
        public virtual decimal DiscountInclTax { get; set; }

        /// <summary>
        /// Gets or sets the subtotal discount (excl tax)
        /// 折扣金额（不含税）
        /// </summary>
        public virtual decimal DiscountExclTax { get; set; }
        #endregion

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public virtual string ProductName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the SKU.
        /// </summary>
        /// <value>
        /// The SKU.
        /// </value>
        public virtual string SKU
        {
            get;
            set;
        }

        private IDictionary<string, object> _productVariations;
        /// <summary>
        /// Gets or sets the product variations.
        /// </summary>
        /// <value>
        /// The product variations.
        /// </value>
        public virtual IDictionary<string, object> ProductVariations
        {
            get
            {
                return _productVariations ?? (_productVariations = new Dictionary<string, object>());
            }
            set
            {
                _productVariations = value;
            }
        }


        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public virtual Order Order { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        public virtual Product Product { get; set; }
    }
}
