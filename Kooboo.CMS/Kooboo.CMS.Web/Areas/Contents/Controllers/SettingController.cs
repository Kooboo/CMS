using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.Web;
using Kooboo.Globalization;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
	//Kooboo.CMS.Account.Models.Permission.Contents_SettingPermission
	[Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Setting")]
	public class SettingController : ManagerControllerBase
	{
		[HttpGet]
		public virtual ActionResult Index()
		{
			return View(Repository);
		}
		public virtual ActionResult Index(Repository model)
		{
			JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
			try
			{
				if (ModelState.IsValid)
				{
					RepositoryManager.Update(model, RepositoryManager.Get(model.Name));
					resultEntry.AddMessage("Database setting has been changed.".Localize());
				}
			}
			catch (Exception e)
			{
				resultEntry.AddException(e);
			}
			return Json(resultEntry);
		}
	}
}
