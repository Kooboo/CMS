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

namespace Kooboo.CMS.eCommerce.Persistence.Customers
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomerProvider : IProvider<Customer>
    {
        /// <summary>
        /// Queries the name of the by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        Customer QueryByUserName(string userName);
        /// <summary>
        /// Queries the by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Customer QueryByEmail(string email);
    }
}
