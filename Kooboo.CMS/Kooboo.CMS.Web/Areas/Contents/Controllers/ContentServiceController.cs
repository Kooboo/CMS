using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Content.Models.Binder;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Web.Models;
using Kooboo.Globalization;
using Kooboo.CMS.Content.Services;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [ValidateInput(false)]
    public class ContentServiceController : SubmissionControllerBase
    {
        public class CategoryContent
        {
            public string FolderName { get; set; }
            public string UUID { get; set; }
        }
        public virtual ActionResult Create(string folderName, string schemaName, string parentFolder, string parentUUID)
        {
            folderName = Decrypt(folderName);
            schemaName = Decrypt(schemaName);
            parentFolder = Decrypt(parentFolder);

            TextFolder textFolder;
            Schema schema;
            GetSchemaAndFolder(folderName, schemaName, out textFolder, out schema);
            Exception exception = null;
            ContentBase content = null;
            try
            {
                if (textFolder != null)
                {
                    var categories = ContentPlugin.GetCategories("Categories", this.ControllerContext);
                    content = ServiceFactory.TextContentManager.Add(Repository.Current, textFolder,
                        parentFolder, parentUUID, HttpContext.Request.Form, HttpContext.Request.Files, categories, HttpContext.User.Identity.Name);
                }
                else
                {
                    content = CMS.Content.Services.ServiceFactory.TextContentManager.Add(Repository.Current, schema, parentUUID,
                 HttpContext.Request.Form, HttpContext.Request.Files, HttpContext.User.Identity.Name);
                }
            }
            catch (Exception e)
            {
                exception = e;
            }
            return ReturnActionResult(content, exception);
        }
        public virtual ActionResult Update(string folderName, string schemaName, string uuid)
        {
            folderName = Decrypt(folderName);
            schemaName = Decrypt(schemaName);

            TextFolder textFolder;
            Schema schema;
            GetSchemaAndFolder(folderName, schemaName, out textFolder, out schema);
            Exception exception = null;
            ContentBase content = null;
            try
            {
                if (textFolder != null)
                {
                    var addCategories = ContentPlugin.GetCategories("AddCategories", this.ControllerContext);
                    var removeCategories = ContentPlugin.GetCategories("RemoveCategories", this.ControllerContext);
                    content = CMS.Content.Services.ServiceFactory.TextContentManager.Update(Repository.Current, textFolder, uuid, HttpContext.Request.Form
                           , HttpContext.Request.Files, DateTime.UtcNow, addCategories, removeCategories, HttpContext.User.Identity.Name);

                }
                else
                {
                    content = CMS.Content.Services.ServiceFactory.TextContentManager.Update(Repository.Current, schema, uuid, HttpContext.Request.Form,
                        HttpContext.Request.Files, HttpContext.User.Identity.Name);
                }
            }
            catch (Exception e)
            {
                exception = e;
            }
            return ReturnActionResult(content, exception);
        }
        public virtual ActionResult Delete(string folderName, string schemaName, string uuid)
        {
            folderName = Decrypt(folderName);
            schemaName = Decrypt(schemaName);

            TextFolder textFolder;
            Schema schema;
            GetSchemaAndFolder(folderName, schemaName, out textFolder, out schema);
            Exception exception = null;
            try
            {
                if (textFolder != null)
                {
                    CMS.Content.Services.ServiceFactory.TextContentManager.Delete(Repository.Current, textFolder, uuid);

                }
                else
                {
                    CMS.Content.Services.ServiceFactory.TextContentManager.Delete(Repository.Current, schema, uuid);
                }
            }
            catch (Exception e)
            {
                exception = e;
            }
            return ReturnActionResult(null, exception);
        }

        private void GetSchemaAndFolder(string folderName, string schemaName, out TextFolder textFolder, out Schema schema)
        {
            if (string.IsNullOrEmpty(folderName) && string.IsNullOrEmpty(schemaName))
            {
                throw new KoobooException("The folderName or schemaName is required.".Localize());
            }
            textFolder = null;
            schema = null;
            if (!string.IsNullOrEmpty(folderName))
            {
                textFolder = (TextFolder)(FolderHelper.Parse<TextFolder>(Repository.Current, folderName).AsActual());
            }
            if (!string.IsNullOrEmpty(schemaName))
            {
                schema = new Schema(Repository.Current, schemaName).AsActual();
            }
            else
            {
                schema = new Schema(Repository.Current, textFolder.SchemaName).AsActual();
            }
        }

        private string Decrypt(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            return Kooboo.CMS.Sites.View.SecurityHelper.Decrypt(s);
        }
    }
}
