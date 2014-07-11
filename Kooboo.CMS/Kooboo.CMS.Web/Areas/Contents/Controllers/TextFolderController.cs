#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Web.Models;
using Kooboo.Common.Globalization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Kooboo.Common.Web;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    public class TextFolderController : ManagerControllerBase
    {
        #region .ctor
        TextFolderManager Manager
        {
            get;
            set;
        }
        public TextFolderController(TextFolderManager manager)
        {
            Manager = manager;
        }
        #endregion

        #region Index
        // Kooboo.CMS.Account.Models.Permission.Contents_ContentPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult Index(string FolderName, string search)
        {
            var folders = Manager.All(Repository, search, FolderName);

            folders = folders
                .Select(it => it.AsActual())
                .Where(it => it.Visible)
                .Where(it => Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager.AvailableViewContent(new TextFolder(Repository, it.FullName), User.Identity.Name)).ToArray();

            return View(folders);
        }
        #endregion


        #region Create
        // Kooboo.CMS.Account.Models.Permission.Contents_FolderPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Folder", Order = 99)]
        public virtual ActionResult Create()
        {
            ViewData["textFolderList"] = Manager.All(Repository, "");
            return View(new TextFolder());
        }
        // Kooboo.CMS.Account.Models.Permission.Contents_FolderPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Folder", Order = 99)]
        [HttpPost]

        public virtual ActionResult Create(TextFolder model, string folderName, string @return)
        {
            //compatible with the Folder parameter changed to FolderName.
            folderName = folderName ?? this.ControllerContext.RequestContext.GetRequestValue("Folder");

            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {

                    Folder parent = null;
                    if (!string.IsNullOrEmpty(folderName))
                    {
                        parent = FolderHelper.Parse<TextFolder>(Repository, folderName);
                    }
                    model.Parent = parent;
                    model.UtcCreationDate = DateTime.UtcNow;
                    Manager.Add(Repository, model);

                    resultData.RedirectUrl = @return;
                });


            }

            return Json(data);
        }

        #endregion


        #region Edit
        // Kooboo.CMS.Account.Models.Permission.Contents_FolderPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Folder", Order = 99)]
        public virtual ActionResult Edit(string UUID)
        {
            ViewData["textFolderList"] = Manager.All(Repository, "");
            return View(Manager.Get(Repository, UUID).AsActual());
        }

        // Kooboo.CMS.Account.Models.Permission.Contents_FolderPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Folder", Order = 99)]
        [HttpPost]
        public virtual ActionResult Edit(TextFolder model, string UUID, string successController, string @return)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    UUID = string.IsNullOrEmpty(UUID) ? model.FullName : UUID;
                    var old = Manager.Get(Repository, UUID);
                    model.Parent = old.Parent;
                    Manager.Update(Repository, model, old);

                    resultData.RedirectUrl = @return;
                });
            }
            return Json(data);
        }
        #endregion

        #region Delete
        // Kooboo.CMS.Account.Models.Permission.Contents_FolderPermission
        //[Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Folder", Order = 99)]
        [HttpPost]
        public virtual ActionResult Delete(TextFolder[] model)
        {
            ModelState.Clear();
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                foreach (var folder in model)
                {
                    folder.Repository = Repository;
                    Manager.Remove(Repository, folder);
                }
                resultData.ReloadPage = true;
            });
            return Json(data);
        }
        #endregion

        #region Relations
        public virtual ActionResult Relations(string uuid)
        {
            var model = Manager.Relations(new TextFolder(Repository, uuid));

            return View(model);
        }
        #endregion

        public virtual ActionResult GetSchemaFields(string schemaName)
        {
            var schema = new Schema(Repository, schemaName).AsActual();

            if (schema == null)
            {
                schema = new Schema();
            }

            return Json(schema.AllColumns.Select(o =>
            new
            {
                id = o.Name,
                text = o.Name
            }), JsonRequestBehavior.AllowGet);
        }
    }
}
