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

namespace Kooboo.CMS.eCommerce.EventBus
{
    public class PreUpdate<T> : IEventMessage
    {
        public PreUpdate(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
