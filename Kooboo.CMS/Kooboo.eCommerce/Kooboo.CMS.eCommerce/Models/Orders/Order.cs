#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Relational;
using Kooboo.CMS.eCommerce.Models.Customers;
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
    public partial class Order : IRelationEntity, ISiteObject
    {
        #region Properties
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
        /// Get or set CustomerId
        /// </summary>
        public virtual int CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the order status.
        /// </summary>
        /// <value>
        /// The order status.
        /// </value>
        public virtual OrderStatus OrderStatus
        {
            get;
            set;
        }

        #region Subtotal && Tax


        /// <summary>
        /// Gets or sets the  subtotal (incl tax)
        /// 订单产品总的含税价格
        /// </summary>
        public virtual decimal SubtotalInclTax { get; set; }

        /// <summary>
        /// Gets or sets the subtotal (excl tax)
        /// 订单产品的不含税价格
        /// </summary>
        public virtual decimal SubtotalExclTax { get; set; }

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

        /// <summary>
        /// Gets or sets the shipping (incl tax)
        /// 含税物流金额
        /// </summary>
        public virtual decimal ShippingCostInclTax { get; set; }

        /// <summary>
        /// Gets or sets the shipping (excl tax)
        /// 不含税的物流金额
        /// </summary>
        public virtual decimal ShippingCostExclTax { get; set; }

        /// <summary>
        /// Gets or sets the payment method additional fee (incl tax)
        /// 
        /// </summary>
        public virtual decimal PaymentMethodAdditionalFeeInclTax { get; set; }

        /// <summary>
        /// Gets or sets the payment method additional fee (excl tax)
        /// </summary>
        public virtual decimal PaymentMethodAdditionalFeeExclTax { get; set; }


        /// <summary>
        /// Gets or sets the order tax
        /// </summary>
        public virtual decimal OrderTax { get; set; }

        /// <summary>
        /// Gets or sets the order discount (applied to order total)
        /// </summary>
        public virtual decimal OrderDiscount { get; set; }

        /// <summary>
        /// Gets or sets the order total
        /// </summary>
        public virtual decimal OrderTotal { get; set; }
        #endregion

        #region Address

        private OrderAddress _shippingAddress;
        /// <summary>
        /// Gets or sets the shipping address.
        /// </summary>
        /// <value>
        /// The shipping address.
        /// </value>
        public virtual OrderAddress ShippingAddress
        {
            get
            {
                return _shippingAddress ?? (_shippingAddress = new OrderAddress());
            }
            set
            {
                _shippingAddress = value;
            }
        }

        private OrderAddress _billingAddress;
        /// <summary>
        /// Gets or sets the billing address.
        /// </summary>
        /// <value>
        /// The billing address.
        /// </value>
        public virtual OrderAddress BillingAddress
        {
            get
            {
                return _billingAddress ?? (_billingAddress = new OrderAddress());
            }
            set
            {
                _billingAddress = value;
            }
        }
        #endregion

        #region Payment
        private Payment _payment;
        /// <summary>
        /// Gets or sets the payment.
        /// </summary>
        /// <value>
        /// The payment.
        /// </value>
        public virtual Payment Payment
        {
            get
            {
                return _payment ?? (_payment = new Payment());
            }
            set
            {
                _payment = value;
            }
        }
        #endregion

        #region Shipment
        private Shipment _shipment;
        /// <summary>
        /// Gets or sets the shipment.
        /// </summary>
        /// <value>
        /// The shipment.
        /// </value>
        public virtual Shipment Shipment
        {
            get
            {
                return _shipment ?? (_shipment = new Shipment());
            }
            set
            {
                _shipment = value;
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the note.
        /// The order note
        /// </summary>
        /// <value>
        /// The note.
        /// </value>
        public virtual string Note { get; set; }

        /// <summary>
        /// Gets or sets the UTC creation date.
        /// </summary>
        /// <value>
        /// The UTC creation date.
        /// </value>
        public virtual DateTime UtcCreationDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public virtual string Site { get; set; }


        #region Relations
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        public virtual Customer Customer
        {
            get;
            set;
        }

        private ICollection<OrderItem> _orderItems;
        /// <summary>
        /// Gets or sets the order items.
        /// </summary>
        public virtual ICollection<OrderItem> OrderItems
        {
            get
            {
                return _orderItems ?? (_orderItems = new List<OrderItem>());
            }
            set
            {
                _orderItems = value;
            }
        }
        #endregion

        #endregion        
    }
}
