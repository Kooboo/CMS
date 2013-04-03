using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.Models;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc.Grid;
using System.Web.Mvc;
using Kooboo.CMS.Content.Models;


namespace Kooboo.CMS.Web.Areas.Sites.Models
{
	public class DataRuleFormModel
	{
		public IEnumerable<TextFolder> FolderList { get; set; }
		public IEnumerable<TextContent> ContentList { get; set; }
		public TextFolder Current { get; set; }
		public DataRuleSetting DataRuleSetting { get; set; }
	}

	public class DataRuleSettingRouteGetter : IGridActionRouteValuesGetter
	{
		public System.Web.Routing.RouteValueDictionary GetValues(object dataItem, System.Web.Routing.RouteValueDictionary routeValueDictionary, ViewContext viewContext)
		{
			routeValueDictionary["from"] = viewContext.HttpContext.Request["from"];
			routeValueDictionary["fromName"] = viewContext.HttpContext.Request["fromName"];
			return routeValueDictionary;
		}
	}
	[GridAction(ActionName = "Edit", RouteValueProperties = "Name=DataName", RouteValuesGetter = typeof(DataRuleSettingRouteGetter))]
	[GridAction(ActionName = "Delete", RouteValueProperties = "Name=DataName", RouteValuesGetter = typeof(DataRuleSettingRouteGetter), ConfirmMessage = "Are you sure you want to delete this item?")]
	public class DataRuleSetting_Metadata
	{
		[Required(ErrorMessage = "Required")]
		[GridColumn(Order = 1)]
		public string DataName { get; set; }
		[UIHint("DataRuleBase")]
		public IDataRule DataRule { get; set; }
		[GridColumn(Order = 5)]
		[UIHint("DropDownList")]
		[EnumDataType(typeof(TakeOperation))]
		public TakeOperation TakeOperation { get; set; }
	}

	public class DataRuleBase_Metadata
	{
		public IEnumerable<WhereClause> WhereClauses { get; set; }
		public string SortField { get; set; }
		[UIHint("DropDownList")]
		[EnumDataType(typeof(SortDirection))]
		public SortDirection SortDirection { get; set; }
		public string PageIndex { get; set; }
		public string PageSize { get; set; }
		public string Top { get; set; }
	}

	public static class DataRuleTypeDataSource
	{
		public static IEnumerable<SelectListItem> DropdownListDataSource(string selectName)
		{
			var results = Enum
				.GetNames(typeof(DataRuleType))
				.Select(o => new SelectListItem()
				{
					Text = o,
					Selected = o == selectName,
					Value = ((int)Enum.Parse(typeof(DataRuleType), o)).ToString()
				});
			return results;
		}
	}


}