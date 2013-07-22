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

        public virtual ActionResult Guide()
        {
            var repositoryName = this.Request.RequestContext.GetRequestValue("repositoryName");//ControllerContext.RouteData.Values["repositoryName"];

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

                if (CMS.Sites.Services.ServiceFactory.UserManager.Authorize(CMS.Sites.Models.Site.Current, User.Identity.Name, CMS.Account.Models.Permission.Contents_SchemaPermission))
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
        public ActionResult Index()
        {
            var list = Manager.All();
            return View(list);
        }
        #endregion

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
