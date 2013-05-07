#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Models.Catalog
{
    /// <summary>
    /// 产品的自定义字段
    /// </summary>
    public partial class ProductFieldType : IColumn
    {
        #region IColumn Members
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public virtual string Label
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public virtual DataType DataType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public virtual int Length
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the control.
        /// </summary>
        /// <value>
        /// The type of the control.
        /// </value>
        public virtual string ControlType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        public virtual string Tooltip
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selection source.
        /// </summary>
        /// <value>
        /// The selection source.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual SelectionSource SelectionSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selection items.
        /// </summary>
        /// <value>
        /// The selection items.
        /// </value>
        public virtual SelectListItem[] SelectionItems
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selection folder.
        /// </summary>
        /// <value>
        /// The selection folder.
        /// </value>
        public virtual string SelectionFolder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the validations.
        /// </summary>
        /// <value>
        /// The validations.
        /// </value>
        public virtual ColumnValidation[] Validations
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public virtual string DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public virtual int Order
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IColumn" /> is modifiable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if modifiable; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Modifiable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show in grid].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show in grid]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool ShowInGrid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is system field.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is system field; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsSystemField
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [allow null].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow null]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool AllowNull
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the custom settings.
        /// </summary>
        /// <value>
        /// The custom settings.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual Dictionary<string, string> CustomSettings
        {
            get;
            set;
        }
        #endregion
    }
}
