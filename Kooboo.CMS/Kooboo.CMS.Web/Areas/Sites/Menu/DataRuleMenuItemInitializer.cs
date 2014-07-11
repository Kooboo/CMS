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
using System.Web;
using System.Web.Routing;

using Kooboo.Common.Web.Menu;

namespace Kooboo.CMS.Web.Areas.Sites.Menu
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
}