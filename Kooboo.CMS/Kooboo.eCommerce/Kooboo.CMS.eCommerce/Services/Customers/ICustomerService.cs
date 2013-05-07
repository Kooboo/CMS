#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models.Customers;
using Kooboo.CMS.eCommerce.Persistence.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Services.Customers
{
    public interface ICustomerService
    {
        ICustomerProvider Provider { get; }
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);
    }
}
