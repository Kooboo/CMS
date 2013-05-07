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
using System.Web.Mvc;
using Kooboo.Web;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Web.Areas.Contents.Models;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Common;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [Kooboo.CMS.Web.Authorizations.RequiredLogOnAttribute(RequiredAdministrator = true, Order = 1)]
    public class RepositoryController : ControllerBase
    {
        RepositoryManager Manager { get; set; }
        public RepositoryController(RepositoryManager manager)
        {
            Manager = manager;
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Create(CreateRepositoryModel model)
        {
            JsonResultData data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    Manager.Create(model.Name, model.Template);
                    resultData.RedirectUrl = Url.Action("Index", new { controller = "home", repositoryName = model.Name });
                }
            });
            return Json(data);
        }

        public virtual ActionResult Delete(string repositoryName)
        {
            if (!string.IsNullOrEmpty(repositoryName))
            {
                var repository = new Repository(repositoryName);
                Manager.Remove(repository);
            }
            return RedirectToAction("Index", "Home");
        }

        public virtual ActionResult Export(string repositoryName)
        {
            string fileName = repositoryName + ".zip";
            Response.AttachmentHeader(fileName);
            Manager.Export(repositoryName, Response.OutputStream);
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
