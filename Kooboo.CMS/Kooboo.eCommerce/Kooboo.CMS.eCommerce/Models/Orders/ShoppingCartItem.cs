#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Models.Orders
{
    public class ShoppingCartItem
    {
        public virtual ShoppingCart ShoppingCart { get; set; }

        public virtual Product Product { get; set; }

        public virtual string ProductVariantUUID { get; set; }

        public virtual int Quantity { get; set; }

        public virtual DateTime UtcCreationDate { get; set; }

        public virtual DateTime UtcUpdateDate { get; set; }
    }
}
