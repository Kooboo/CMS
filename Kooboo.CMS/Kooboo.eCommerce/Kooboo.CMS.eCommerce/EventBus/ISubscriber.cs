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

namespace Kooboo.CMS.eCommerce.EventBus
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISubscriber<T>
    {
        /// <summary>
        /// Handles the specified event message.
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        void Handle(T eventMessage);
    }
}
