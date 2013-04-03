using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Web.Areas.Contents.Models;
using Kooboo.CMS.Web.Authorizations;
using Kooboo.CMS.Web.Models;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using System.IO;
using System.Drawing.Imaging;
using Kooboo.Drawing;
using Kooboo.IO;
using System.Drawing;
using Kooboo.Extensions;
using Kooboo.CMS.Content.Query.Expressions;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [LargeFileAuthorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
    public class MediaContentController : ContentControllerBase
    {
        #region Properties
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

        #endregion

        #region Index
        // Kooboo.CMS.Account.Models.Permission.Contents_ContentPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 99)]
        public virtual ActionResult Index(string folderName, string search, int? page, int? pageSize)
        {
            return MediaContentGrid(folderName, search, page, pageSize);
        }
        #endregion

        #region Create
        // Kooboo.CMS.Account.Models.Permission.Contents_ContentPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult Create(string folder)
        {
            return View();
        }
        // Kooboo.CMS.Account.Models.Permission.Contents_ContentPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpPost]
        public virtual ActionResult Create(string folderName, FormCollection form)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                HttpFileCollectionBase files = Request.Files;
                MediaContent mediaContent = SaveFile(folderName, files);
                if (mediaContent.IsImage)
                {
                    resultEntry.RedirectUrl = ControllerContext.RequestContext.UrlHelper().Action("Edit",
                        ControllerContext.RequestContext.AllRouteValues().Merge("uuid", mediaContent.UUID));
                }
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry);

            //return View();
        }

        private MediaContent SaveFile(string folderName, HttpFileCollectionBase files)
        {
            if (files != null)
            {
                var @overrided = string.Equals(Request.Form["Overrided"], "true", StringComparison.OrdinalIgnoreCase);
                var currentFolder = FolderManager.Get(Repository, folderName);
                foreach (var f in files.AllKeys)
                {
                    var file = files[f];
                    if (file.ContentLength > 0)
                    {
                        //if upload from ie filename will be fullpath include disk symbol 
                        var fileName = file.FileName.Contains("\\") ? file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1) : file.FileName;

                        return ContentManager.Add(Repository, currentFolder, fileName, file.InputStream, overrided, User.Identity.Name);
                    }
                }
            }
            return null;
        }

        public JsonResult ValidFileExisted(IEnumerable<string> files, string folderName)
        {
            List<object> result = new List<object>();
            foreach (var f in files)
            {
                var content = new MediaContent()
                {
                    Repository = Repository.Name,
                    FileName = f,
                    FolderName = folderName
                };
                result.Add(new
                {
                    Item = f,
                    Exist = content.Exist(),
                    Data = content
                });
            }
            return Json(result);
        }
        #endregion

        #region Delete
        // Kooboo.CMS.Account.Models.Permission.Contents_ContentPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult Delete(string folderName, string uuid)
        {
            var folder = FolderManager.Get(Repository, folderName);
            var folderPath = FolderHelper.SplitFullName(folderName);
            ContentManager.Delete(Repository, folder, uuid);

            return RedirectToAction("Index", new { folderName = folderName });
        }
        // Kooboo.CMS.Account.Models.Permission.Contents_ContentPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpPost]
        public virtual ActionResult DeleteSelect(string folderName, string[] folders, string[] files)
        {

            var entry = new JsonResultEntry();

            try
            {
                if (string.IsNullOrWhiteSpace(folderName))
                {
                    if (folders != null)
                        foreach (var f in folders)
                        {
                            if (string.IsNullOrEmpty(f)) continue;
                            var folderPath = FolderHelper.SplitFullName(f);
                            FolderManager.Remove(Repository, new MediaFolder(Repository, folderPath));
                        }
                }
                else
                {
                    var folder = FolderManager.Get(Repository, folderName).AsActual();
                    if (folders != null)
                        foreach (var f in folders)
                        {
                            if (string.IsNullOrEmpty(f)) continue;
                            var folderPath = FolderHelper.SplitFullName(f);
                            FolderManager.Remove(Repository, new MediaFolder(Repository, folderPath));
                        }

                    if (files != null)
                        foreach (var f in files)
                        {
                            if (string.IsNullOrEmpty(f)) continue;
                            ContentManager.Delete(Repository, folder, f);
                        }
                }

                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.SetFailed().AddException(e);
            }

            return Json(entry);

            //return RedirectToAction("Index", new { folderName = folderName });
        }
        #endregion

        #region Publish
        [HttpPost]
        public virtual ActionResult Publish(string uuid, string folderName, bool published)
        {
            var folder = FolderManager.Get(Repository, folderName);
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                ContentManager.Update(folder, uuid, new string[] { "Published" }, new object[] { published });
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }
        #endregion

        #region  selection

        public virtual ActionResult Selection(string folderName, string search, int? page, int? pageSize)
        {

            return MediaContentGrid(folderName, search, page, pageSize ?? 20);
        }

        #endregion

        #region Import
        public virtual ActionResult Import(string folderName, bool @overrided)
        {
            var entry = new JsonResultEntry();
            try
            {
                FolderManager.Import(Repository, Request.Files[0].InputStream, folderName, @overrided);
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }
        #endregion

        #region MediaContentGrid

        private ActionResult MediaContentGrid(string folderName, string search, int? page, int? pageSize)
        {
            if (string.IsNullOrWhiteSpace(folderName))
            {
                var folders = FolderManager.All(Repository, search, "");
                return View(new MediaContentGrid
                {
                    ChildFolders = folders
                });
            }
            else
            {
                IEnumerable<MediaFolder> childFolders = new MediaFolder[0];
                if (!page.HasValue || page.Value <= 1)
                {
                    childFolders = FolderManager.All(Repository, search, folderName);
                }

                var currentFolder = FolderManager.Get(Repository, folderName);
                var contentQuery = currentFolder.CreateQuery();
                if (!string.IsNullOrEmpty(search))
                {

                    IWhereExpression expression = new WhereContainsExpression(null, "FileName", search);
                    expression = new OrElseExpression(expression, new WhereContainsExpression(null, "Metadata.AlternateText", search));
                    expression = new OrElseExpression(expression, new WhereContainsExpression(null, "Metadata.Description", search));
                    contentQuery = contentQuery
                        .Where(expression);

                }

                return View(new MediaContentGrid
                {
                    ChildFolders = childFolders,
                    Contents = contentQuery.ToPageList(page ?? 0, pageSize ?? 50)
                });
            }
        }

        #endregion

        #region Edit Image

        public virtual ActionResult ImageDetailInfo(string folderName, string fileName)
        {
            var folder = FolderHelper.Parse<MediaFolder>(Repository, folderName);

            FolderPath path = new FolderPath(folder);

            var fullName = System.IO.Path.Combine(new string[] { path.PhysicalPath, fileName });

            object jsonData = new { Success = false };
            try
            {
                using (var img = System.Drawing.Image.FromFile(fullName))
                {
                    jsonData = new
                    {
                        Success = true,
                        FileName = System.IO.Path.GetFileName(fullName),
                        //FileUrl = url,
                        Width = img.Width,
                        Height = img.Height
                    };
                }
            }
            catch { }

            // ret
            return Json(jsonData);
        }

        public virtual ActionResult Edit(string folderName, string uuid)
        {
            var mediaFolder = FolderHelper.Parse<MediaFolder>(Repository, folderName);
            var mediaContent = mediaFolder.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();
            return View(mediaContent);
        }

        [HttpPost]
        public virtual ActionResult EditMetadata(string folderName, string uuid, [System.Web.Mvc.Bind(Prefix = "Metadata")] MediaContentMetadata metadata, string returnUrl)
        {
            var mediaFolder = FolderHelper.Parse<MediaFolder>(Repository, folderName);
            var mediaContent = mediaFolder.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                ContentManager.Update(Repository, mediaFolder, uuid, mediaContent.FileName, null, User.Identity.Name, metadata);
                entry.RedirectUrl = returnUrl;
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }

        public ActionResult Preview(string imagePath, string rotateTypes)
        {
            var physicalPath = Server.MapPath(HttpUtility.UrlDecode(imagePath));
            var imageFormat = ImageTools.ConvertToImageFormat(Path.GetExtension(physicalPath));
            Stream imageStream = new MemoryStream();
            Stream outputStream = new MemoryStream();
            try
            {
                imageStream = RotateImage(rotateTypes, physicalPath, imageFormat);

                ImageTools.ResizeImage(imageStream, outputStream, imageFormat, 0, 200, true, 80);
                outputStream.Position = 0;
                return File(outputStream, IOUtility.MimeType(physicalPath));
            }
            finally
            {
                if (imageStream != null)
                {
                    imageStream.Close();
                    imageStream.Dispose();
                }
            }
        }

        private static Stream RotateImage(string rotateTypes, string physicalPath, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            Stream imageStream = new MemoryStream();
            if (!string.IsNullOrEmpty(rotateTypes))
            {
                using (Image image = Image.FromFile(physicalPath))
                {
                    var rotates = rotateTypes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var rotateType in rotates)
                    {
                        switch (rotateType)
                        {
                            case "1": //逆时针旋转90
                                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                break;
                            case "2"://顺时针旋转90
                                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                break;
                            case "3"://垂直翻转
                                image.RotateFlip(RotateFlipType.Rotate180FlipX);
                                break;
                            case "4"://水平翻转
                                image.RotateFlip(RotateFlipType.Rotate180FlipY);
                                break;
                            default:
                                break;
                        }
                    }
                    image.Save(imageStream, imageFormat);
                }
            }
            else
            {
                using (var fs = new FileStream(physicalPath, FileMode.Open, FileAccess.Read))
                {
                    fs.CopyTo(imageStream);
                }
            }
            imageStream.Position = 0;
            return imageStream;
        }

        [HttpPost]
        public virtual ActionResult EditImage(string folderName, string uuid, [System.Web.Mvc.Bind(Prefix = "Crop")] ImageCropModel cropModel,
           [System.Web.Mvc.Bind(Prefix = "Scale")]  ImageScaleModel scaleModel, string rotateTypes)
        {
            var mediaFolder = FolderHelper.Parse<MediaFolder>(Repository.Current, folderName);
            var mediaContent = mediaFolder.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();
            JsonResultEntry entry = new JsonResultEntry();
            Stream targetStream = new MemoryStream();
            MemoryStream resizedImage = new MemoryStream();
            Stream imageStream = new MemoryStream();
            int width = 0, height = 0;
            try
            {
                ImageFormat imageFormat = ImageTools.ConvertToImageFormat(Path.GetExtension(mediaContent.PhysicalPath));
                imageStream = RotateImage(rotateTypes, mediaContent.PhysicalPath, imageFormat);

                if (cropModel.X.HasValue)
                {
                    ImageTools.CropImage(imageStream, targetStream, imageFormat, cropModel.X.Value, cropModel.Y.Value, cropModel.Width.Value, cropModel.Height.Value);
                    targetStream.Position = 0;
                }
                else
                {
                    targetStream = imageStream;
                }
                if (scaleModel.Height.HasValue && scaleModel.Width.HasValue)
                {
                    ImageTools.ResizeImage(targetStream, resizedImage, imageFormat, scaleModel.Width.Value, scaleModel.Height.Value, scaleModel.PreserveAspectRatio, scaleModel.Quality ?? 80);
                    resizedImage.Position = 0;
                    targetStream = resizedImage;
                }

                Image image = Image.FromStream(targetStream);
                width = image.Width;
                height = image.Height;
                targetStream.Position = 0;

                ServiceFactory.MediaContentManager.Update(Repository.Current, mediaFolder, uuid, mediaContent.FileName, targetStream, User.Identity.Name);

                entry.AddMessage("The image has been changed.".Localize());
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            finally
            {
                if (targetStream != null)
                {
                    targetStream.Close();
                    targetStream.Dispose();
                    targetStream = null;
                }
                if (resizedImage != null)
                {
                    resizedImage.Close();
                    resizedImage.Dispose();
                    resizedImage = null;
                }
                if (imageStream != null)
                {
                    imageStream.Close();
                    imageStream.Dispose();
                    imageStream = null;
                }
            }
            entry.Model = new { Width = width, Height = height };
            return Json(entry);
        }
        #endregion

        #region Large File
        public virtual ActionResult LargeFile()
        {
            return View();
        }

        [HttpPost]
        [LargeFileAuthorization(Order = 1)]
        public virtual ActionResult LargeFile(FormCollection form, string folderName)
        {
            var f = folderName.Substring(folderName.LastIndexOf('/') + 1);
            SaveFile(f, Request.Files);
            return View();
        }

        [HttpPost]
        public virtual ActionResult CheckLargeFile()
        {
            return Json(0);
        }
        #endregion

        #region TextFile
        public virtual ActionResult TextFile(string folderName, string fileName)
        {
            MediaContent content = new MediaContent(Repository.Name, folderName);
            content.FileName = fileName;

            var contentPath = new MediaContentPath(content);

            string body = Kooboo.IO.IOUtility.ReadAsString(contentPath.PhysicalPath);

            return View(new TextFileModel
            {
                Title = fileName,
                Body = body
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult TextFile(string folderName, string fileName, string body)
        {
            var entry = new JsonResultEntry();

            try
            {
                MediaContent content = new MediaContent(Repository.Name, folderName);
                content.FileName = fileName;

                var contentPath = new MediaContentPath(content);

                Kooboo.IO.IOUtility.SaveStringToFile(contentPath.PhysicalPath, body);
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }
        #endregion

        #region Rename & Move
        /// <summary>
        /// Renames the specified folder name.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="uuid">The UUID.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public virtual ActionResult RenameFile(string folderName, string uuid, string name)
        {
            JsonResultEntry entry = new JsonResultEntry();

            try
            {
                ServiceFactory.MediaContentManager.Move(Repository, folderName, uuid, folderName, name);
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }
        [HttpGet]
        public virtual ActionResult MoveFile(string folderName, string uuid)
        {
            return View(new MoveMediaContent());
        }
        [HttpPost]
        public virtual ActionResult MoveFile(string folderName, string uuid, string targetFolder)
        {
            JsonResultEntry entry = new JsonResultEntry();

            try
            {
                ServiceFactory.MediaContentManager.Move(Repository, folderName, uuid, targetFolder, uuid);
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }

        public virtual ActionResult TargetFolderAvailable(string folderName, string targetFolder)
        {
            if (folderName.EqualsOrNullEmpty(targetFolder, StringComparison.OrdinalIgnoreCase))
            {
                return Json("The target folder is the same as the source folder.".Localize());
            }
            return Json(true);
        }
        #endregion

        #region Html5uploader
        public ActionResult Html5Uploader()
        {
            return View();
        }
        #endregion
    }

    public class TextFileModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }


}
