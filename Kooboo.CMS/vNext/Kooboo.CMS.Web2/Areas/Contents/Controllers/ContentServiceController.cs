#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Binder;
using Kooboo.CMS.Content.Services;

using Kooboo.CMS.Web.Models;
using Kooboo.Common.Globalization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [Obsolete("Please use new SubmssionSetting function for security reason.")]
    [ValidateInput(false)]
    public class ContentServiceController : SubmissionControllerBase
    {
        public class CategoryContent
        {
            public string FolderName { get; set; }
            public string UUID { get; set; }
        }
        [HttpPost]
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
                    var categories = ContentPlugin.GetCategories("Categories", this.ControllerContext, this.ControllerContext.HttpContext.Request.Form);
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
        [HttpPost]
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
                    var addCategories = ContentPlugin.GetCategories("AddCategories", this.ControllerContext, this.ControllerContext.HttpContext.Request.Form);
                    var removeCategories = ContentPlugin.GetCategories("RemoveCategories", this.ControllerContext, this.ControllerContext.HttpContext.Request.Form);
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
        [HttpPost]
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
                throw new Exception("The folderName or schemaName is required.".Localize());
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
            return Kooboo.CMS.Sites.View.SecurityHelper.Decrypt(s);
        }
    }
}
