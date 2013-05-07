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
using System.Runtime.Serialization;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Form
{
    [Serializable]
    [DataContract]
    public class SelectListItem
    {
        // Summary:
        //     Initializes a new instance of the System.Web.Mvc.SelectListItem class.
        public SelectListItem() { }

        // Summary:
        //     Gets or sets a value that indicates whether this System.Web.Mvc.SelectListItem
        //     is selected.
        //
        // Returns:
        //     true if the item is selected; otherwise, false.
        [DataMember(Order = 1)]
        public bool Selected { get; set; }
        //
        // Summary:
        //     Gets or sets the text of the selected item.
        //
        // Returns:
        //     The text.
        [DataMember(Order = 3)]
        public string Text { get; set; }
        //
        // Summary:
        //     Gets or sets the value of the selected item.
        //
        // Returns:
        //     The value.
        [DataMember(Order = 5)]
        public string Value { get; set; }
    }

    /// <summary>
    /// 选项字段的数据来源
    /// </summary>
    public enum SelectionSource
    {
        ManuallyItems = 0,
        TextFolder = 1
    }

    /// <summary>
    /// 字段定义
    /// </summary>
    public interface IColumn
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        string Label { get; set; }
        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        DataType DataType { get; set; }
        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        int Length { get; set; }
        /// <summary>
        /// Gets or sets the type of the control.
        /// </summary>
        /// <value>
        /// The type of the control.
        /// </value>
        string ControlType { get; set; }
        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        string Tooltip { get; set; }

        /// <summary>
        /// Gets or sets the selection source.
        /// </summary>
        /// <value>
        /// The selection source.
        /// </value>
        SelectionSource SelectionSource { get; set; }
        /// <summary>
        /// Gets or sets the selection items.
        /// </summary>
        /// <value>
        /// The selection items.
        /// </value>
        SelectListItem[] SelectionItems { get; set; }
        /// <summary>
        /// Gets or sets the selection folder.
        /// </summary>
        /// <value>
        /// The selection folder.
        /// </value>
        string SelectionFolder { get; set; }
        /// <summary>
        /// Gets or sets the validations.
        /// </summary>
        /// <value>
        /// The validations.
        /// </value>
        ColumnValidation[] Validations { get; set; }
        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        string DefaultValue { get; set; }
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        int Order { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IColumn" /> is modifiable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if modifiable; otherwise, <c>false</c>.
        /// </value>
        bool Modifiable { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [show in grid].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show in grid]; otherwise, <c>false</c>.
        /// </value>
        bool ShowInGrid { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is system field.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is system field; otherwise, <c>false</c>.
        /// </value>
        bool IsSystemField { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [allow null].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [allow null]; otherwise, <c>false</c>.
        /// </value>
        bool AllowNull { get; set; }
        /// <summary>
        /// Gets or sets the custom settings.
        /// </summary>
        /// <value>
        /// The custom settings.
        /// </value>
        Dictionary<string, string> CustomSettings { get; set; }
    }
}
