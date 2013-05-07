#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Web.Models;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    public class MediaFolderController : ManagerControllerBase
    {
        #region .ctor
        MediaContentManager ContentManager
        {
            get;
            set;
        }

        MediaFolderManager FolderManager
        {
            get;
            set;
        }
        public MediaFolderController(MediaContentManager mediaContentManager, MediaFolderManager mediaFolderManager)
        {
            this.ContentManager = mediaContentManager;
            this.FolderManager = mediaFolderManager;
        } 
        #endregion
        
        #region Index
        public virtual ActionResult Index(string fullName, string search)
        {
            var folders = FolderManager.All(Repository, search, fullName);

            return View(folders);
        } 
        #endregion

        [HttpPost]
        public virtual ActionResult Create(MediaFolder model, string folderName)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    MediaFolder parent = null;
                    if (!string.IsNullOrEmpty(folderName))
                    {
                        parent = FolderHelper.Parse<MediaFolder>(Repository, folderName).AsActual();
                    }
                    model.Parent = parent;

                    FolderManager.Add(Repository, model);
                });
            }

            return Json(data);
        }


        [HttpPost]
        public virtual ActionResult Edit(string folderName)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    var old = FolderManager.Get(Repository, folderName);
                    var @new = FolderManager.Get(Repository, folderName);
                    TryUpdateModel(@new);

                    FolderManager.Update(Repository, @new, old);

                    data.Model = new
                    {
                        folderName = @new.FullName
                    };
                });
            }

            return Json(data);
        }


        public virtual ActionResult Delete(string selectedFolders)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                var folderArr = selectedFolders.Split(',');
                foreach (var f in folderArr)
                {
                    if (string.IsNullOrEmpty(f)) continue;
                    FolderManager.Remove(Repository, new MediaFolder(Repository, f));
                }
            });
            return Json(data);
        }

    }
}
