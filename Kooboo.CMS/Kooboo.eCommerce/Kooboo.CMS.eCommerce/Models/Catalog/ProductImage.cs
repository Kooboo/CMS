#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Persistence.Catalog;
using System;

namespace Kooboo.CMS.eCommerce.Models.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProductImage : IRelationEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public virtual int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set whenever the productimage has been delete
        /// </summary>
        public virtual bool Deleted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the image.
        /// 图片的类型，比如：
        /// Thumbnail
        /// Detail
        /// List
        /// Cart
        /// 还有可能一些自定义的类型名称。
        /// </summary>
        /// <value>
        /// The type of the image.
        /// </value>
        public virtual string ImageType { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
        public virtual string ImageUrl { get; set; }      

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual Product Product { get; set; }

    }

    public partial class ProductImage
    {
        public virtual int? ProductId
        {
            get
            {
                return (this.Product == null) ? default(int) : new Nullable<int>(this.Product.Id);
            }
            set
            {
                this.Product = value == null ? null : Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IProductProvider>().QueryById(value.Value);
            }
        }

        public virtual EntityFile ImageFile { get; set; }
    }
}
