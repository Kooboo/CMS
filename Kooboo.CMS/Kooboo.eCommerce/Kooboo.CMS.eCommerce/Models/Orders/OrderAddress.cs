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
    public partial class OrderAddress : IValueObject
    {
        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Gets or sets the company
        /// </summary>
        public virtual string Company { get; set; }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        public virtual string City { get; set; }

        /// <summary>
        /// Gets or sets the address 1
        /// </summary>
        public virtual string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the address 2
        /// </summary>
        public virtual string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the zip/postal code
        /// </summary>
        public virtual string ZipPostalCode { get; set; }

        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the fax number
        /// </summary>
        public virtual string FaxNumber { get; set; }

    }
}
