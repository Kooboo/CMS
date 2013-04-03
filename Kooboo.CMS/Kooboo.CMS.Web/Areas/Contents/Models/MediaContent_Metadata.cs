using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
	public class BinaryContentRouteValueGetter : IGridActionRouteValuesGetter
	{

		public System.Web.Routing.RouteValueDictionary GetValues(object dataItem, System.Web.Routing.RouteValueDictionary routeValueDictionary, System.Web.Mvc.ViewContext viewContext)
		{
			routeValueDictionary["folder"] = viewContext.HttpContext.Request["folder"];
			return routeValueDictionary;
		}
	}
	[GridAction(Order = 0, ActionName = "Edit", RouteValueProperties = "FileName,UUID=UUID")]
	[GridAction(ActionName = "Delete", Order = 1, RouteValueProperties = "FileName,UUID=UUID", ConfirmMessage = "Are you sure you want to delete this item?")]
	public class MediaContent_Metadata
	{
		[GridColumn()]
		public string FileName { get; set; }

		public string VirtualPath { get; set; }

		public string PhysicalPath { get; set; }

		public string Url { get; set; }

		public string UserKey { get; set; }
	}
}