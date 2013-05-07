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

namespace Kooboo.CMS.eCommerce.Models.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RelatedProduct : IRelationEntity
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public virtual int Id
        {
            get;
            set;
        }
        /// <summary>
        /// is Deleted?
        /// </summary>
        public virtual bool Deleted
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public virtual int DisplayOrder { get; set; }
        #endregion

        #region Relation
        /// <summary>
        /// Gets or sets the product1.
        /// </summary>
        /// <value>
        /// The product1.
        /// </value>
        public virtual Product Product1 { get; set; }

        /// <summary>
        /// Gets or sets the product2.
        /// </summary>
        /// <value>
        /// The product2.
        /// </value>
        public virtual Product Product2 { get; set; }
        #endregion
    }
}
