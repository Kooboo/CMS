using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.Web.Mvc.Grid
{
	public class PageSizeCommand
	{
		public PageSizeCommand(ViewContext viewContext, string pageIndexName, string pageIndexValue)
		{
			this.ActionName = viewContext.RouteData.Values["action"].ToString();
			this.ControllerName = viewContext.RouteData.Values["controller"].ToString();
			var routeValues = new RouteValueDictionary(viewContext.RouteData.Values);
			routeValues[pageIndexName] = pageIndexValue;
			this.RouteValues = routeValues;

			this.PageIndexName = pageIndexName;
			this.PageIndexValue = pageIndexValue;
		}
		public string ActionName { get; private set; }
		public string ControllerName { get; private set; }
		public RouteValueDictionary RouteValues { get; private set; }
		public string PageIndexName { get; private set; }
		public string PageIndexValue { get; private set; }
	}
}
