#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using DiffPlex;
using DiffPlex.DiffBuilder;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Binder;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Versioning;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Web.Areas.Contents.Models;
using Kooboo.CMS.Web.Models;
using Kooboo.Extensions;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Paging;
using Kooboo.Web.Script.Serialization;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
    [ValidateInput(false)]
    public class TextContentController : ContentControllerBase
    {
        #region .ctor
        TextContentManager TextContentManager { get; set; }
        TextFolderManager TextFolderManager { get; set; }
        WorkflowManager WorkflowManager { get; set; }
        ITextContentBinder Binder { get; set; }
        public TextContentController(TextContentManager textContentManager, TextFolderManager textFolderManager,
            WorkflowManager workflowManager, ITextContentBinder binder)
        {
            TextContentManager = textContentManager;
            TextFolderManager = textFolderManager;
            WorkflowManager = workflowManager;
            Binder = binder;
        }
        #endregion

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
            , IEnumerable<WhereClause> whereClause, int? page, int? pageSize, string sortField = null, string sortDir = null)
        {
            //compatible with the Folder parameter changed to FolderName.
            folderName = folderName ?? this.ControllerContext.RequestContext.GetRequestValue("Folder");

            TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
            var schema = textFolder.GetSchema().AsActual();

            SchemaPath schemaPath = new SchemaPath(schema);
            ViewData["Folder"] = textFolder;
            ViewData["Schema"] = schema;
            ViewData["Menu"] = textFolder.GetFormTemplate(FormType.Grid_Menu);
            ViewData["Template"] = textFolder.GetFormTemplate(FormType.Grid);
            ViewData["WhereClause"] = whereClause;

            SetPermissionData(textFolder);

            IEnumerable<TextFolder> childFolders = new TextFolder[0];
            //Skip the child folders on the embedded folder grid.
            if (string.IsNullOrEmpty(parentFolder))
            {
                if (!page.HasValue || page.Value <= 1)
                {
                    childFolders = TextFolderManager.ChildFolders(textFolder, search).Select(it => it.AsActual());
                }
            }

            IContentQuery<TextContent> query = textFolder.CreateQuery();
            query = SortByField(sortField, sortDir, query);
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
                    .Where(it => it.Visible)
                    .Where(it => Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager.AvailableViewContent(it, User.Identity.Name));
            }
            page = page ?? 1;
            pageSize = pageSize ?? textFolder.PageSize;

            //var pagedList = query.ToPageList(page.Value, pageSize.Value);

            //IEnumerable<TextContent> contents = pagedList.ToArray();

            //if (Repository.EnableWorkflow == true)
            //{
            //    contents =WorkflowManager.GetPendWorkflowItemForContents(Repository, contents.ToArray(), User.Identity.Name);
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

        private static IContentQuery<TextContent> SortByField(string sortField, string sortDir, IContentQuery<TextContent> query)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                query = query.DefaultOrder();
            }
            else
            {
                if (!string.IsNullOrEmpty(sortDir) && sortDir.ToLower() == "desc")
                {
                    query = query.OrderByDescending(sortField);
                }
                else
                {
                    query = query.OrderBy(sortField);
                }
            }
            return query;
        }

        public virtual ActionResult SubContent(string parentFolder, string folderName, string parentUUID, IEnumerable<WhereClause> whereClause,
            string search, int? page, int? pageSize, string orderField = null, string direction = null)
        {
            return Index(folderName, parentUUID, parentFolder, search, whereClause, page, pageSize, orderField, direction);
        }
        #endregion

        #region CUD

        #region Create
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
            ViewData["Menu"] = textFolder.GetFormTemplate(FormType.Create_Menu);
            ViewData["Template"] = textFolder.GetFormTemplate(FormType.Create);
            SetPermissionData(textFolder);

            var content = Binder.Default(schema);
            content = Binder.Bind(schema, content, Request.QueryString, true, false);

            return View(content);
        }

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpPost]
        public virtual ActionResult Create(string folderName, string parentFolder, string parentUUID, FormCollection form, string @return)
        {
            var data = new JsonResultData();
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

                    content = TextContentManager.Add(Repository, textFolder, parentFolder, parentUUID, form, Request.Files, addedCategories, User.Identity.Name);

                    data.RedirectUrl = @return;
                }
            }
            catch (RuleViolationException ruleEx)
            {
                foreach (var item in ruleEx.Issues)
                {
                    data.AddFieldError(item.PropertyName, item.ErrorMessage);
                }
            }
            catch (Exception e)
            {
                data.AddException(e);
            }
            data.AddModelState(ModelState);
            return Json(data);
        }

        #endregion

        #region Edit
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpGet]
        public virtual ActionResult Edit(string folderName, string uuid)
        {
            TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
            var schema = textFolder.GetSchema().AsActual();

            SchemaPath schemaPath = new SchemaPath(schema);
            ViewData["Folder"] = textFolder;
            ViewData["Schema"] = schema;
            ViewData["Menu"] = textFolder.GetFormTemplate(FormType.Update_Menu);
            ViewData["Template"] = textFolder.GetFormTemplate(FormType.Update);
            SetPermissionData(textFolder);
            var content = schema.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();
            if (content != null)
            {
                content = WorkflowManager.GetPendWorkflowItemForContent(Repository, content, User.Identity.Name);
            }
            return View(content);
        }


        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpPost]
        public virtual ActionResult Edit(string folderName, string uuid, FormCollection form, string @return, bool localize = false)
        {
            var data = new JsonResultData();
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

                    content = TextContentManager.Update(Repository, textFolder, uuid, form,
                    Request.Files, DateTime.UtcNow, addedCategories, removedCategories, User.Identity.Name);

                    if (localize == true)
                    {
                        TextContentManager.Localize(textFolder, uuid);
                    }

                    data.RedirectToOpener = true;

                    data.RedirectUrl = @return;

                }
            }
            catch (RuleViolationException violationException)
            {
                foreach (var item in violationException.Issues)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                data.Success = false;
            }
            catch (Exception e)
            {
                data.AddException(e);
            }
            data.AddModelState(ModelState);
            return Json(data);
        }

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpGet]
        public virtual ActionResult InlineEdit(string folderName, string uuid)
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
                content = WorkflowManager.GetPendWorkflowItemForContent(Repository, content, User.Identity.Name);
            }
            return View(content);
        }
        #endregion

        #region Delete
        // Kooboo.CMS.Account.Models.Permission.Contents_ContentPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult Delete(string folderName, string parentFolder, string[] folders, string[] docs)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                var schema = textFolder.GetSchema().AsActual();

                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (string.IsNullOrEmpty(doc)) continue;

                        TextContentManager.Delete(Repository, textFolder, doc);
                    }

                }

                if (folders != null)
                {
                    foreach (var f in folders)
                    {
                        if (string.IsNullOrEmpty(f)) continue;
                        var folderPathArr = FolderHelper.SplitFullName(f);
                        TextFolderManager.Remove(Repository, new TextFolder(Repository, folderPathArr));
                    }
                }

                resultData.ReloadPage = true;
            });
            return Json(data);

        }
        #endregion

        private void ParseCategories(FormCollection form, out IEnumerable<TextContent> addedCategory, out IEnumerable<TextContent> removedCategory)
        {
            List<TextContent> added = new List<TextContent>();
            List<TextContent> removed = new List<TextContent>();

            foreach (var item in GetCategories(form))
            {
                var categoryFolder = new TextFolder(Repository, FolderHelper.SplitFullName(item.CategoryFolderName));
                string[] newValues = string.IsNullOrEmpty(item.Value) ? new string[0] : item.Value.Split(',');
                string[] oldValues = string.IsNullOrEmpty(item.OldValue) ? new string[0] : item.OldValue.Split(',');

                var addValues = newValues.Except(oldValues, StringComparer.CurrentCultureIgnoreCase);
                var removedValues = oldValues.Except(newValues, StringComparer.CurrentCultureIgnoreCase);

                added.AddRange(addValues.Select(it => new TextContent(Repository.Name, "", categoryFolder.FullName) { UUID = it }));
                removed.AddRange(removedValues.Select(it => new TextContent(Repository.Name, "", categoryFolder.FullName) { UUID = it }));

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
        public virtual ActionResult SelectCategories(string folderName, string selected, int? page, int? pageSize, string search, IEnumerable<WhereClause> whereClause
            , string sortField = null, string sortDir = null)
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

            var query = textFolder.CreateQuery();

            query = SortByField(sortField, sortDir, query);


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
                showTreeStyle = false;
            }
            if (whereClause != null && whereClause.Count() > 0)
            {
                var expression = WhereClauseToContentQueryHelper.Parse(whereClause, schema, new MVCValueProviderWrapper(ValueProvider));
                query = query.Where(expression);
                showTreeStyle = false;
            }

            var contents = query.ToPageList(page ?? 1, pageSize ?? textFolder.PageSize, textFolder.EnablePaging.HasValue ? textFolder.EnablePaging.Value : true);
            SelectableViewModel viewModel = new SelectableViewModel() { ShowTreeStyle = showTreeStyle, ChildFolders = childFolders, Contents = contents, SingleChoice = singleChoice };

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
        public virtual ActionResult Diff(string folderName, string parentFolder, string uuid, string version)
        {
            TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
            var schema = textFolder.GetSchema().AsActual();

            var versions = version.Split(',');
            var v1 = int.Parse(versions[0]);
            var v2 = int.Parse(versions[1]);
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
        public virtual ActionResult RevertTo(string folderName, string schemaName, string uuid, Kooboo.CMS.Content.Versioning.VersionInfo[] model, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (model.Length == 1)
                {
                    TextContentManager.RevertTo(Repository, folderName, schemaName, uuid, model[0].Version, User.Identity.Name);
                    resultData.RedirectUrl = @return;
                }

            });
            return Json(data);
        }

        #endregion

        #region Publish
        // Kooboo.CMS.Account.Models.Permission.Contents_ContentPermission
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpPost]
        public virtual ActionResult Publish(string folderName, string parentFolder, string uuid)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                var schema = textFolder.GetSchema().AsActual();

                TextContent textContent = schema.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();
                var published = (bool?)textContent["Published"];
                if (published.HasValue && published.Value == true)
                {
                    TextContentManager.Update(Repository, schema, uuid, new string[] { "Published" }, new object[] { false }, User.Identity.Name);
                }
                else
                {
                    TextContentManager.Update(Repository, schema, uuid, new string[] { "Published" }, new object[] { true }, User.Identity.Name);
                }
            });

            return Json(data);
        }

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult BatchPublish(string folderName, string parentFolder, string[] docs)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                var schema = textFolder.GetSchema().AsActual();

                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (string.IsNullOrEmpty(doc)) continue;

                        TextContentManager.Update(Repository, schema, doc, new string[] { "Published" },
                             new object[] { true }, User.Identity.Name);
                    }

                }
                resultData.ReloadPage = true;
            });
            return Json(data);

        }
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult BatchUnpublish(string folderName, string parentFolder, string[] docs)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                var schema = textFolder.GetSchema().AsActual();

                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (string.IsNullOrEmpty(doc)) continue;

                        TextContentManager.Update(Repository, schema, doc, new string[] { "Published" },
                             new object[] { false }, User.Identity.Name);
                    }

                }
                resultData.ReloadPage = true;
            });
            return Json(data);

        }
        #endregion

        #region ImageCrop Files

        public virtual ActionResult ImageCrop(string sourceUrl, string previewUrl, int x = 0, int y = 0, int width = 0, int height = 0)
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
        public virtual ActionResult ImageCrop()
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    var postFile = Request.Files[0];
                    var repositoryPath = new RepositoryPath(Repository);
                    var tempPath = Kooboo.Web.Url.UrlUtility.Combine(repositoryPath.VirtualPath, "Temp");
                    Kooboo.IO.IOUtility.EnsureDirectoryExists(Server.MapPath(tempPath));

                    var fileUrl = Kooboo.Web.Url.UrlUtility.Combine(tempPath, UniqueIdGenerator.GetInstance().GetBase32UniqueId(24) + Path.GetExtension(postFile.FileName));

                    postFile.SaveAs(Server.MapPath(fileUrl));
                    resultData.Model = new { ImageUrl = Url.Content(fileUrl), PreviewUrl = this.Url.Action("ResizeImage", "Resource", new { siteName = Site.FullName, url = fileUrl, area = "", width = 600, height = 400, preserverAspectRatio = true, quality = 80 }) };
                }
                else
                {
                    resultData.AddErrorMessage("It is not a valid image file.".Localize());
                }
            });

            return Json(data);
        }

        #endregion

        #region Order

        public virtual ActionResult Top(string folderName, string uuid)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                TextContentManager.Top(Repository, folderName, uuid);
            });
            return Json(data);
        }

        [HttpPost]
        public virtual ActionResult Sort(ContentSorter[] list, string folderName)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                var schema = textFolder.GetSchema().AsActual();
                foreach (var c in list)
                {
                    TextContentManager.Update(Repository, schema, c.UUID, new string[] { "Sequence" }, new object[] { c.Sequence }, User.Identity.Name);
                }
                resultData.ReloadPage = true;
            });

            return Json(data);
        }

        [HttpPost]
        public virtual ActionResult CrossPageSort(ContentSorter sourceContent, string folderName, int? page, int? pageSize, bool up = true)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
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
                    TextContentManager.Update(textFolder, sourceContent.UUID, "Sequence", destContent["Sequence"], User.Identity.Name);
                    TextContentManager.Update(textFolder, destContent.UUID, "Sequence", content["Sequence"], User.Identity.Name);
                }
            });
            return Json(data);
        }

        #endregion

        #region Move content
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult MoveContent(string folderName, string uuid, string parentUUID)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (uuid != parentUUID)
                {
                    var textFolder = new TextFolder(Repository.Current, folderName);
                    TextContentManager.Update(textFolder, uuid, new[] { "ParentUUID", "ParentFolder" },
                         new[] { parentUUID, folderName }, User.Identity.Name);

                }

                resultData.ReloadPage = true;
            });
            return Json(data);
        }
        #endregion

        #region Copy
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        public virtual ActionResult Copy(string folderName, string parentFolder, string[] docs)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();
                var schema = textFolder.GetSchema().AsActual();

                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (string.IsNullOrEmpty(doc)) continue;

                        TextContentManager.Copy(textFolder.GetSchema(), doc);
                    }

                }
                data.ReloadPage = true;
            });
            return Json(data);

        }
        #endregion

        #region Rebroadcast
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpPost]
        public virtual ActionResult Rebroadcast(string folderName, string parentFolder, string[] docs)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();

                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (string.IsNullOrEmpty(doc)) continue;

                        var textContent = textFolder.CreateQuery().WhereEquals("UUID", doc).FirstOrDefault();
                        if (textContent != null)
                        {
                            Kooboo.CMS.Content.EventBus.Content.ContentEvent.Fire(ContentAction.Add, textContent);
                        }
                    }
                }
                resultData.ReloadPage = true;
            });
            return Json(data);

        }
        #endregion

        #region Localize
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Content", Order = 1)]
        [HttpPost]
        public virtual ActionResult Localize(string folderName, string[] docs)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                TextFolder textFolder = new TextFolder(Repository, folderName).AsActual();

                if (docs != null)
                {
                    foreach (var doc in docs)
                    {
                        if (string.IsNullOrEmpty(doc)) continue;

                        TextContentManager.Localize(textFolder, doc);
                    }
                }
                resultData.ReloadPage = true;
            });
            return Json(data);

        }
        #endregion

        #region QueryByParentUUID json
        public ActionResult QueryByParentUUID(string folderName, string parentUUID, string parentChain, string @return)
        {
            var data = new JsonResultData(ModelState);
            var textFolder = new TextFolder(Repository, folderName).AsActual();
            var contentQuery = textFolder.CreateQuery().WhereEquals("ParentUUID", parentUUID).DefaultOrder();
            var contents = contentQuery.ToArray();
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
                item["_EditUrl_"] = url.Action("Edit", requestContext.AllRouteValues().Merge("UserKey", (object)(item.UserKey)).Merge("UUID", (object)(item.UUID)).Merge("return", @return));
                item["_CreateUrl_"] = url.Action("Create", requestContext.AllRouteValues().Merge("UserKey", null).Merge("ParentFolder", folderName).Merge("ParentUUID", (object)(item.UUID)).Merge("return", @return));
                item["_VersionUrl_"] = url.Action("Versions", requestContext.AllRouteValues().Merge("UserKey", (object)(item.UserKey)).Merge("UUID", (object)(item.UUID)).Merge("return", @return));
                item["_PublishUrl_"] = url.Action("Publish", requestContext.AllRouteValues().Merge("UserKey", (object)(item.UserKey)).Merge("UUID", (object)(item.UUID)).Merge("return", @return));
                item["_ParentChain_"] = parentChain;
                item["_TRClass_"] = ("doctr" + (item.Published.HasValue && item.Published == true ? " published" : " unpublished") + ((item.IsLocalized != null && item.IsLocalized == false) ? " unlocalized" : " localized"));
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
}
