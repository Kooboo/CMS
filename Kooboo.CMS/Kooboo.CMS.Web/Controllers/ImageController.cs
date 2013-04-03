using Kooboo.CMS.Sites;
using Kooboo.Drawing;
using Kooboo.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Controllers
{
    public class ImageController : Controller
    {
        private static object resizeImageLocker = new object();
        public ActionResult Resize(string url, int width, int height, bool? preserverAspectRatio, int? quality)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(url);
            }
            url = HttpUtility.UrlDecode(url);
            var index = url.IndexOf("?");
            if (index != -1)
            {
                url = url.Substring(0, index);
            }
            var imageFullPath = Server.MapPath(url);
            preserverAspectRatio = preserverAspectRatio ?? true;
            quality = quality ?? 80;
            //var cachingPath = GetCachingFilePath(imageFullPath, width, height, preserverAspectRatio.Value, quality.Value);
            var imageFormat = ImageTools.ConvertToImageFormat(Path.GetExtension(imageFullPath));
            MemoryStream ms = new MemoryStream();
            using (FileStream fs = new FileStream(imageFullPath, FileMode.Open, FileAccess.Read))
            {
                ImageTools.ResizeImage(fs, ms, imageFormat, width, height, preserverAspectRatio.Value, quality.Value);
                ms.Position = 0;
            }

            return File(ms, IOUtility.MimeType(imageFullPath));
        }
        private string GetCachingFilePath(string imagePath, int width, int height, bool preserverAspectRatio, int quality)
        {
            var lastModeifyDate = System.IO.File.GetLastWriteTimeUtc(imagePath);
            string cms_dataPath = Path.Combine(Kooboo.Settings.BaseDirectory, PathEx.BasePath);
            string fileName = Path.GetFileNameWithoutExtension(imagePath);
            string newFileName = fileName + "-" + width.ToString() + "-" + height.ToString() + "-" + preserverAspectRatio.ToString() + "-" + quality.ToString() + "-" + lastModeifyDate.Ticks;
            string imageCachingPath = Path.Combine(Kooboo.Settings.BaseDirectory, PathEx.BasePath, "ImageCaching");
            string cachingPath = imageCachingPath + imagePath.Substring(cms_dataPath.Length);
            return Path.Combine(Path.GetDirectoryName(cachingPath), newFileName + Path.GetExtension(imagePath));
        }
    }
}
