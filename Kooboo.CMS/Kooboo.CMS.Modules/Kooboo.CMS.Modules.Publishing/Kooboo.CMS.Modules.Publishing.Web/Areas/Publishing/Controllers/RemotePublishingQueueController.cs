using Kooboo.Web.Mvc;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Common;
using Kooboo.CMS.Content.Models;
using Kooboo.Globalization;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Web.Areas.Contents.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Modules.Publishing.Services;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Web;
namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Publishing", Group = "Remote", Name = "Queue", Order = 1)]
    public class RemotePublishingQueueController : AreaControllerBase
    {
        #region .ctor
        private readonly RemotePublishingQueueManager _manager;
        public RemotePublishingQueueController(RemotePublishingQueueManager manager)
        {
            this._manager = manager;
        }
        #endregion

        #region Index
        public ActionResult Index(string siteName, string search, int? publishingObject, int? publishingType, int? status,
            string sortField, string sortDir)
        {
            var query = this._manager.CreateQuery(Site);
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(it => it.ObjectUUID.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            if (publishingObject.HasValue)
            {
                PublishingObject po = (PublishingObject)publishingObject.Value;
                query = query.Where(it => it.PublishingObject == po);
            }
            if (status.HasValue)
            {
                QueueStatus qs = (QueueStatus)status;
                query = query.Where(it => it.Status == qs);
            }
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                query = query.SortByField(sortField, sortDir);
            }
            else
            {
                query = query.OrderByDescending(it => it.UtcCreationDate);
            }
            return View(query.ToList());
        }
        #endregion

        #region PublishPage
        public ActionResult PublishPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PublishPage(RemotePagePublishingModel model, string @return)
        {
            var resultEntry = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                if (model.Schedule && !model.UtcTimeToPublish.HasValue && !model.UtcTimeToUnpublish.HasValue)
                {
                    resultEntry.AddErrorMessage("UtcTimeToPublish and UtcTimeToUnpublish can not be both empty.".Localize());
                }
                else
                {
                    foreach (string uuid in model.Pages)
                    {
                        foreach (string endpoint in model.RemoteEndPoints)
                        {
                            var queue = new RemotePublishingQueue(Site, Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10))
                            {
                                PublishingObject = PublishingObject.Page,                                
                                UserId = User.Identity.Name,
                                UtcCreationDate = DateTime.UtcNow,
                                RemoteEndpoint = endpoint,
                                ObjectUUID = uuid,
                                ObjectTitle = uuid,
                                Status = QueueStatus.Pending
                            };
                            if (model.Schedule)
                            {
                                if (model.UtcTimeToPublish.HasValue)
                                {
                                    queue.UtcTimeToPublish = model.UtcTimeToPublish.Value.ToUniversalTime();
                                }
                                if (model.UtcTimeToUnpublish.HasValue)
                                {
                                    queue.UtcTimeToUnpublish = model.UtcTimeToUnpublish.Value.ToUniversalTime();
                                }
                            }
                            else
                            {
                                queue.UtcTimeToPublish = DateTime.UtcNow;
                            }
                            resultEntry.RunWithTry((data) =>
                            {
                                _manager.Add(queue);
                            });
                        }
                    }
                    resultEntry.RedirectUrl = @return;
                }
            }
            return Json(resultEntry);
        }
        #endregion

        #region PublishTextContent
        public ActionResult PublishTextContent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PublishTextContent(CreateTextContentPublishingQueueViewModel model, string @return)
        {           
            if (model.TextFolderMappings == null || model.TextFolderMappings.Length == 0)
            {
                ModelState.AddModelError("TextFolderMappings", "Required".Localize());
            }
            var resultEntry = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                if (model.Schedule && !model.UtcTimeToPublish.HasValue && !model.UtcTimeToUnpublish.HasValue)
                {
                    resultEntry.AddErrorMessage("UtcTimeToPublish and UtcTimeToUnpublish can not be both empty.");
                }
                else
                {
                    TextFolder textFolder = new TextFolder(Repository.Current, model.LocalFolderId).AsActual();
                    for (int i = 0, j = model.TextContents.Length; i < j; i++)
                    {
                        var content = textFolder.CreateQuery().WhereEquals("UUID", model.TextContents[i]).FirstOrDefault();
                        if (content != null)
                        {
                            foreach (string mapping in model.TextFolderMappings)
                            {
                                var queue = new RemotePublishingQueue(Site, Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10))
                                {
                                    PublishingObject = PublishingObject.TextContent,                                    
                                    UserId = User.Identity.Name,
                                    UtcCreationDate = DateTime.UtcNow,
                                    TextFolderMapping = mapping,
                                    ObjectUUID = content.IntegrateId,
                                    ObjectTitle = model.ObjectTitles[i],
                                    Status = QueueStatus.Pending
                                };
                                if (model.Schedule)
                                {
                                    if (model.UtcTimeToPublish.HasValue)
                                    {
                                        queue.UtcTimeToPublish = model.UtcTimeToPublish.Value.ToUniversalTime();
                                    }
                                    if (model.UtcTimeToUnpublish.HasValue)
                                    {
                                        queue.UtcTimeToUnpublish = model.UtcTimeToUnpublish.Value.ToUniversalTime();
                                    }
                                }
                                else
                                {
                                    queue.UtcTimeToPublish = DateTime.UtcNow;
                                }
                                resultEntry.RunWithTry((data) =>
                                {
                                    _manager.Add(queue);
                                });
                            }
                        }
                    }
                    resultEntry.RedirectUrl = @return;
                }
            }
            return Json(resultEntry);
        }

        public virtual ActionResult SelectTextContent(string siteName, string folderName, string selected, int? page, int? pageSize, string search,
             string sortField = null, string sortDir = null)
        {
            var site = SiteHelper.Parse(siteName);
            var repository = site.GetRepository();
            var textFolder = (TextFolder)(FolderHelper.Parse<TextFolder>(repository, folderName).AsActual());

            var singleChoice = string.Equals("True", Request.RequestContext.GetRequestValue("SingleChoice"), StringComparison.OrdinalIgnoreCase);

            Schema schema = new Schema(repository, textFolder.SchemaName).AsActual();
            SchemaPath schemaPath = new SchemaPath(schema);
            ViewData["Folder"] = textFolder;
            ViewData["Schema"] = schema;
            ViewData["Template"] = textFolder.GetFormTemplate(FormType.Selectable);
            //ViewData["WhereClause"] = whereClause;

            IEnumerable<TextFolder> childFolders = new TextFolder[0];
            //Skip the child folders on the embedded folder grid.
            if (!page.HasValue || page.Value <= 1)
            {
                childFolders = ServiceFactory.TextFolderManager.ChildFoldersWithSameSchema(textFolder).Select(it => it.AsActual());
            }

            var query = textFolder.CreateQuery();

            if (!string.IsNullOrEmpty(sortField))
            {
                query = query.SortBy(sortField, sortDir);
            }
            else
            {
                query = query.DefaultOrder();
            }

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
        #endregion

        #region Detail
        public ActionResult Detail(string uuid)
        {
            var model = this._manager.Get(Site,uuid);
            return View(model);
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete(DeleteModel[] model)
        {
            var resultEntry = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                resultEntry.RunWithTry((data) =>
                {
                    var uuids = model.Select(it => it.UUID).ToArray();
                    if (uuids.Any())
                    {
                        _manager.Delete(Site,uuids);
                    }
                    data.ReloadPage = true;
                });
            }
            return Json(resultEntry);
        }
        #endregion
    }
}
