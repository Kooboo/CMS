using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;

using Kooboo.Web;
using Kooboo.Web.Script.Serialization;
using Kooboo.CMS.Web.Areas.Sites.Models;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
     [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Development", Name = "File", Order = 1)]
    public class CustomFileController : AdminControllerBase
    {
        //
        // GET: /Admin/Images/
        private CustomFileManager Manager
        {
            get { return new CustomFileManager(); }
        }


        public virtual ActionResult Index(string search, string fullName)
        {

            CustomDirectory di = new CustomDirectory(Site, fullName ?? "");
            var data = Manager.All(di);

            return View(data);
        }

        public virtual ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Create(CustomFile model, string fullName)
        {
            Msg msg = new Msg();
            try
            {
                var userFile = Request.Files["image"];
                if (userFile.InputStream.Length == 0)
                {
                    throw new FriendlyException("Please select a file!");
                }
                Manager.SaveFile(this.Site, fullName, userFile.FileName, userFile.InputStream);
                return RedirectToAction("Index", new { fullName = fullName });
            }
            catch (FriendlyException e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }


        public virtual ActionResult Delete(string fullName, string fileName, string fileType)
        {
            try
            {
                Manager.Delete(Site, fullName, fileName);
                if (!string.IsNullOrEmpty(fullName))
                {
                    var arr = CustomDirectoryHelper.SplitFullName(fullName);
                    var aCount = arr.Count();
                    fullName = CustomDirectoryHelper.CombineFullName(arr.Take(aCount - 1));
                }

            }
            catch (FriendlyException e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return RedirectToAction("Index", new { fullName = fullName });
        }

        [HttpPost]
        public string CreateDirectory(string fullName)
        {
            Msg msg = new Msg();
            try
            {
                string folderName = Request.Form["folderName"];
                var nameArr = !string.IsNullOrEmpty(fullName) ? CustomDirectoryHelper.SplitFullName(fullName) : new string[] { };
                CustomDirectory di = !string.IsNullOrEmpty(fullName) ? new CustomDirectory(this.Site, nameArr) : null;
                Manager.CreateDirectory(this.Site, di, folderName);
                msg.Success = true;
            }
            catch (Exception e)
            {
                msg.Success = false;
                msg.ErrMsg = e.Message;
            }
            return msg.ToJSON();
        }

        [HttpPost]
        public virtual ActionResult Import(string fullName, bool @override)
        {
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                CustomDirectory dir = new CustomDirectory(Site, fullName ?? "");
                var file = Request.Files["file"];
                Manager.Import(Site, dir, file.InputStream, @override);
            }
            return RedirectToAction("Index", new { fullName = fullName });
        }

        public virtual ActionResult Export(string fullName)
        {
            CustomDirectory dir = new CustomDirectory(Site, fullName ?? "");
            var fileName = GetZipFileName(fullName ?? "");
            Response.AttachmentHeader(fileName);
            Manager.Export(dir, Response.OutputStream);
            return null;
        }

        private string GetZipFileName(string fullName)
        {
            string name = "Files";
            name += string.IsNullOrEmpty(fullName) ? "" : "_" + string.Join("_", CustomDirectoryHelper.SplitFullName(fullName));
            name = string.Join(".", name, "zip");
            return name;
        }
    }
}
