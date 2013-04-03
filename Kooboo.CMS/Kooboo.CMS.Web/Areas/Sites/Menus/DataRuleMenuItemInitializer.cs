using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Menu;
using Kooboo.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Sites.Menus
{
	public abstract class DataRuleMenuItemInitializer : SiteAuthorizeMenuItemInitializer
	{
		public abstract string From { get; }
		#region IMenuItemStatus Members


		protected override bool GetIsActive(MenuItem item, System.Web.Mvc.ControllerContext controllerContext)
		{
			if (controllerContext.RouteData.Values["controller"].ToString().ToLower() == "datarule")
			{
				if (controllerContext.RequestContext.GetRequestValue("from").ToLower() == item.Controller.ToLower())
				{
					return true;
				}
				else
				{
					return false;
				}
			}

			return base.GetIsActive(item, controllerContext);
		}

		#endregion
	}

	public class ViewMenuItemInitializer : DataRuleMenuItemInitializer
	{
		public override string From
		{
			get { return "view"; }
		}
	}

	public class PageMenuItemInitializer : DataRuleMenuItemInitializer
	{
		public override string From
		{
			get { return "page"; }
		}

		protected override bool GetIsActive(MenuItem item, System.Web.Mvc.ControllerContext controllerContext)
		{
			string parent = controllerContext.RequestContext.GetRequestValue("fullName");
			var fullName = item.RouteValues["fullName"] == null ? "" : item.RouteValues["fullName"].ToString();

			return (string.Compare(parent, fullName, true) == 0 || string.IsNullOrEmpty(fullName)) && controllerContext.RequestContext.GetRequestValue("controller").ToLower() == "page";

		}
	}
}