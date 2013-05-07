#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Models.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProductVariant : DynamicDictionary, INonRelationEntity
    {
        /// <summary>
        /// Gets or sets the UUID.
        /// 本地生成的UUID作为主键
        /// </summary>
        /// <value>
        /// The UUID.
        /// </value>
        public virtual string UUID
        {
            get
            {
                return Get<string>("UUID");
            }
            set
            {
                this["UUID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the product id.
        /// </summary>
        /// <value>
        /// The product id.
        /// </value>
        public virtual int ProductId
        {
            get
            {
                return Get<int>("ProductId");
            }
            set
            {
                this["ProductId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the SKU.
        /// </summary>
        /// <value>
        /// The SKU.
        /// </value>
        public virtual string SKU
        {
            get
            {
                return Get<string>("SKU");
            }
            set
            {
                this["SKU"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the purchase price.
        /// </summary>
        /// <value>
        /// The purchase price.
        /// </value>
        public virtual decimal PurchasePrice
        {
            get
            {
                return Get<decimal>("PurchasePrice");
            }
            set
            {
                this["PurchasePrice"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the retail price.
        /// </summary>
        /// <value>
        /// The retail price.
        /// </value>
        public virtual decimal RetailPrice
        {
            get
            {
                return Get<decimal>("RetailPrice");
            }
            set
            {
                this["RetailPrice"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the stock.
        /// </summary>
        /// <value>
        /// The stock.
        /// </value>
        public virtual int Stock
        {
            get
            {
                return Get<int>("Stock");
            }
            set
            {
                this["Stock"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the delivery days.
        /// </summary>
        /// <value>
        /// The delivery days.
        /// </value>
        public virtual int DeliveryDays
        {
            get
            {
                return Get<int>("DeliveryDays");
            }
            set
            {
                this["DeliveryDays"] = value;
            }
        }

    }
}
