#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.Collections.Generic;

namespace Kooboo.CMS.eCommerce.Models.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProductType : INonRelationEntity
    {
        #region .ctor
        public ProductType()
        {
        }
        public ProductType(string name)
        {
            this.Name = name;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the UUID.
        /// 本地生成的UUID作为主键
        /// </summary>
        /// <value>
        /// The UUID.
        /// </value>
        public string UUID
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        #endregion

        #region Relation
        private ICollection<ProductFieldType> _customFields;
        /// <summary>
        /// Gets or sets the custom fields.
        /// </summary>
        /// <value>
        /// The custom fields.
        /// </value>
        public ICollection<ProductFieldType> CustomFields
        {
            get
            {
                return _customFields ?? (_customFields = new List<ProductFieldType>());
            }
            set
            {
                this._customFields = value;
            }
        }
        private ICollection<ProductFieldType> _variants;
        /// <summary>
        /// Gets or sets the variants.
        /// </summary>
        /// <value>
        /// The variants.
        /// </value>
        public ICollection<ProductFieldType> Variants
        {
            get
            {
                return _variants ?? (_variants = new List<ProductFieldType>());
            }
            set
            {
                this._variants = value;
            }
        }
        #endregion


    }
}
