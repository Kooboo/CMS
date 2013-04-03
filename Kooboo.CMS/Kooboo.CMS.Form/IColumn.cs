using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kooboo.Data;

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
    public enum SelectionSource
    {
        ManuallyItems = 0,
        TextFolder = 1
    }
    public interface IColumn
    {
        string Name { get; set; }
        string Label { get; set; }
        DataType DataType { get; set; }
        int Length { get; set; }
        string ControlType { get; set; }
        string Tooltip { get; set; }

        SelectionSource SelectionSource { get; set; }
        SelectListItem[] SelectionItems { get; set; }
        string SelectionFolder { get; set; }
        ColumnValidation[] Validations { get; set; }
        string DefaultValue { get; set; }
        int Order { get; set; }
        bool Modifiable { get; set; }
        bool ShowInGrid { get; set; }
        bool IsSystemField { get; set; }
        bool AllowNull { get; set; }
        Dictionary<string, string> CustomSettings { get; set; }
    }
}
