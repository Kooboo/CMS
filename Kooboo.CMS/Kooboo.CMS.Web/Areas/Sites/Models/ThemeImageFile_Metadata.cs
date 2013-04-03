using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using Kooboo.Web;


namespace Kooboo.CMS.Web.Areas.Sites.Models
{

	[GridAction(ActionName = "DeleteImage", ConfirmMessage = "Are you sure you want to delete this image?", DisplayName = "Delete", RouteValueProperties = "Theme.Name,FileName")]
	public class ThemeImageFile_Metadata
	{
		[GridColumn(Order = 0)]
		public string Name { get; set; }
		[GridColumn(Order = 1, ItemRenderType = typeof(ImageRender), HeaderText = "Preview")]
		public string VirtualPath { get; set; }


	}
}