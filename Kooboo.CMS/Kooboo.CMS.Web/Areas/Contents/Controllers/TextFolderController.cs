using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;

using Kooboo.Web.Mvc;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    public class TextFolderController : ManagerControllerBase
    {
        //
        // GET: /Contents/TextFolder/

        public TextFolderManager Manager
        {
            get
            {
                return ServiceFactory.TextFolderManager;
            }
        }
        // Kooboo.CMS.Account.Models.Permission.Contents_ContentPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult Index(string FolderName, string search)
        {
            var folders = Manager.All(Repository, search, FolderName);

            folders = folders
                .Select(it => it.AsActual())
                .Where(it => it.VisibleOnSidebarMenu == null || it.VisibleOnSidebarMenu.Value == true)
                .Where(it => Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager.AvailableViewContent(new TextFolder(Repository, it.FullName), User.Identity.Name)).ToArray();

            return View(folders);
        }


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

        public virtual ActionResult Create(TextFolder model, string folderName)
        {
            //compatible with the Folder parameter changed to FolderName.
            folderName = folderName ?? this.ControllerContext.RequestContext.GetRequestValue("Folder");

            JsonResultEntry resultEntry = new JsonResultEntry() { Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    Folder parent = null;
                    if (!string.IsNullOrEmpty(folderName))
                    {
                        parent = FolderHelper.Parse<TextFolder>(Repository, folderName);
                    }
                    model.Parent = parent;
                    model.UtcCreationDate = DateTime.UtcNow;
                    Manager.Add(Repository, model);


                    resultEntry.Success = true;

                    return Json(resultEntry);
                }
            }
            catch (Exception e)
            {
                resultEntry.SetFailed().AddException(e);
            }
            resultEntry.AddModelState(ViewData.ModelState);
            return Json(resultEntry);
        }

        #endregion


        #region Edit
        // Kooboo.CMS.Account.Models.Permission.Contents_FolderPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Folder", Order = 99)]
        public virtual ActionResult Edit(string fullName)
        {
            ViewData["textFolderList"] = Manager.All(Repository, "");
            return View(Manager.Get(Repository, fullName).AsActual());
        }

        // Kooboo.CMS.Account.Models.Permission.Contents_FolderPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Folder", Order = 99)]
        [HttpPost]
        public virtual ActionResult Edit(TextFolder model, string fullName, string successController)
        {
            JsonResultEntry resultEntry = new JsonResultEntry() { Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    fullName = string.IsNullOrEmpty(fullName) ? model.FullName : fullName;
                    var old = Manager.Get(Repository, fullName);
                    model.Parent = old.Parent;
                    Manager.Update(Repository, model, old);

                    var fromPop = ControllerContext.RequestContext.GetRequestValue("FromPop");

                    resultEntry.Success = true;
                }



            }
            catch (Exception e)
            {
                resultEntry.SetFailed().AddException(e);
            }
            resultEntry.AddModelState(ViewData.ModelState);
            return Json(resultEntry);
        }
        #endregion

        #region Delete
        // Kooboo.CMS.Account.Models.Permission.Contents_FolderPermission
        //[Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Folder", Order = 99)]
        [HttpPost]
        public virtual ActionResult Delete(string[] model)
        {
            var entry = new JsonResultEntry();
            try
            {
                foreach (var f in model)
                {
                    if (string.IsNullOrEmpty(f)) continue;
                    var folderPathArr = FolderHelper.SplitFullName(f);
                    Manager.Remove(Repository, new TextFolder(Repository, folderPathArr));
                }
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.SetFailed().AddException(e);
            }
            return Json(entry);

            //return RedirectToIndex();
        }
        #endregion


        public virtual ActionResult GetSchemaFields(string name)
        {
            var schema = new Schema(Repository, name).AsActual();

            if (schema == null)
            {
                schema = new Schema();
            }

            return Json(schema.AllColumns.Select(o =>
            new
            {
                Text = o.Name,
                Value = o.Name
            }), JsonRequestBehavior.AllowGet);
        }
    }
}
