using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Web.Models;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
	public class FileExtensionDataSource : ISelectListDataSource
	{
		public IEnumerable<SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
		{
			yield return new SelectListItem() { Text = ".txt", Value = ".txt" };
			yield return new SelectListItem() { Text = ".js", Value = ".js" };
			yield return new SelectListItem() { Text = ".css", Value = ".css" };
			yield return new SelectListItem() { Text = ".rule", Value = ".rule" };
		}
	}
	public class FileResource_Metadata
	{
		[Required(ErrorMessage = "Required")]
		[RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
		[RemoteEx("IsNameAvailable", "", AdditionalFields = "SiteName,old_Key,directoryPath,FileExtension")]
		public string Name { get; set; }

		[UIHint("DropDownList")]
		[DataSource(typeof(FileExtensionDataSource))]
		public string FileExtension { get; set; }
		[AllowHtml]
		[UIHintAttribute("TemplateEditor")]
		public string Body { get; set; }
	}
}