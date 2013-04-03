using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Sites.Models;

using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.Extension;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Extensions;
namespace Kooboo.CMS.Web.Areas.Sites.Models
{
	[GridAction(ActionName = "ViewTypes", DisplayName = "View", Class = "o-icon view dialog-link", Order = 1, RouteValueProperties = "FileName")]
	//[GridAction(ActionName = "Delete", ConfirmMessage = "Are you sure you want to delete this assembly?", Order = 10, RouteValueProperties = "FileName")]
	[Grid(Checkable = true, IdProperty = "FileName")]
	public class AssemblyFile_Metadata
	{
		[GridColumn(Order = 1)]
		public string FileName { get; set; }
	}
	public class UploadAssemblyViewModel
	{
        [Required(ErrorMessage = "Required")]
		[UIHint("file")]
		[RegularExpression(".+\\.dll")]
		public string File { get; set; }
	}

	public class PluginDataSource : ISelectListDataSource
	{
		public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
		{
			var site = Site.Current;
			var types = ServiceFactory.GetService<AssemblyManager>().GetTypes(site, typeof(IPagePlugin));
			return types.Select(o => new SelectListItem { Text = o.Name, Value = o.AssemblyQualifiedNameWithoutVersion() });
		}
	}

}