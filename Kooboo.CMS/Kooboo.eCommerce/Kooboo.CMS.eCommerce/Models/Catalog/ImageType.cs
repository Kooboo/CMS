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
    /// 定义图片的规格
    /// </summary>
    public class ImageType : INonRelationEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public virtual int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public virtual int Height { get; set; }


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
    }
}
