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
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Web.Areas.Contents.Models;
using Kooboo.CMS.Web.Authorizations;
using Kooboo.CMS.Web.Models;
using Kooboo.Drawing;
using Kooboo.Drawing.Filters;
using Kooboo.Globalization;
using Kooboo.IO;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web;
using Kooboo.CMS.Content.Persistence;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [LargeFileAuthorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
    public class MediaContentController : ContentControllerBase
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
        public MediaContentController(MediaContentManager mediaContentManager, MediaFolderManager mediaFolderManager)
        {
            this.ContentManager = mediaContentManager;
            this.FolderManager = mediaFolderManager;
        }
        #endregion

        #region Index

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 99)]
        public virtual ActionResult Index(string folderName, string search, int? page, int? pageSize, string sortField, string sortDir, string listType)
        {
            return MediaContentGrid(folderName, search, page, pageSize, sortField, sortDir, listType);
        }
        #endregion

        #region Create
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpPost]
        public virtual ActionResult Create(string folderName, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                HttpFileCollectionBase files = Request.Files;
                MediaContent mediaContent = SaveFile(folderName, files);
                if (mediaContent.IsImage)
                {
                    data.RedirectUrl = ControllerContext.RequestContext.UrlHelper().Action("EditInDialog",
                        ControllerContext.RequestContext.AllRouteValues().Merge("return", null).Merge("uuid", mediaContent.UUID));
                }
            });
            return Json(data);
        }
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpPost]
        public virtual ActionResult Upload(string folderName, string @return)
        {
            HttpFileCollectionBase files = Request.Files;
            MediaContent mediaContent = SaveFile(folderName, files);
            return Redirect(@return);
        }

        private MediaContent SaveFile(string folderName, HttpFileCollectionBase files)
        {
            if (files != null)
            {
                var @overrided = true;
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
        [HttpPost]
        public virtual ActionResult Delete(string folderName, string[] folders, string[] docs)
        {

            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
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

                        if (docs != null)
                            foreach (var f in docs)
                            {
                                if (string.IsNullOrEmpty(f)) continue;
                                ContentManager.Delete(Repository, folder, f);
                            }
                    }
                    resultData.ReloadPage = true;
                });
            return Json(data);

            //return RedirectToAction("Index", new { folderName = folderName });
        }
        #endregion

        #region Publish
        [HttpPost]
        public virtual ActionResult Publish(string uuid, string folderName, bool published)
        {
            var folder = FolderManager.Get(Repository, folderName);
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                ContentManager.Update(folder, uuid, new string[] { "Published" }, new object[] { published });
            });

            return Json(data);
        }
        #endregion

        #region  Selection

        public virtual ActionResult Selection(string folderName, string search, int? page, int? pageSize, string sortField, string sortDir, string listType)
        {
            return MediaContentGrid(folderName, search, page, pageSize ?? 50, sortField, sortDir, listType);
        }

        #endregion

        #region Import
        public virtual ActionResult Import(string folderName)
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Import(string folderName, bool @override, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                FolderManager.Import(Repository, Request.Files[0].InputStream, folderName, @override);
                data.RedirectUrl = @return;
            });
            return Json(data);
        }
        #endregion

        #region Export

        [HttpPost]
        public virtual void Export(string directoryPath, string folderName, string[] folders, string[] docs)
        {
            var fileName = "Medialibrary.zip";
            Response.AttachmentHeader(fileName);

            ((IMediaFolderProvider)this.FolderManager.Provider).Export(Repository, folderName, folders, docs, Response.OutputStream);
        }

        #endregion

        #region MediaContentGrid

        private ActionResult MediaContentGrid(string folderName, string search, int? page, int? pageSize, string sortField, string sortDir, string listType)
        {
            var viewName = ControllerContext.RequestContext.GetRequestValue("action");
            if (!string.IsNullOrEmpty(listType))
            {
                viewName = viewName + "_" + listType;
            }
            if (string.IsNullOrWhiteSpace(folderName))
            {
                var folders = FolderManager.All(Repository, search, "");
                return View(viewName, new MediaContentGrid
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

                contentQuery = contentQuery.SortBy(sortField, sortDir);

                return View(viewName, new MediaContentGrid
                {
                    ChildFolders = childFolders,
                    Contents = contentQuery.ToPageList(page ?? 0, pageSize ?? 50)
                });
            }
        }

        #endregion

        #region Edit Image
        public virtual ActionResult EditInDialog(string folderName, string uuid)
        {
            return Edit(folderName, uuid);
        }
        public virtual ActionResult ImageDetailInfo(string fileUrl)
        {
            var filePath = Server.MapPath(System.Web.HttpUtility.UrlDecode(fileUrl));

            JsonResultData jsonData = new JsonResultData();
            try
            {
                using (var img = System.Drawing.Image.FromFile(filePath))
                {
                    jsonData.Model = new
                    {
                        Width = img.Width,
                        Height = img.Height
                    };
                }
            }
            catch (Exception e)
            {
                jsonData.AddException(e);
            }

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
        public virtual ActionResult Edit(string folderName, string uuid, [System.Web.Mvc.Bind(Prefix = "Metadata")] MediaContentMetadata metadata, string @return)
        {
            var mediaFolder = FolderHelper.Parse<MediaFolder>(Repository, folderName);
            var mediaContent = mediaFolder.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();
            JsonResultData data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                this.ContentManager.Update(Repository, mediaFolder, uuid, mediaContent.FileName, null, User.Identity.Name, metadata);
                resultData.ClosePopup = true;
                resultData.RedirectUrl = @return;
            });
            return Json(data);
        }

        public ActionResult Preview(string folderName, string uuid, string rotateTypes)
        {
            //var physicalPath = Server.MapPath(HttpUtility.UrlDecode(imagePath));
            var mediaFolder = FolderHelper.Parse<MediaFolder>(Repository, folderName);
            var mediaContent = mediaFolder.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();

            var provider = Kooboo.CMS.Content.Persistence.Providers.DefaultProviderFactory.GetProvider<IMediaContentProvider>();
            var data = provider.GetContentStream(mediaContent);
            var imageFormat = ImageTools.ConvertToImageFormat(Path.GetExtension(mediaContent.VirtualPath));

            Stream imageStream = new MemoryStream();
            Stream outputStream = new MemoryStream();
            try
            {
                using (var rawImage = new MemoryStream(data))
                {
                    imageStream = RotateImage(rotateTypes, rawImage, imageFormat);

                    ImageTools.ResizeImage(imageStream, outputStream, imageFormat, 600, 0, true, 80);
                    outputStream.Position = 0;

                    return File(outputStream, IOUtility.MimeType(mediaContent.VirtualPath));
                }

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

        private static Stream RotateImage(string rotateTypes, Stream inputStream, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            Stream imageStream = new MemoryStream();
            if (!string.IsNullOrEmpty(rotateTypes))
            {

                using (Image image = Image.FromStream(inputStream))
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
                    image.Dispose();
                }
            }
            else
            {
                inputStream.Position = 0;
                inputStream.CopyTo(imageStream);
            }
            imageStream.Position = 0;
            return imageStream;
        }

        [HttpPost]
        public virtual ActionResult EditImage(string folderName, string uuid, [System.Web.Mvc.Bind(Prefix = "Crop")] ImageCropModel cropModel,
           [System.Web.Mvc.Bind(Prefix = "Scale")]  ImageScaleModel scaleModel, string rotateTypes, string @return)
        {
            var mediaFolder = FolderHelper.Parse<MediaFolder>(Repository.Current, folderName);
            var mediaContent = mediaFolder.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();
            var provider = Kooboo.CMS.Content.Persistence.Providers.DefaultProviderFactory.GetProvider<IMediaContentProvider>();
            JsonResultData data = new JsonResultData(ModelState);

            Stream targetStream = new MemoryStream();
            MemoryStream resizedImage = new MemoryStream();
            Stream imageStream = new MemoryStream();
            int width = 0, height = 0;
            try
            {
                using (var contentStream = new MemoryStream(provider.GetContentStream(mediaContent)))
                {
                    ImageFormat imageFormat = ImageTools.ConvertToImageFormat(Path.GetExtension(mediaContent.VirtualPath));
                    imageStream = RotateImage(rotateTypes, contentStream, imageFormat);

                    if (cropModel != null && cropModel.X.HasValue && cropModel.Width.Value > 0 && cropModel.Height.Value > 0)
                    {
                        ImageTools.CropImage(imageStream, targetStream, imageFormat, cropModel.X.Value, cropModel.Y.Value, cropModel.Width.Value, cropModel.Height.Value);
                        targetStream.Position = 0;
                    }
                    else
                    {
                        targetStream = imageStream;
                    }
                    if (scaleModel != null && scaleModel.Height.HasValue && scaleModel.Width.HasValue && scaleModel.Height.Value > 0 && scaleModel.Width.Value > 0)
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

                    image.Dispose();
                    targetStream.Close();
                }
                //data.ClosePopup = true;
                //data.AddMessage("The image has been changed.".Localize());
            }
            catch (Exception e)
            {
                data.AddException(e);
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
            data.Model = new { Width = width, Height = height };
            data.RedirectToOpener = false;
            data.ReloadPage = true;
            return Json(data);
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
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                MediaContent content = new MediaContent(Repository.Name, folderName);
                content.FileName = fileName;

                var contentPath = new MediaContentPath(content);

                Kooboo.IO.IOUtility.SaveStringToFile(contentPath.PhysicalPath, body);
            });

            return Json(data);
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
        public virtual ActionResult RenameFile(string folderName, string uuid, string name, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                ServiceFactory.MediaContentManager.Move(Repository, folderName, uuid, folderName, name);
                resultData.RedirectUrl = @return;
            });

            return Json(data);
        }
        [HttpGet]
        public virtual ActionResult MoveFile(string folderName, string uuid)
        {
            return View(new MoveMediaContent());
        }
        [HttpPost]
        public virtual ActionResult MoveFile(string folderName, string uuid, string targetFolder)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                ServiceFactory.MediaContentManager.Move(Repository, folderName, uuid, targetFolder, uuid);
            });

            return Json(data);
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

        #region Create folder
        [HttpPost]
        public virtual ActionResult CreateFolder(MediaFolder model, string folderName, string @return)
        {
            var data = new JsonResultData();
            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    MediaFolder parent = null;
                    if (!string.IsNullOrEmpty(folderName))
                    {
                        parent = FolderHelper.Parse<MediaFolder>(Repository, folderName).AsActual();
                    }
                    model.Parent = parent;

                    FolderManager.Add(Repository, model);

                    resultData.RedirectToOpener = false;
                    resultData.RedirectUrl = @return;
                }
            });

            return Json(data);
        }
        #endregion

        #region Rename folder
        [HttpPost]
        public virtual ActionResult RenameFolder(string name, string folderName, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                var oldFolder = new MediaFolder(Repository, folderName);
                var @new = new MediaFolder(Repository, folderName);
                @new.Name = name;

                if (oldFolder != @new)
                {
                    FolderManager.Rename(@new, oldFolder);
                }
                resultData.RedirectUrl = @return;
            });

            return Json(data);
        }
        #endregion

        #region IsFolderNameAvailable
        public virtual ActionResult IsFolderNameAvailable(string name, string folderName, string old)
        {
            if (!string.IsNullOrEmpty(name) && !name.EqualsOrNullEmpty(old, StringComparison.OrdinalIgnoreCase))
            {
                string fullName = name;
                if (!string.IsNullOrEmpty(folderName))
                {
                    fullName = FolderHelper.CombineFullName(new[] { folderName, name });
                }
                var folder = new MediaFolder(Repository, fullName);
                if (folder.AsActual() != null)
                {
                    return Json("The name already exists.".Localize(), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region IsFileNameAvailable
        public virtual ActionResult IsFileNameAvailable(string name, string folderName, string uuid)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var folder = new MediaFolder(Repository, folderName);
                var exists = folder.CreateQuery().WhereEquals("FileName", name).Count() != 0;
                if (exists)
                {
                    return Json("The name already exists.".Localize(), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Watermark

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpPost]
        public virtual ActionResult Watermark( string folderName, string[] docs )
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry(( resultData ) =>
            {
                if( !string.IsNullOrWhiteSpace(folderName) && docs != null )
                {
                    var folder = FolderManager.Get(Repository, folderName).AsActual();
                    var uuids = from uuid in docs where !string.IsNullOrEmpty(uuid) select uuid;
                    foreach( var uuid in uuids )
                    {
                        var content = folder.CreateQuery().WhereEquals("UUID", uuid).First();
                        var provider = Providers.DefaultProviderFactory.GetProvider<IMediaContentProvider>();
                        using( var sourceStream = new MemoryStream(provider.GetContentStream(content)) )
                        {
                            var imageFormat = GetImageFormat(Path.GetExtension(content.FileName));
                            if( imageFormat == null )
                            {
                                continue;
                            }
                            var stream = Watermark(Repository, sourceStream, imageFormat);
                            ContentManager.Update(Repository, folder, uuid, content.FileName, stream, User.Identity.Name);
                        }
                    }
                }
                resultData.ReloadPage = true;
            });
            return Json(data);
        }

        public static ImageFormat GetImageFormat( string extension )
        {
            switch( extension.ToLower() )
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".gif":
                    return ImageFormat.Gif;
                case ".png":
                    return ImageFormat.Png;
                case ".bmp":
                    return ImageFormat.Bmp;
            }
            return null;
        }
        private Stream Watermark( Repository repository, Stream stream, ImageFormat imageFormat )
        {
            var waterImageFile = GetWaterImage(repository);
            if (System.IO.File.Exists(waterImageFile))
            {
                var waterImage = Image.FromFile(waterImageFile);

                var rawImage = Image.FromStream(stream);

                var filter = new ImageWatermarkFilter
                {
                    WaterMarkImage = waterImage,
                    Alpha = 0.7f,
                    Valign = WaterMarkFilter.VAlign.Right,
                    Halign = WaterMarkFilter.HAlign.Bottom
                };
                var image = filter.ExecuteFilter(rawImage);

                var ms = new MemoryStream();
                image.Save(ms, imageFormat);
                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
            return stream;
        }
        private string GetWaterImage( Repository repository )
        {
            var path = new RepositoryPath(repository);

            return Path.Combine(path.PhysicalPath, "Watermark.png");
        }
        #endregion
    }

    public class TextFileModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }


}
