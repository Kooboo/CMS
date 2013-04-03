using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.Web.Url;
using Kooboo.CMS.Web.Areas.Contents.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models.Binder;
using Kooboo.Web.Mvc.Paging;
using Kooboo.CMS.Content.Query.Expressions;
using System.Collections.Specialized;
using Kooboo.Extensions;


using Kooboo.Web.Mvc;
using Kooboo.Web.Script.Serialization;
using Kooboo.CMS.Content.Versioning;
using DiffPlex;
using DiffPlex.DiffBuilder;
using System.Text;
using Kooboo.CMS.Web.Models;
using System.Diagnostics;
using Kooboo.CMS.Sites;
using System.IO;

using Kooboo.Globalization;
using Kooboo.CMS.Sites.DataRule;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{

    [ValidateInput(false)]
    public class TextContentController : ContentControllerBase
    {
        private class CategoryValue
        {
            public string CategoryFolderName { get; set; }
            public string Value { get; set; }
            public string OldValue { get; set; }
        }
        public class ContentSorter
        {
            public string UUID { get; set; }
            public int Sequence { get; set; }
        }

        #region Grid

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult Index(string folderName, string parentUUID, string parentFolder, string search
            , IEnumerable<WhereClause> whereClause, int? page, int? pageSize, string orderField = null, string direction = null)
        {
            //compatible with the Folder parameter changed to FolderName.
            folderName = folderName ?? this.ControllerContext.RequestContext.GetRequestValue("Folder");

            TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
            var schema = textFolder.GetSchema().AsActual();

            SchemaPath schemaPath = new SchemaPath(schema);
            ViewData["Folder"] = textFolder;
            ViewData["Schema"] = schema;
            ViewData["Template"] = textFolder.GetFormTemplate(FormType.Grid);
            ViewData["WhereClause"] = whereClause;

            SetPermissionData(textFolder);

            IEnumerable<TextFolder> childFolders = new TextFolder[0];
            //Skip the child folders on the embedded folder grid.
            if (string.IsNullOrEmpty(parentFolder))
            {
                if (!page.HasValue || page.Value <= 1)
                {
                    childFolders = ServiceFactory.TextFolderManager.ChildFolders(textFolder, search).Select(it => it.AsActual());
                }
            }

            IContentQuery<TextContent> query = textFolder.CreateQuery();
            if (string.IsNullOrEmpty(orderField))
            {
                query = query.DefaultOrder();
            }
            else
            {
                if (!string.IsNullOrEmpty(direction) && direction.ToLower() == "desc")
                {
                    query = query.OrderByDescending(orderField);
                }
                else
                {
                    query = query.OrderBy(orderField);
                }
            }
            bool showTreeStyle = schema.IsTreeStyle;
            //如果有带搜索条件，则不输出树形结构
            if (!string.IsNullOrEmpty(search))
            {
                IWhereExpression exp = new FalseExpression();
                foreach (var item in schema.Columns.Where(it => it.ShowInGrid))
                {
                    exp = new OrElseExpression(exp, (new WhereContainsExpression(null, item.Name, search)));
                }
                if (exp != null)
                {
                    query = query.Where(exp);
                }
                showTreeStyle = false;
            }
            if (whereClause != null && whereClause.Count() > 0)
            {
                var expression = WhereClauseToContentQueryHelper.Parse(whereClause, schema, new MVCValueProviderWrapper(ValueProvider));
                query = query.Where(expression);
                showTreeStyle = false;
            }
            if (!string.IsNullOrWhiteSpace(parentUUID))
            {
                query = query.WhereEquals("ParentUUID", parentUUID);
            }
            else
            {
                //有两种情况需要考虑要不要查询所有的数据（ParentUUID=null)
                //1.树形结构数据，第一次查询需要过滤ParentUUID==null
                //2.自嵌套的目前结构，也需要过滤ParentUUID==null
                var selfEmbedded = textFolder.EmbeddedFolders != null && textFolder.EmbeddedFolders.Contains(textFolder.FullName, StringComparer.OrdinalIgnoreCase);
                if (showTreeStyle || selfEmbedded)
                {
                    query = query.Where(new OrElseExpression(new WhereEqualsExpression(null, "ParentUUID", null), new WhereEqualsExpression(null, "ParentUUID", "")));
                }
            }

            if (childFolders != null)
            {
                childFolders = childFolders
                    .Select(it => it.AsActual())
                    .Where(it => it.VisibleOnSidebarMenu == null || it.VisibleOnSidebarMenu.Value == true)
                    .Where(it => Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager.AvailableViewContent(it, User.Identity.Name));
            }
            page = page ?? 1;
            pageSize = pageSize ?? textFolder.PageSize;

            //var pagedList = query.ToPageList(page.Value, pageSize.Value);

            //IEnumerable<TextContent> contents = pagedList.ToArray();

            //if (Repository.EnableWorkflow == true)
            //{
            //    contents = ServiceFactory.WorkflowManager.GetPendWorkflowItemForContents(Repository, contents.ToArray(), User.Identity.Name);
            //}

            //var workflowContentPagedList = new PagedList<TextContent>(contents, page.Value, pageSize.Value, pagedList.TotalItemCount);
            //ViewData["ContentPagedList"] = workflowContentPagedList;
            return View(new TextContentGrid()
            {
                ChildFolders = childFolders.ToArray(),
                ContentQuery = query,
                PageIndex = page.Value,
                PageSize = pageSize.Value,
                ShowTreeStyle = showTreeStyle
            });
        }

        public virtual ActionResult SubContent(string parentFolder, string folderName, string parentUUID, IEnumerable<WhereClause> whereClause,
            string search, int? page, int? pageSize, string orderField = null, string direction = null)
        {
            return Index(folderName, parentUUID, parentFolder, search, whereClause, page, pageSize, orderField, direction);
        }
        public virtual ActionResult Selectable(string categoryFolder, int? page, int? pageSize, string orderField = null, string direction = null)
        {
            var textFolder = (TextFolder)(FolderHelper.Parse<TextFolder>(Repository, categoryFolder).AsActual());
            Schema schema = new Schema(Repository, textFolder.SchemaName).AsActual();
            SchemaPath schemaPath = new SchemaPath(schema);
            ViewData["Folder"] = textFolder;
            ViewData["Schema"] = schema;
            ViewData["Template"] = textFolder.GetFormTemplate(FormType.Selectable);


            IContentQuery<TextContent> query = textFolder.CreateQuery();
            if (string.IsNullOrEmpty(orderField))
            {
                query = query.DefaultOrder();
            }
            else
            {
                if (!string.IsNullOrEmpty(direction) && direction.ToLower() == "asc")
                {
                    query = query.OrderBy(orderField);
                }
                else
                {
                    query = query.OrderByDescending(orderField);
                }
            }
            return View(query.ToPageList(page ?? 1, pageSize ?? textFolder.PageSize));
        }
        #endregion

        #region CUD

        // Kooboo.CMS.Account.Models.Permission.Contents_ContentPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpGet]
        public virtual ActionResult Create(string folderName, string parentFolder)
        {
            TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
            var schema = textFolder.GetSchema().AsActual();

            SchemaPath schemaPath = new SchemaPath(schema);
            ViewData["Folder"] = textFolder;
            ViewData["Schema"] = schema;
            ViewData["Template"] = textFolder.GetFormTemplate(FormType.Create);
            SetPermissionData(textFolder);

            var content = schema.DefaultContent();
            content = Kooboo.CMS.Content.Models.Binder.TextContentBinder.DefaultBinder.Bind(schema, content, Request.QueryString, true, false);

            return View(content);
        }

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpPost]
        public virtual ActionResult Create(string folderName, string parentFolder, string parentUUID, FormCollection form)
        {
            JsonResultEntry resultEntry = new JsonResultEntry() { Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                    var schema = textFolder.GetSchema().AsActual();

                    SchemaPath schemaPath = new SchemaPath(schema);
                    IEnumerable<TextContent> addedCategories;
                    IEnumerable<TextContent> removedCategories;

                    ParseCategories(form, out addedCategories, out removedCategories);
                    ContentBase content;

                    content = ServiceFactory.TextContentManager.Add(Repository, textFolder, parentFolder, parentUUID, form, Request.Files, addedCategories, User.Identity.Name);

                    resultEntry.ReloadPage = true;

                    resultEntry.Success = true;
                }
            }
            catch (RuleViolationException ruleEx)
            {
                foreach (var item in ruleEx.Issues)
                {
                    resultEntry.AddFieldError(item.PropertyName, item.ErrorMessage);
                }
            }
            catch (Exception e)
            {
                resultEntry.SetFailed().AddException(e);
            }
            resultEntry.AddModelState(ViewData.ModelState);
            return Json(resultEntry);


        }


        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpGet]
        public virtual ActionResult Edit(string folderName, string parentFolder, string uuid)
        {
            TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
            var schema = textFolder.GetSchema().AsActual();

            SchemaPath schemaPath = new SchemaPath(schema);
            ViewData["Folder"] = textFolder;
            ViewData["Schema"] = schema;
            ViewData["Template"] = textFolder.GetFormTemplate(FormType.Update);
            SetPermissionData(textFolder);
            var content = schema.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();
            if (content != null)
            {
                content = ServiceFactory.WorkflowManager.GetPendWorkflowItemForContent(Repository, content, User.Identity.Name);
            }
            return View(content);
        }


        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpPost]
        public virtual ActionResult Edit(string folderName, string parentFolder, string uuid, FormCollection form, bool? localize)
        {
            JsonResultEntry resultEntry = new JsonResultEntry() { Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                    var schema = textFolder.GetSchema().AsActual();

                    SchemaPath schemaPath = new SchemaPath(schema);
                    IEnumerable<TextContent> addedCategories;
                    IEnumerable<TextContent> removedCategories;

                    ParseCategories(form, out addedCategories, out removedCategories);
                    ContentBase content;
                    //if (textFolder != null)
                    //{
                    content = ServiceFactory.TextContentManager.Update(Repository, textFolder, uuid, form,
                    Request.Files, DateTime.UtcNow, addedCategories, removedCategories, User.Identity.Name);

                    if (localize.HasValue && localize.Value == true)
                    {
                        ServiceFactory.TextContentManager.Localize(textFolder, uuid);
                    }
                    //}
                    //else
                    //{
                    //    content = ServiceFactory.TextContentManager.Update(Repository, schema, uuid, form,
                    //    Request.Files, User.Identity.Name);
                    //}

                    resultEntry.Success = true;
                }
            }
            catch (RuleViolationException violationException)
            {
                foreach (var item in violationException.Issues)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                resultEntry.Success = false;
            }
            catch (Exception e)
            {
                resultEntry.SetFailed().AddException(e);
            }
            resultEntry.AddModelState(ViewData.ModelState);
            return Json(resultEntry);
        }
        // Kooboo.CMS.Account.Models.Permission.Contents_ContentPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult Delete(string folderName, string parentFolder, string[] docArr, string[] folderArr)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                var schema = textFolder.GetSchema().AsActual();

                if (docArr != null)
                {
                    foreach (var doc in docArr)
                    {
                        if (string.IsNullOrEmpty(doc)) continue;

                        ServiceFactory.TextContentManager.Delete(Repository, textFolder, doc);
                    }

                }

                if (folderArr != null)
                {
                    foreach (var f in folderArr)
                    {
                        if (string.IsNullOrEmpty(f)) continue;
                        var folderPathArr = FolderHelper.SplitFullName(f);
                        ServiceFactory.TextFolderManager.Remove(Repository, new TextFolder(Repository, folderPathArr));
                    }
                }


                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);

        }

        private void ParseCategories(FormCollection form, out IEnumerable<TextContent> addedCategory, out IEnumerable<TextContent> removedCategory)
        {
            List<TextContent> added = new List<TextContent>();
            List<TextContent> removed = new List<TextContent>();
            Func<string, TextContent> parseToCategoryContent = uuidAndFolder =>
            {
                if (string.IsNullOrEmpty(uuidAndFolder))
                {
                    return null;
                }
                var spliter = uuidAndFolder.Split(':');
                var uuid = spliter[0];
                var categoryFolder = spliter[1];
                return new TextContent(Repository.Name, "", categoryFolder)
                {
                    UUID = uuid
                };
            };

            foreach (var item in GetCategories(form))
            {
                var categoryFolder = new TextFolder(Repository, FolderHelper.SplitFullName(item.CategoryFolderName));
                string[] newValues = string.IsNullOrEmpty(item.Value) ? new string[0] : item.Value.Split(',');
                string[] oldValues = string.IsNullOrEmpty(item.OldValue) ? new string[0] : item.OldValue.Split(',');

                var addValues = newValues.Except(oldValues, StringComparer.CurrentCultureIgnoreCase);
                var removedValues = oldValues.Except(newValues, StringComparer.CurrentCultureIgnoreCase);

                added.AddRange(addValues.Select(parseToCategoryContent));
                removed.AddRange(removedValues.Select(parseToCategoryContent));

                //ServiceFactory.TextContentManager.AddCategories(Repository, content, addValues.Select(v => new TextContent(Repository.Name, "", item.CategoryFolderName) { UUID = v }).ToArray());
                //ServiceFactory.TextContentManager.RemoveCategories(Repository, content, removedValues.Select(v => new TextContent(Repository.Name, "", item.CategoryFolderName) { UUID = v }).ToArray());
            }
            addedCategory = added;
            removedCategory = removed;
        }
        private IEnumerable<CategoryValue> GetCategories(FormCollection form)
        {
            foreach (var key in form.AllKeys)
            {
                if (key.StartsWith("cat_") && key.EndsWith("_value"))
                {
                    var category = key.Substring(4, key.Length - 10);
                    var oldKey = key + "_old";
                    yield return new CategoryValue() { CategoryFolderName = category, Value = form[key], OldValue = form[oldKey] };
                }
            }
        }


        private void SetPermissionData(TextFolder folder)
        {
            var workflowManager = Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager;

            ViewData["AllowedEdit"] = workflowManager.AvailableToPublish(folder, User.Identity.Name);

            ViewData["AllowedView"] = workflowManager.AvailableViewContent(folder, User.Identity.Name);

        }
        #endregion

        #region Categories
        public virtual ActionResult SelectCategories(string folderName, string selected, int? page, int? pageSize, string search, IEnumerable<WhereClause> whereClause)
        {
            var textFolder = (TextFolder)(FolderHelper.Parse<TextFolder>(Repository, folderName).AsActual());

            var singleChoice = string.Equals("True", Request.RequestContext.GetRequestValue("SingleChoice"), StringComparison.OrdinalIgnoreCase);

            Schema schema = new Schema(Repository, textFolder.SchemaName).AsActual();
            SchemaPath schemaPath = new SchemaPath(schema);
            ViewData["Folder"] = textFolder;
            ViewData["Schema"] = schema;
            ViewData["Template"] = textFolder.GetFormTemplate(FormType.Selectable);
            ViewData["WhereClause"] = whereClause;

            IEnumerable<TextFolder> childFolders = new TextFolder[0];
            //Skip the child folders on the embedded folder grid.
            if (!page.HasValue || page.Value <= 1)
            {
                childFolders = ServiceFactory.TextFolderManager.ChildFoldersWithSameSchema(textFolder).Select(it => it.AsActual());
            }

            var query = textFolder.CreateQuery().DefaultOrder();

            bool showTreeStyle = schema.IsTreeStyle;
            if (showTreeStyle)
            {
                query = query.Where(new OrElseExpression(new WhereEqualsExpression(null, "ParentUUID", null), new WhereEqualsExpression(null, "ParentUUID", "")));
            }

            if (!string.IsNullOrEmpty(search))
            {
                IWhereExpression exp = new FalseExpression();
                foreach (var item in schema.Columns.Where(it => it.ShowInGrid))
                {
                    exp = new OrElseExpression(exp, (new WhereContainsExpression(null, item.Name, search)));
                }
                if (exp != null)
                {
                    query = query.Where(exp);
                }
            }
            if (whereClause != null && whereClause.Count() > 0)
            {
                var expression = WhereClauseToContentQueryHelper.Parse(whereClause, schema, new MVCValueProviderWrapper(ValueProvider));
                query = query.Where(expression);
            }

            var contents = query.ToPageList(page ?? 1, pageSize ?? textFolder.PageSize);
            SelectableViewModel viewModel = new SelectableViewModel() { ChildFolders = childFolders, Contents = contents, SingleChoice = singleChoice };

            if (Request.IsAjaxRequest())
            {
                return PartialView("", viewModel);
            }
            else
            {
                IEnumerable<TextContent> selectedContents = new TextContent[0];
                if (!string.IsNullOrEmpty(selected))
                {
                    string[] selectedArr = selected.Split(',');
                    IContentQuery<TextContent> selectedQuery = textFolder.CreateQuery().DefaultOrder();
                    foreach (var userKey in selectedArr)
                    {
                        selectedQuery = selectedQuery.Or((IWhereExpression)textFolder.CreateQuery().DefaultOrder().WhereEquals("UUID", userKey).Expression);
                    }

                    selectedContents = selectedQuery;
                }
                viewModel.Selected = selectedContents;
            }

            return View(viewModel);


        }
        public virtual ActionResult Categories(string uuid, string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                return null;
            }

            return PartialView(ServiceFactory.TextContentManager.QueryCategories(Repository, folderName, uuid));
        }
        #endregion

        #region Version Diff

        public virtual ActionResult Versions(string folderName, string parentFolder, string uuid)
        {
            TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
            var schema = textFolder.GetSchema().AsActual();

            var textContent = schema.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();
            var versions = VersionManager.AllVersionInfos(textContent);
            return View(versions);
        }
        public virtual ActionResult Diff(string folderName, string parentFolder, string uuid, int v1, int v2)
        {
            TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
            var schema = textFolder.GetSchema().AsActual();

            var textContent = schema.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();


            var version1 = VersionManager.GetVersion(textContent, v1);
            var version1Content = new TextContent(version1.TextContent);
            var version2 = VersionManager.GetVersion(textContent, v2);
            var version2Content = new TextContent(version2.TextContent);


            var sideBySideDiffBuilder = new SideBySideDiffBuilder(new Differ());

            var model = new TextContentDiffModel() { UUID = uuid, Version1Name = v1, Version2Name = v2 };


            foreach (var k in textContent.Keys)
            {
                var version1Text = version1Content[k] != null ? version1Content[k].ToString() : "";

                var version2Text = version2Content[k] != null ? version2Content[k].ToString() : "";

                var diffModel = sideBySideDiffBuilder.BuildDiffModel(version1Text, version2Text);

                model.Version1.Add(k, diffModel.OldText);
                model.Version2.Add(k, diffModel.NewText);

            }


            return View(model);
        }
        [HttpPost]
        public virtual ActionResult RevertTo(string folderName, string schemaName, string uuid, int version)
        {
            var entry = new JsonResultEntry();
            try
            {
                ServiceFactory.TextContentManager.RevertTo(Repository, folderName, schemaName, uuid, version, User.Identity.Name);
                entry.SetSuccess().AddMessage("Revert successfully.");
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }

        #endregion

        #region Publish
        // Kooboo.CMS.Account.Models.Permission.Contents_ContentPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpPost]
        public virtual ActionResult Publish(string folderName, string parentFolder, string uuid)
        {
            var entry = new JsonResultEntry();

            try
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                var schema = textFolder.GetSchema().AsActual();

                TextContent textContent = schema.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();
                var published = (bool?)textContent["Published"];
                if (published.HasValue && published.Value == true)
                {
                    ServiceFactory.TextContentManager.Update(Repository, schema, uuid, new string[] { "Published" }, new object[] { false }, User.Identity.Name);
                }
                else
                {
                    ServiceFactory.TextContentManager.Update(Repository, schema, uuid, new string[] { "Published" }, new object[] { true }, User.Identity.Name);
                }


            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult BatchPublish(string folderName, string parentFolder, string[] docArr)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                var schema = textFolder.GetSchema().AsActual();

                if (docArr != null)
                {
                    foreach (var doc in docArr)
                    {
                        if (string.IsNullOrEmpty(doc)) continue;

                        ServiceFactory.TextContentManager.Update(Repository, schema, doc, new string[] { "Published" },
                            new object[] { true }, User.Identity.Name);
                    }

                }

                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);

        }
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult BatchUnpublish(string folderName, string parentFolder, string[] docArr)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                var schema = textFolder.GetSchema().AsActual();

                if (docArr != null)
                {
                    foreach (var doc in docArr)
                    {
                        if (string.IsNullOrEmpty(doc)) continue;

                        ServiceFactory.TextContentManager.Update(Repository, schema, doc, new string[] { "Published" },
                            new object[] { false }, User.Identity.Name);
                    }

                }

                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);

        }
        #endregion

        #region Temporary Files

        public virtual ActionResult TempFile(string sourceUrl, string previewUrl, int x = 0, int y = 0, int width = 0, int height = 0)
        {
            ViewData["SourceUrl"] = sourceUrl;
            ViewData["PreviewUrl"] = previewUrl;
            ViewData["CropParam"] = new
            {
                Url = sourceUrl,
                X = x,
                Y = y,
                Width = width,
                Height = height
            };
            return View();
        }

        [HttpPost]
        public virtual ActionResult TempFile()
        {
            var entry = new JsonResultEntry();

            try
            {

                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    var postFile = Request.Files[0];
                    var repositoryPath = new RepositoryPath(Repository);
                    var tempPath = Kooboo.Web.Url.UrlUtility.Combine(repositoryPath.VirtualPath, "Temp");
                    Kooboo.IO.IOUtility.EnsureDirectoryExists(Server.MapPath(tempPath));

                    var fileUrl = Kooboo.Web.Url.UrlUtility.Combine(tempPath, UniqueIdGenerator.GetInstance().GetBase32UniqueId(24) + Path.GetExtension(postFile.FileName));

                    postFile.SaveAs(Server.MapPath(fileUrl));
                    entry.Model = Url.Content(fileUrl);
                }
                else
                {
                    entry.SetFailed().AddMessage("It is not a valid image file.".Localize());
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }



        #endregion

        #region Order

        public virtual ActionResult Top(string folderName, string uuid)
        {
            JsonResultEntry result = new JsonResultEntry();
            try
            {
                ServiceFactory.TextContentManager.Top(Repository, folderName, uuid);
            }
            catch (Exception e)
            {
                result.AddException(e);
            }

            return Json(result);
        }

        [HttpPost]
        public virtual ActionResult Sort(IEnumerable<ContentSorter> list, string folderName)
        {
            var entry = new JsonResultEntry();

            try
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                var schema = textFolder.GetSchema().AsActual();
                foreach (var c in list)
                {
                    ServiceFactory.TextContentManager.Update(Repository, schema, c.UUID, new string[] { "Sequence" }, new object[] { c.Sequence }, User.Identity.Name);
                }

            }
            catch (Exception e)
            {

            }

            return Json(entry);
        }

        [HttpPost]
        public virtual ActionResult CrossPageSort(ContentSorter sourceContent, string folderName, int? page, int? pageSize, bool up = true)
        {
            var entry = new JsonResultEntry();
            try
            {
                //page = page ?? 1;
                //pageSize = pageSize ?? 50;
                var textFolder = new TextFolder(Repository, folderName);
                var content = new TextFolder(Repository, folderName).CreateQuery().WhereEquals("UUID", sourceContent.UUID).FirstOrDefault();

                TextContent destContent;
                if (up)
                {
                    destContent = content.Previous();
                }
                else
                {
                    destContent = content.Next();
                }
                if (destContent != null)
                {
                    ServiceFactory.TextContentManager.Update(textFolder, sourceContent.UUID, "Sequence", destContent["Sequence"], User.Identity.Name);
                    ServiceFactory.TextContentManager.Update(textFolder, destContent.UUID, "Sequence", content["Sequence"], User.Identity.Name);
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }

        #endregion

        #region Move content
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult MoveContent(string folderName, string uuid, string parentUUID)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                if (uuid != parentUUID)
                {
                    var textFolder = new TextFolder(Repository.Current, folderName);
                    ServiceFactory.TextContentManager.Update(textFolder, uuid, new[] { "ParentUUID", "ParentFolder" },
                        new[] { parentUUID, folderName }, User.Identity.Name);
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }
        #endregion

        #region Move Conent And Sort
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult ReconfigContent(IEnumerable<ContentSorter> list, string folderName, string uuid, string parentUUID)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                if (!string.IsNullOrEmpty(uuid) && string.Compare(uuid, parentUUID, true) != 0)
                {
                    ServiceFactory.TextContentManager.Update(textFolder, uuid, new[] { "ParentUUID", "ParentFolder" },
                new[] { parentUUID, folderName }, User.Identity.Name);
                }

                var schema = textFolder.GetSchema().AsActual();
                foreach (var c in list)
                {
                    ServiceFactory.TextContentManager.Update(Repository, schema, c.UUID, new string[] { "Sequence" }, new object[] { c.Sequence }, User.Identity.Name);
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }
        #endregion

        #region Copy
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult Copy(string folderName, string parentFolder, string[] docArr, string[] folderArr)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                var schema = textFolder.GetSchema().AsActual();

                if (docArr != null)
                {
                    foreach (var doc in docArr)
                    {
                        if (string.IsNullOrEmpty(doc)) continue;

                        ServiceFactory.TextContentManager.Copy(textFolder.GetSchema(), doc);
                    }

                }
                entry.ReloadPage = true;
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);

        }
        #endregion

        #region Rebroadcast
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult Rebroadcast(string folderName, string parentFolder, string[] docArr, string[] folderArr)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();

                if (docArr != null)
                {
                    foreach (var doc in docArr)
                    {
                        if (string.IsNullOrEmpty(doc)) continue;

                        var textContent = textFolder.CreateQuery().WhereEquals("UUID", doc).FirstOrDefault();
                        if (textContent != null)
                        {
                            Kooboo.CMS.Content.EventBus.Content.ContentEvent.Fire(ContentAction.Add, textContent);
                        }
                    }
                }

                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);

        }
        #endregion

        #region QueryByParentUUID json
        public ActionResult QueryByParentUUID(string folderName, string parentUUID, string parentChain)
        {
            JsonResultEntry data = new JsonResultEntry();
            var textFolder = new TextFolder(Repository, folderName).AsActual();
            var contentQuery = textFolder.CreateQuery().WhereEquals("ParentUUID", parentUUID);
            var contents = contentQuery.DefaultOrder().ToArray();
            var requestContext = ControllerContext.RequestContext;

            List<TextFolder> embeddedFolders = new List<TextFolder>();
            if (textFolder.EmbeddedFolders != null)
            {
                foreach (var s in textFolder.EmbeddedFolders)
                {
                    embeddedFolders.Add(Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(new TextFolder(Repository.Current, s)));
                }
            }
            var url = requestContext.UrlHelper();
            parentChain = string.IsNullOrEmpty(parentChain) ? parentUUID : parentChain + "," + parentUUID;
            foreach (var item in contents)
            {
                item["_LocalCreationDate_"] = item.UtcCreationDate.ToLocalTime().ToShortDateString();
                item["_EditUrl_"] = url.Action("Edit", requestContext.AllRouteValues().Merge("UserKey", (object)(item.UserKey)).Merge("UUID", (object)(item.UUID)));
                item["_CreateUrl_"] = url.Action("Create", requestContext.AllRouteValues().Merge("UserKey", null).Merge("ParentUUID", (object)(item.UUID)));
                item["_VersionUrl_"] = url.Action("Versions", requestContext.AllRouteValues().Merge("UserKey", (object)(item.UserKey)).Merge("UUID", (object)(item.UUID)));
                item["_PublishUrl_"] = url.Action("Publish", requestContext.AllRouteValues().Merge("UserKey", (object)(item.UserKey)).Merge("UUID", (object)(item.UUID)));
                item["_ParentChain_"] = parentChain;
                item["_EmbeddedFolders_"] = embeddedFolders.Select(it => new
                {
                    Text = it.FriendlyText,
                    Link = url.Action("SubContent", new { SiteName = requestContext.GetRequestValue("SiteName"), RepositoryName = Repository.Name, ParentFolder = item.FolderName, FolderName = it.FullName, parentUUID = (object)(item.UUID) })
                });
            }
            data.Model = contents;

            return new Json_netResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        #endregion
    }

    //public static class TextContentWorkflowVisible
    //{
    //    public static bool NeedProcess(this TextContent content, System.Web.Mvc.ControllerBase controllerBase)
    //    {
    //        var controller = (Controller)controllerBase;
    //        var Manager = ServiceFactory.WorkflowManager;

    //        var items = Manager.GetPendingWorkflowItems(Repository.Current, controller.User.Identity.Name);

    //        return items.Where(o => o.ContentUUID == content.UUID).Count() > 0;
    //    }

    //    public static PendingWorkflowItem NeedProcessItem(this TextContent content, System.Web.Mvc.ControllerBase controllerBase)
    //    {
    //        var controller = (Controller)controllerBase;
    //        var Manager = ServiceFactory.WorkflowManager;

    //        var items = Manager.GetPendingWorkflowItems(Repository.Current, controller.User.Identity.Name);

    //        return items.Where(o => o.ContentUUID == content.UUID).FirstOrDefault();
    //    }

    //    public static IEnumerable<PendingWorkflowItem> NeedProcessItems(this System.Web.Mvc.ControllerBase controllerBase)
    //    {
    //        var controller = (Controller)controllerBase;
    //        var Manager = ServiceFactory.WorkflowManager;

    //        var items = Manager.GetPendingWorkflowItems(Repository.Current, controller.User.Identity.Name);

    //        return items;
    //    }
    //}
}
