#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.EventBus;
using Kooboo.CMS.eCommerce.Models.Customers;
using Kooboo.CMS.eCommerce.Persistence.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Services.Customers
{
    public class CustomerService : RelationalServiceBase<Customer, ICustomerProvider>, ICustomerService
    {
        #region Properties
        public ICustomerProvider CustomerProvider { get; private set; }
        public IEventPublisher EventPublisher { get; set; }
        #endregion
        public CustomerService(ICustomerProvider customerProvider, IEventPublisher eventPublisher)
            : base(customerProvider, eventPublisher)
        {
            this.CustomerProvider = customerProvider;
            this.EventPublisher = eventPublisher;
        }
    }
}
