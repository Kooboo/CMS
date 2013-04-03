using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web.Areas.Sites.Models;
using System.IO;

using Kooboo.Web.Mvc;

using Kooboo.Web.Script.Serialization;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Web.Authorizations;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    public abstract class FileManagerControllerBase : AdminControllerBase
    {
        protected FileManagerControllerBase(FileManager manager)
        {
            this.FileManager = manager;
        }
        public FileManager FileManager { get; set; }

        private string GetView(string viewName)
        {
            return AreaHelpers.CombineAreaFileVirtualPath(SitesAreaRegistration.SiteAreaName, "views", "FileManager", viewName);
        }
        //DirectoryResource GenerateDirectory(string virtualPath)
        //{
        //    DirectoryResource dir;
        //    if (string.IsNullOrEmpty(virtualPath))
        //    {
        //        dir = RootDir;
        //    }
        //    else
        //        dir = new DirectoryEntry(Server.MapPath(virtualPath));
        //    return dir;
        //}

        //private string GenerateFilePath(string virtualPath, string fileName)
        //{
        //    return Path.Combine(GenerateDirectory(virtualPath).PhysicalPath, fileName);
        //}

        //private FileEntry GenerateFileEntry(string virtualPath, string fileName)
        //{

        //    return new FileEntry(GenerateFilePath(virtualPath, fileName));
        //}

        public virtual ActionResult Index(string directoryPath)
        {
            var fileGridModel = new ResourceGridModel()
            {
                Directory = FileManager.GetDirectory(Site, directoryPath),
                Directories = FileManager.GetDirectories(Site, directoryPath).OfType<DirectoryEntry>(),
                Files = FileManager.GetFiles(Site, directoryPath)
            };


            return View(GetView("Index.aspx"), fileGridModel);
        }

        #region File
        #region CreateFile
        [HttpPost]
        public virtual ActionResult Upload(string directoryPath)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                var file = Request.Files[0];

                var fileName = Path.GetFileName(file.FileName);

                FileManager.AddFile(Site, directoryPath, fileName, file.InputStream);

            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry, "text/plain", System.Text.Encoding.UTF8);
        }

        public virtual ActionResult CreateFile(string directoryPath)
        {
            return View(GetView("CreateFile.aspx"));
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult CreateFile(string directoryPath, string name, string fileExtension, string body)
        {
            JsonResultEntry result = new JsonResultEntry(ModelState);
            try
            {
                string fileName = name + fileExtension ?? "";

                FileManager.AddFile(Site, directoryPath, fileName, body);

            }
            catch (Exception e)
            {
                result.AddException(e);
            }
            return Json(result);
        }

        #endregion

        #region Large File

        public virtual ActionResult LargeFile()
        {
            return View(GetView("LargeFile.aspx"));
        }

        [HttpPost]
        public virtual ActionResult LargeFile(string directoryPath)
        {
            //The directoryPath bound in the action parameter will lost the '/'
            directoryPath = this.ControllerContext.RequestContext.GetRequestValue("directoryPath");
            var entry = new JsonResultEntry();
            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var fileName = Path.GetFileName(file.FileName);

                    FileManager.AddFile(Site, directoryPath, fileName, file.InputStream);
                }

            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }

        #endregion

        #region EditFile
        public virtual ActionResult EditFile(string virtualPath)
        {
            var model = FileManager.GetFile(Site, virtualPath);
            return View(GetView("EditFile.aspx"), model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult EditFile(string directoryPath, string virtualPath, string name, string fileExtension, string body)
        {
            JsonResultEntry result = new JsonResultEntry(ModelState);
            try
            {
                //var old = Manager.GetFile(Server.MapPath(virtualPath));

                //var fileName = entry.Name + "." + (string.IsNullOrWhiteSpace(entry.FileExtension) ? "" : entry.FileExtension.TrimStart('.'));

                //var @new = GenerateFileEntry(directoryPath, fileName);

                //@new.Body = entry.Body;

                //Manager.EditFile(@new, old);                
                FileManager.EditFile(Site, directoryPath, virtualPath, name + fileExtension, body);

            }
            catch (Exception e)
            {
                result.AddException(e);
            }
            return Json(result);
        }
        #endregion

        /// <summary>
        /// for remote validation
        /// </summary>
        /// <param name="name"></param>
        /// <param name="old_Key"></param>
        /// <returns></returns>
        public virtual ActionResult IsNameAvailable(string directoryPath, string name, string fileExtension, string old_Key)
        {
            if (old_Key == null || !name.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                var fileEntry = FileManager.GetFile(Site, FileManager.GetRelativePath(directoryPath, name + fileExtension));

                if (fileEntry.Exists())
                {
                    return Json("The name already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Directory

        #region CreateDirectory
        public virtual ActionResult CreateDirectory(string directoryPath)
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult CreateDirectory(string directoryPath, string name)
        {
            FileManager.AddDirectory(Site, directoryPath, name);
            return RediretToIndex(new { directoryPath = directoryPath });
        }
        #endregion

        #region EditDirectory

        public virtual ActionResult EditDirectory(string directoryPath)
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult EditDirectory(string directoryPath, string name)
        {
            var entry = new JsonResultEntry();
            try
            {
                var dir = FileManager.RenameDirectory(Site, directoryPath, name);
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }


            return Json(entry);
        }

        #endregion

        #endregion


        #region Delete

        [HttpPost]
        public virtual ActionResult Delete(string directoryPath, IEnumerable<string> filePaths, IEnumerable<string> directoryPaths)
        {
            var entry = new JsonResultEntry();
            try
            {
                if (filePaths != null)
                {
                    foreach (var f in filePaths)
                    {
                        FileManager.DeleteFile(Site, f);
                    }
                }
                if (directoryPaths != null)
                {
                    foreach (var d in directoryPaths)
                    {
                        FileManager.DeleteDirectory(Site, d);
                    }
                }
                //entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.SetFailed().AddException(e);
            }
            return Json(entry);
        }

        #endregion

        public virtual ActionResult RediretToIndex()
        {
            return RediretToIndex(Request.RequestContext.AllRouteValues());
        }

        public virtual ActionResult RediretToIndex(object routeValues)
        {
            return RedirectToAction("Index", routeValues);
        }

        [HttpPost]
        public virtual ActionResult Sort(string directoryPath, IEnumerable<string> filesOrder)
        {
            FileManager.SaveFileOrders(Site, directoryPath, filesOrder.Select(it => it.Split(Path.DirectorySeparatorChar)).Select(it => it.Last()));
            return null;
        }

        [HttpPost]
        public virtual ActionResult Import(string directoryPath, bool @overrided)
        {
            var entry = new JsonResultEntry();
            try
            {
                var file = Request.Files[0];

                FileManager.Import(Site, directoryPath, file.InputStream, @overrided);

                entry.ReloadPage = true;
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }


    }
}
