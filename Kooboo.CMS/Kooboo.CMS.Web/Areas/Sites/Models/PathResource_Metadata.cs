using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc.Grid;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
	public class PathResource_Metadata
	{
		[Required()]
		[RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
		public string Name { get; set; }
	}
}