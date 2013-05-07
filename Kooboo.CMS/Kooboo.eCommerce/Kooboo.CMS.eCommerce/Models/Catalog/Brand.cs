#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Relational;
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
    public partial class Brand : IRelationEntity, ISiteObject
    {
        #region .ctor
        /// <summary>
        /// 
        /// </summary>
        public Brand()
        {
            this.UtcCreationDate = DateTime.UtcNow;
            this.UtcUpdateDate = DateTime.UtcNow;
        }
        #endregion
        #region Properties
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
        /// Gets or sets the name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public virtual string Image
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the page size
        /// </summary>
        public virtual int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public virtual bool Published { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public virtual bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the UTC creation date.
        /// </summary>
        /// <value>
        /// The UTC creation date.
        /// </value>
        public virtual DateTime UtcCreationDate { get; set; }

        /// <summary>
        /// Gets or sets the UTC update date.
        /// </summary>
        /// <value>
        /// The UTC update date.
        /// </value>
        public virtual DateTime UtcUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public virtual string Site { get; set; }
        #endregion
    }
    public partial class Brand
    {
        
        public virtual EntityFile ImageFile { get; set; }
    }
}
