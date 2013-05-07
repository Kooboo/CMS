#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.EventBus;
using Kooboo.CMS.eCommerce.Models.Orders;
using Kooboo.CMS.eCommerce.Persistence.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Services.Orders
{
    /// <summary>
    /// 
    /// </summary>
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IOrderService))]
    public class OrderService : RelationalServiceBase<Order, IOrderProvider>, IOrderService
    {
        #region .ctor
        public OrderService(IOrderProvider orderProvider, IEventPublisher eventPublisher)
            : base(orderProvider, eventPublisher)
        {
        }
        #endregion

        #region Methods
   
        #endregion
    }
}
