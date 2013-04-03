using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Web.Areas.Contents.Models;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Sites;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [Kooboo.CMS.Web.Authorizations.RequiredLogOnAttribute(RequiredAdministrator = true, Order = 1)]
    public class RepositoryController : ControllerBase
    {
        [HttpGet]
        public virtual ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Create(CreateRepositoryModel model)
        {
            JsonResultEntry resultEntry = new JsonResultEntry() { Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceFactory.RepositoryManager.Create(model.Name, model.Template);
                    resultEntry.RedirectUrl = Url.Action("Index", new { controller = "home", repositoryName = model.Name });
                }

            }
            catch (Exception e)
            {
                resultEntry.SetFailed().AddException(e);
            }
            resultEntry.AddModelState(ViewData.ModelState);
            return Json(resultEntry);
        }

        public virtual ActionResult Delete(string repositoryName)
        {
            if (!string.IsNullOrEmpty(repositoryName))
            {
                var repository = new Repository(repositoryName);
                ServiceFactory.RepositoryManager.Remove(repository);
            }
            return RedirectToAction("Index", "Home");
        }

        public virtual ActionResult Export(string repositoryName)
        {
            string fileName = repositoryName + ".zip";
            Response.AttachmentHeader(fileName);
            Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Export(repositoryName, Response.OutputStream);
            return null;
        }

        /// <summary>
        /// Remote attribute
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual ActionResult IsNameAvailable(string name)
        {
            Repository repository = new Repository(name);
            RepositoryPath path = new RepositoryPath(repository);
            if (!path.Exists())
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            int i = 1;
            while (path.Exists())
            {
                repository = new Repository(name + i.ToString());
                path = new RepositoryPath(repository);
            }
            return Json(string.Format("{0} is not available. Try {1}.", name, repository.Name), JsonRequestBehavior.AllowGet);
        }

    }
}
