#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models.Customers;
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
    public class ShoppingCart : IRelationEntity, ISiteObject
    {
        #region Properties
        public virtual int Id { get; set; }

        public virtual string Cookie { get; set; }

        public virtual DateTime UtcCreationDate { get; set; }

        public virtual DateTime UtcUpdateDate { get; set; }

        public virtual string Site { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual bool Deleted
        {
            get;
            set;
        }

        #region Relations
        private ICollection<ShoppingCartItem> _shoppingCartItems;
        public ICollection<ShoppingCartItem> ShoppingCartItems
        {
            get
            {
                return _shoppingCartItems ?? (_shoppingCartItems = new List<ShoppingCartItem>());
            }
            set
            {
                _shoppingCartItems = value;
            }
        }
        #endregion

        #endregion



    }
}
