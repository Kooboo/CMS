#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models.Orders;
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
    public interface IOrderService
    {
        void Add(Order order);
        void Update(Order order);
        void Delete(Order order);
    }
}
