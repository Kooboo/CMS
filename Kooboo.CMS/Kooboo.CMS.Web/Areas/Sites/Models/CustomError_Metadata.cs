using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Models;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Account.Services;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{


	[GridAction(DisplayName = "Edit", ActionName = "Edit", RouteValueProperties = "name=StatusCode,StatusCode", Order = 1, Class = "o-icon edit dialog-link", Title = "Edit")]
	//[GridAction(DisplayName = "Delete", ActionName = "Delete", ConfirmMessage = "Are you sure you want to delete this item?", RouteValueProperties = "name=StatusCode,StatusCode", Order = 5)]
	[Grid(Checkable = true, IdProperty = "StatusCode")]
	public class CustomError_Metadata
	{
		[EnumDataType(typeof(HttpErrorStatusCode))]
		[UIHintAttribute("DropDownList")]
		[GridColumnAttribute(ItemRenderType = typeof(CommonLinkPopColumnRender))]
		[Remote("IsStatusCodeAvailable", "CustomError", AdditionalFields = "SiteName,old_Key")]
		public HttpErrorStatusCode StatusCode { get; set; }
		[GridColumnAttribute()]
		[Required(ErrorMessage = "Required")]
		[UIHint("AutoComplete")]
		[DataSource(typeof(AutoCompletePageListDataSouce))]
		public string RedirectUrl { get; set; }

        [GridColumn]
        [UIHint("DropDownList")]
        [EnumDataType(typeof(RedirectType))]
        [Display(Name = "Redirect type")]
        public RedirectType RedirectType { get; set; }
	}

	public class AutoCompletePageListDataSouce : ISelectListDataSource
	{
		public IEnumerable<SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
		{

			var siteName = requestContext.GetRequestValue("siteName");

			var site = new Site(siteName).AsActual();


			IEnumerable<Page> pageList = new List<Page>();

			var rootPages = Kooboo.CMS.Sites.Services.ServiceFactory.PageManager.All(site, null);

			pageList = rootPages.ToList();

			foreach (var r in rootPages)
			{
				this.GenerateList(site, r, ref pageList);
			}

			if (filter == null)
			{
				return null;
			}

			var result = pageList.Where(o => o.VirtualPath.StartsWith(filter, StringComparison.OrdinalIgnoreCase)).Select(o => new SelectListItem { Text = o.VirtualPath, Value = o.VirtualPath });

			return result;

		}

		private void GenerateList(Site site, Page page, ref IEnumerable<Page> pageList)
		{
			var children = Kooboo.CMS.Sites.Services.ServiceFactory.PageManager.ChildPages(site, page.FullName, null);

			pageList = pageList.Concat(children);

			foreach (var s in children)
			{
				this.GenerateList(site, s, ref pageList);
			}


		}
	}

}