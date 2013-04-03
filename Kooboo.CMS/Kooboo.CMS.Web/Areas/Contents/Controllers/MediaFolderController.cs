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
    public class MediaFolderController : ManagerControllerBase
    {
        //
        // GET: /Contents/MediaFolder/

        MediaContentManager ContentManager
        {
            get
            {
                return ServiceFactory.MediaContentManager;
            }
        }

        MediaFolderManager FolderManager
        {
            get
            {
                return ServiceFactory.MediaFolderManager;
            }
        }

        public virtual ActionResult Index(string fullName, string search)
        {
            var folders = FolderManager.All(Repository, search, fullName);

            return View(folders);
        }

        public virtual ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Create(MediaFolder model, string folderName)
        {
            var entry = new JsonResultEntry();
            if (ModelState.IsValid)
            {
                try
                {
                    MediaFolder parent = null;
                    if (!string.IsNullOrEmpty(folderName))
                    {
                        parent = FolderHelper.Parse<MediaFolder>(Repository, folderName).AsActual();
                    }
                    model.Parent = parent;

                    FolderManager.Add(Repository, model);

                }
                catch (Exception e)
                {
                    entry.AddException(e);
                }

            }
            else
            {
                entry.AddModelState(ModelState);
            }

            return Json(entry);
        }


        [HttpPost]
        public virtual ActionResult Edit(string folderName)
        {
            JsonResultEntry entry = new JsonResultEntry();
            if (ModelState.IsValid)
            {
                try
                {
                    var old = FolderManager.Get(Repository, folderName);
                    var @new = FolderManager.Get(Repository, folderName);
                    TryUpdateModel(@new);

                    FolderManager.Update(Repository, @new, old);

                    entry.Model = new
                    {
                        folderName = @new.FullName
                    };
                }
                catch (Exception e)
                {
                    entry.AddException(e);
                }
            }
            else
            {
                entry.AddModelState(ModelState);
            }

            return Json(entry);
        }


        public virtual ActionResult Delete(string selectedFolders)
        {
            var entry = new JsonResultEntry();
            try
            {
                var folderArr = selectedFolders.Split(',');
                foreach (var f in folderArr)
                {
                    if (string.IsNullOrEmpty(f)) continue;
                    FolderManager.Remove(Repository, new MediaFolder(Repository, f));
                }
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.SetFailed().AddException(e);
            }
            return Json(entry);
        }

    }
}
