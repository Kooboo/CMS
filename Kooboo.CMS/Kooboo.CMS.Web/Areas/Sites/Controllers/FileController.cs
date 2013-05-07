#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Development", Name = "File", Order = 1)]
    public class FileController : Kooboo.CMS.Sites.AreaControllerBase
    {
        #region Properties
        FileManager _fileManager;
        public FileManager FileManager
        {
            get
            {
                if (_fileManager == null)
                {
                    var type = ControllerContext.RequestContext.GetRequestValue("type").ToLower();
                    _fileManager = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<FileManager>(type);
                }
                return _fileManager;
            }
            set
            {
                _fileManager = value;
            }
        }
        #endregion

        #region Methods

        public virtual ActionResult Index(string folderPath)
        {
            var fileGridModel = new ResourceGridModel()
            {
                Directory = FileManager.GetDirectory(Site, folderPath),
                Directories = FileManager.GetDirectories(Site, folderPath).OfType<DirectoryEntry>(),
                Files = FileManager.GetFiles(Site, folderPath)
            };
            ViewData["Title"] = FileManager.Type;

            return View(fileGridModel);
        }

        #region Folder
        [HttpPost]
        public virtual ActionResult CreateFolder(string folderPath, string folderName, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                FileManager.AddDirectory(Site, folderPath, folderName);

                data.ReloadPage = true;
            });
            return Json(data);

        }
        [HttpPost]
        public virtual ActionResult RenameFolder(string folderPath, string folderName)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                FileManager.RenameDirectory(Site, folderPath, folderName);
                data.ReloadPage = true;
            });
            return Json(data);
        }
        #endregion

        #region CreateFile

        [HttpPost]
        public virtual ActionResult Upload(string folderPath, string @return)
        {
            var file = Request.Files[0];

            var fileName = Path.GetFileName(file.FileName);

            FileManager.AddFile(Site, folderPath, fileName, file.InputStream);

            return Redirect(@return);
        }


        public virtual ActionResult IsNameAvailable(string folderPath, string name, string fileExtension, string old_Key)
        {
            if (old_Key == null || !name.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                var fileEntry = FileManager.GetFile(Site, FileManager.GetRelativePath(folderPath, name + fileExtension));

                if (fileEntry.Exists())
                {
                    return Json("The name already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult CreateFile(string folderPath)
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult CreateFile(string folderPath, string name, string fileExtension, string body, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                string fileName = name + fileExtension ?? "";

                FileManager.AddFile(Site, folderPath, fileName, body);
                data.RedirectUrl = @return;
            });

            return Json(data);
        }

        #endregion

        #region EditFile
        public virtual ActionResult EditFile(string relativePath)
        {
            var model = FileManager.GetFile(Site, relativePath);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult EditFile(string folderPath, string relativePath, string body, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                FileManager.EditFile(Site, relativePath, body);
                resultData.AddMessage("The item has been saved.".Localize());
            });
            return Json(data);
        }
        [HttpPost]
        public virtual ActionResult RenameFile(string relativePath, string fileName)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                FileManager.RenameFile(Site, relativePath, fileName);
                data.ReloadPage = true;
            });
            return Json(data);
        }
        #endregion

        #region Delete

        [HttpPost]
        public virtual ActionResult Delete(string directoryPath, string[] folders, string[] docs)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (folders != null)
                {
                    foreach (var d in folders)
                    {
                        FileManager.DeleteDirectory(Site, d);
                    }
                }

                if (docs != null)
                {
                    foreach (var f in docs)
                    {
                        FileManager.DeleteFile(Site, f);
                    }
                }
                resultData.ReloadPage = true;
            });
            return Json(data);
        }

        #endregion

        #region Import
        public virtual ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Import(string folderPath, bool @override, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                var file = Request.Files[0];

                FileManager.Import(Site, folderPath, file.InputStream, @override);

                data.RedirectUrl = @return;
            });
            return Json(data);
        }
        #endregion

        #region Export

        [HttpPost]
        public virtual void Export(string directoryPath, string[] folders, string[] docs)
        {
            var fileName = FileManager.Type + ".zip";
            Response.AttachmentHeader(fileName);

            FileManager.Export(Site, directoryPath, folders, docs, Response.OutputStream);

        }

        #endregion

        #region Sort
        public virtual ActionResult Sort(string folderPath, string[] fileNames)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                FileManager.SaveFileOrders(Site, folderPath, fileNames);
            });
            return Json(data);
        }
        #endregion

        #region IsFolderNameAvailable
        public virtual ActionResult IsFolderNameAvailable(string folderName, string old_Key, string folderPath)
        {

            if (FileManager.IsDirectoryExists(Site, folderPath, folderName))
            {
                return Json("The name already exists.".Localize(), JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region IsFileNameAvailable
        public virtual ActionResult IsFileNameAvailable(string fileName, string old_Key, string folderPath)
        {
            if (FileManager.IsFileExists(Site, folderPath, fileName))
            {
                return Json("The name already exists.".Localize(), JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

    }
}