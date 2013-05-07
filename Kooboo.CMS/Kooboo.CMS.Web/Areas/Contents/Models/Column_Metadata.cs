#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Form;
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{

    [MetadataFor(typeof(Column))]
    public class Column_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [Description("Name of your field, it uses the same naming rule as C# veriable name.")]
        [RegularExpression(RegexPatterns.VeriableName, ErrorMessage = "Invalid column name.")]
        public string Name { get; set; }

        [Description("Descriptive label of your field on the content editing page.")]
        public string Label { get; set; }

        [UIHint("DropDownList")]
        [EnumDataType(typeof(Kooboo.CMS.Common.DataType))]
        [Required(ErrorMessage = "Required")]
        public DataType DataType { get; set; }


        [UIHint("DropDownList")]
        [DataSource(typeof(Kooboo.CMS.Web.Areas.Contents.Models.DataSources.ControlTypesDataSource))]
        [Description("The way that you would like to input your content.")]
        public string ControlType { get; set; }

        [DisplayName("Allow null")]
        [Description("Allows null value in this field")]
        [Required(ErrorMessage = "Required")]
        public bool AllowNull { get; set; }
        [Required(ErrorMessage = "Required")]
        public int Length { get; set; }
        [Description("Order of your field in the content editing page.")]
        [Required(ErrorMessage = "Required")]
        public int Order { get; set; }

        [Description("Enable the modification of this field in the content editing page.")]
        [DefaultValue(true)]
        [Required(ErrorMessage = "Required")]
        public bool Modifiable { get; set; }

        [Description("Include this field in the Lucene.NET full text search index.")]
        [Required(ErrorMessage = "Required")]
        public bool Indexable { get; set; }

        [Description("Show this field in the CMS content list page.")]
        [Display(Name = "Content list page")]
        [Required(ErrorMessage = "Required")]
        public bool ShowInGrid { get; set; }

        [Description("Input tip while users are editing content.")]
        public string Tooltip { get; set; }

        [Display(Name = "Default value")]
        public string DefaultValue { get; set; }

        [Display(Name = "Summary field")]
        [Description("The Summary field will be used as a title or summary to describe your content item.")]
        [Required(ErrorMessage = "Required")]
        public bool Summarize { get; set; }

        [UIHint("List_SelectItem")]
        public IEnumerable<Kooboo.CMS.Form.SelectListItem> SelectionItems { get; set; }

        [UIHint("SingleFolderTree")]
        public string SelectionFolder { get; set; }

        [UIHint("ColumnValidations")]
        public IEnumerable<ColumnValidation> Validations { get; set; }
        [UIHint("Dictionary")]
        [Description("The other custom settings for the field.")]
        public Dictionary<string, string> CustomSettings { get; set; }
    }
}