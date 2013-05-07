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

namespace Kooboo.CMS.eCommerce.Models.Customers
{
    #region Customer Membership Members
    /// <summary>
    /// 
    /// </summary>
    public partial class Customer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Customer" /> class.
        /// </summary>
        public Customer()
        {
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public virtual string Username { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public virtual string Email { get; set; }

        /// <summary>
        /// Gets or sets the membership.
        /// </summary>
        public virtual Membership Membership { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the time zone identifier
        /// </summary>
        public virtual string TimeZoneId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer has been deleted
        /// </summary>
        public virtual bool Deleted { get; set; }

    }
    #endregion
}
