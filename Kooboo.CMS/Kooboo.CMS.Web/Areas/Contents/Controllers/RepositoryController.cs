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
using Kooboo.Web.Mvc;
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [Kooboo.CMS.Web.Authorizations.RequiredLogOnAttribute(RequiredAdministrator = true, Order = 1)]
    public class RepositoryController : ControllerBase
    {
        #region .ctor
        RepositoryManager Manager { get; set; }
        public RepositoryController(RepositoryManager manager)
        {
            Manager = manager;
        }
        #endregion

        #region Guide
        private enum ContentEditStatus
        {
            Disable,
            Enable,
            Done
        }

        private class ContentEditStep
        {
            public ContentEditStatus Status { get; set; }
            public string ActionUrl { get; set; }
        }

        private class ContentMapModel
        {
            public ContentEditStep Repository
            {
                get
                {
                    return repository;
                }
                set
                {
                    repository = value;
                }
            }
            private ContentEditStep repository = new ContentEditStep();

            public ContentEditStep ContentType
            {
                get
                {
                    return contentType;
                }
                set
                {
                    contentType = value;
                }
            }
            private ContentEditStep contentType = new ContentEditStep();

            public ContentEditStep TextFolder
            {
                get
                {
                    return textFolder;
                }
                set
                {
                    textFolder = value;
                }
            }
            private ContentEditStep textFolder = new ContentEditStep();

            public ContentEditStep TextContent
            {
                get
                {
                    return textContent;
                }
                set
                {
                    textContent = value;
                }
            }
            private ContentEditStep textContent = new ContentEditStep();

            public ContentEditStep MediaContent
            {
                get
                {
                    return mediaContent;
                }
                set
                {
                    mediaContent = value;
                }
            }
            private ContentEditStep mediaContent = new ContentEditStep();
        }

        public virtual ActionResult Guide(string repositoryName, string uuid)
        {
            if (string.IsNullOrEmpty(repositoryName) && !string.IsNullOrEmpty(uuid))
            {
                return RedirectToAction("Guide", new { repositoryName = uuid });
            }


            ContentMapModel mapModel = new ContentMapModel();

            if (string.IsNullOrEmpty(repositoryName))
            {
                mapModel.Repository = new ContentEditStep
                {
                    Status = ContentEditStatus.Enable,
                    ActionUrl = this.Url.Action("Create", "Repository")
                };
            }
            else
            {
                var allRequestValue = Request.RequestContext.AllRouteValues();

                var repository = ServiceFactory.RepositoryManager.Get(repositoryName);


                mapModel.Repository.Status = ContentEditStatus.Disable;

                if (CMS.Sites.Services.ServiceFactory.UserManager.Authorize(CMS.Sites.Models.Site.Current, User.Identity.Name, Kooboo.CMS.Account.Models.Permission.Contents_ContentPermission))
                {
                    mapModel.ContentType.Status = ContentEditStatus.Done;

                    mapModel.ContentType.ActionUrl = this.Url.Action("Index", "Schema", allRequestValue);
                }
                else
                {
                    mapModel.ContentType.Status = ContentEditStatus.Disable;
                }

                if (CMS.Sites.Services.ServiceFactory.UserManager.Authorize(CMS.Sites.Models.Site.Current, User.Identity.Name, CMS.Account.Models.Permission.Contents_FolderPermission))
                {
                    mapModel.TextFolder.Status = ContentEditStatus.Enable;
                    mapModel.TextFolder.ActionUrl = this.Url.Action("Index", "TextFolder", allRequestValue);
                }
                else
                {
                    mapModel.ContentType.Status = ContentEditStatus.Disable;
                }

                var textFolder = ServiceFactory.TextFolderManager.All(repository, null);

                if (textFolder != null && textFolder.Count() > 0)
                {
                    if (CMS.Sites.Services.ServiceFactory.UserManager.Authorize(CMS.Sites.Models.Site.Current, User.Identity.Name, CMS.Account.Models.Permission.Contents_ContentPermission))
                    {
                        mapModel.TextFolder.Status = ContentEditStatus.Done;
                        mapModel.TextContent.Status = ContentEditStatus.Enable;

                        mapModel.TextContent.ActionUrl = Url.Action("Index", "TextFolder", allRequestValue);
                    }
                    else
                    {
                        mapModel.TextContent.Status = ContentEditStatus.Disable;
                    }
                }
                if (CMS.Sites.Services.ServiceFactory.UserManager.Authorize(CMS.Sites.Models.Site.Current, User.Identity.Name, CMS.Account.Models.Permission.Contents_ContentPermission))
                {
                    mapModel.MediaContent.Status = ContentEditStatus.Enable;
                    mapModel.MediaContent.ActionUrl = Url.Action("Index", "MediaContent", allRequestValue);
                }
                else
                {
                    mapModel.MediaContent.Status = ContentEditStatus.Disable;
                }
            }

            return View(mapModel);
        }
        #endregion

        #region Index
        public ActionResult Index(string search)
        {
            var list = Manager.All().Select(it => it.AsActual());
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(it => it.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || (!string.IsNullOrEmpty(it.DisplayName) && it.DisplayName.Contains(search, StringComparison.OrdinalIgnoreCase))); ;
            }
            return View(list);
        }
        #endregion

        #region Edit
        [HttpGet]
        public virtual ActionResult Edit(string repositoryName)
        {
            return View(new Repository(repositoryName).AsActual());
        }
        public virtual ActionResult Edit(Repository model, string @return)
        {
            JsonResultData data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    Manager.Update(model, Manager.Get(model.Name));
                    if (!string.IsNullOrEmpty(@return))
                    {
                        data.RedirectUrl = @return;
                    }
                    else
                    {
                        data.ReloadPage = true;
                    }
                }
            });
            return Json(data);
        }
        #endregion

        #region Create
        [HttpGet]
        public virtual ActionResult Create()
        {
            return View(new Repository());
        }
        [HttpPost]
        public virtual ActionResult Create(Repository repository)
        {
            JsonResultData data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    Manager.Add(repository);
                    resultData.RedirectUrl = Url.Action("Guide", new { controller = "Repository", repositoryName = repository.Name });
                }
            });
            return Json(data);
        }
        #endregion

        #region Delete

        [HttpPost]
        public virtual ActionResult Delete(Repository[] model, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        Manager.Remove(item);
                    }
                    resultData.RedirectUrl = @return;
                }
            });
            return Json(data);
        }
        #endregion

        #region Export
        [HttpPost]
        public virtual ActionResult Export(Repository[] model)
        {
            if (model != null || model.Length > 1)
            {
                string fileName = model.First().Name + ".zip";
                Response.AttachmentHeader(fileName);
                Manager.Export(model.First().Name, Response.OutputStream);

            }
            return null;
        }
        #endregion

        #region Import

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Import(ImportRepositoryModel model, string @return)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    Manager.Create(model.Name, model.File.InputStream);
                    data.RedirectUrl = @return;
                });
            }
            return Json(data, "text/plain", System.Text.Encoding.UTF8);
        }
        #endregion
        #region IsNameAvailable

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
            return Json(string.Format("Duplicate name. Try {1}.", name, repository.Name), JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
