using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;

using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Kooboo.CMS.Content.Models;

using System.ComponentModel;
using System.Web.Routing;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Form;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
	public class ControlTypeDataSource : ISelectListDataSource
	{

		public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
		{
			var controls = Kooboo.CMS.Form.Html.ControlHelper.ResolveAll();
			foreach (var c in controls)
			{
				yield return new System.Web.Mvc.SelectListItem() { Text = c, Value = c };
			}
		}
	}

	public class ColumnLinkPopColumnRender : IItemColumnRender
	{

		public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
		{
			UrlHelper url = new UrlHelper(viewContext.RequestContext);

			return new HtmlString(string.Format(@"<a href=""{0}"" class=""dialog-link"" title=""{1}"">{2}</a>", url.Action("EditColumn", viewContext.RequestContext.AllRouteValues().Merge("ColumnName", value)), "Edit field".Localize(), value));
		}
	}



	[Grid(ItemDetailView = "Column", IdProperty = "Name")]
	[GridAction(ActionName = "EditColumn", DisplayName = "Edit", RouteValueProperties = "columnName = Name", Order = 0, Class = "o-icon edit dialog-link-column", Title = "Edit")]
	[GridAction(ActionName = "DeleteColumn", Class = "o-icon delete", ConfirmMessage = "Are you sure you want to delete this item?", DisplayName = "Delete", RouteValueProperties = "columnName = Name", Order = 1)]
	public class Column_Metadata
	{
		[GridColumn(Order = 1)]
		[Required(ErrorMessage = "Required")]
		[Description("Name of your field, excludes the following characters:\\/:*?<>|")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
		public string Name { get; set; }
		[GridColumn(Order = 3)]
		[Description("Descriptive label of your field on the content editing page")]
		public string Label { get; set; }

		[GridColumn(Order = 5)]
		[UIHint("DropDownList")]
		[EnumDataType(typeof(Kooboo.Data.DataType))]
        [Required(ErrorMessage = "Required")]
		public DataType DataType { get; set; }


		[UIHint("DropDownList")]
		[DataSource(typeof(ControlTypeDataSource))]
		[GridColumn(Order = 7)]
		[Description("The way that you would like to input your content")]
		public string ControlType { get; set; }
		[GridColumn(Order = 9, ItemRenderType = typeof(BooleanColumnRender))]
		[Description("Allows null value in this field")]
        [Required(ErrorMessage="Required")]
		public bool AllowNull { get; set; }
		[GridColumn(Order = 11)]
        [Required(ErrorMessage = "Required")]
		public int Length { get; set; }
		[GridColumn(Order = 21)]
		[Description("Order of your field in the content editing page")]
        [Required(ErrorMessage = "Required")]
		public int Order { get; set; }


		/// <summary>
		/// to be removed. this is not used any more. 
		/// </summary>
		//[GridColumn(Order = 15, ItemRenderType = typeof(BooleanColumnRender))]
		//public bool Queryable { get; set; }


		[Description("Enable the modification of this field in the content editing page.")]
		[GridColumn(Order = 17, ItemRenderType = typeof(BooleanColumnRender))]
		[DefaultValue(true)]
        [Required(ErrorMessage = "Required")]
		public bool Modifiable { get; set; }

		/// <summary>
		/// Used on full text index. 
		/// </summary>

		[Description("Include this field in the Lucene.NET full text search index")]
		[GridColumn(Order = 19, ItemRenderType = typeof(BooleanColumnRender))]
        [Required(ErrorMessage = "Required")]
		public bool Indexable { get; set; }


		[Description("Show this field in the CMS content list page")]
		[Display(Name = "Content list page")]
		[GridColumn(Order = 13, ItemRenderType = typeof(BooleanColumnRender))]
        [Required(ErrorMessage="Required")]
		public bool ShowInGrid { get; set; }

		[Description("Input tip while users entering content")]
		[GridColumn(Order = 23)]
		public string Tooltip { get; set; }

		[GridColumn(Order = 8)]
		[Display(Name = "Default value")]
		public string DefaultValue { get; set; }

		[Display(Name = "Summary field")]
        [Description("Summary fields will be used as a title or summary to describe your content item")]
		[GridColumn(Order = 10, ItemRenderType = typeof(BooleanColumnRender))]
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