using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
	[GridAction(ActionName = "DeleteStyle",
		ConfirmMessage = "Are you sure you want to delete this file?",
		Order = 3,
		RouteValueProperties = "FileName", DisplayName = "Delete"
		)]
	[GridAction(ActionName = "EditStyle",
		Order = 0,
		DisplayName = "Edit", RouteValueProperties = "FileName", Icon = "Edit.gif"
		)]
	public class Style_Metadata
	{
		[GridColumn()]
		public string Name { get; set; }
	}
}