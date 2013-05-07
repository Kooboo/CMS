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
using Kooboo.CMS.Content.Models.Binder;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web.Authorizations;
using Kooboo.CMS.Web.Models;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Authorization(AreaName = "Sites", Group = "Page", Name = "Publish", Order = 1)]
    public class InlineEditingController : Kooboo.CMS.Sites.AreaControllerBase
    {
        IPageProvider PageProvider { get; set; }
        ITextContentBinder Binder { get; set; }
        public InlineEditingController(IPageProvider pageProvider, ITextContentBinder binder)
        {
            this.PageProvider = pageProvider;
            Binder = binder;
        }
        #region Content
        [HttpPost]
        public virtual ActionResult CopyContent(string schema, string uuid)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                var content = Kooboo.CMS.Content.Services.ServiceFactory.TextContentManager.Copy(new Schema(Repository.Current, schema), uuid);
                resultData.Model = new
                {
                    uuid = content.UUID,
                    schema = content.SchemaName,
                    published = string.Empty,
                    editUrl = Url.Action("InlineEdit", new
                    {
                        controller = "TextContent",
                        Area = "Contents",
                        RepositoryName = content.Repository,
                        SiteName = Site.FullName,
                        FolderName = content.FolderName,
                        UUID = content.UUID
                    }),
                    summary = HttpUtility.HtmlAttributeEncode(content.GetSummary())
                };
            });
            return Json(data);
        }
        [HttpPost]
        public virtual ActionResult DeleteContent(string schema, string uuid)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                Kooboo.CMS.Content.Services.ServiceFactory.TextContentManager.Delete(Repository.Current, new Schema(Repository.Current, schema), uuid);
            });
            return Json(data);
        }
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult UpdateContent(string schema, string uuid, string fieldName, string value)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                var schemaObject = new Schema(Repository.Current, schema).AsActual();

                var fieldValue = Binder.ConvertToColumnType(schemaObject, fieldName, value);

                Kooboo.CMS.Content.Services.ServiceFactory.TextContentManager.Update(Repository.Current, schemaObject, uuid, fieldName, fieldValue, User.Identity.Name);
            });
            return Json(data);
        }
        #endregion

        #region Html Position
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult UpdateHtml(string positionId, string pageName, string value, bool? _draft_)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
                {
                    Page page = (PageHelper.Parse(Site.Current, pageName)).AsActual();

                    if (page != null)
                    {
                        bool isDraft = _draft_.HasValue && _draft_.Value == true;
                        if (isDraft)
                        {
                            page = PageProvider.GetDraft(page);
                        }
                        var position = page.PagePositions.Where(it => it.PagePositionId.EqualsOrNullEmpty(positionId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                        if (position != null && position is HtmlPosition)
                        {
                            ((HtmlPosition)position).Html = value;
                        }
                        if (isDraft)
                        {
                            PageProvider.SaveAsDraft(page);
                        }
                        else
                        {
                            CMS.Sites.Services.ServiceFactory.PageManager.Update(Site.Current, page, page);
                        }
                    }
                });
            return Json(data);
        }
        #endregion

        #region Label
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult UpdateLable(string key, string category, string value)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
                {
                    var element = new Element() { Name = key, Category = category, Value = value };
                    ServiceFactory.LabelManager.Update(Site.Current, element);
                });
            return Json(data);
        }
        #endregion

        #region Html Block
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult UpdateHtmlBlock(string blockName, string value)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                var manager = ServiceFactory.GetService<HtmlBlockManager>();
                var old = manager.Get(this.Site, blockName);
                var @new = new HtmlBlock(this.Site, blockName) { Body = value };
                manager.Update(this.Site, @new, old);
            });
            return Json(data);
        }
        #endregion

        #region Variables
        [RequiredLogOn(Exclusive = true, Order = 0)]
        public virtual ActionResult Variables(string pageName, string _draft_)
        {
            var vars = PageDesignController.jsSerializer.Serialize(new
            {
                #region urls
                CopyContent = Url.Action("CopyContent", new { controller = "InlineEditing", repositoryName = Repository.Current.Name, siteName = Site.Current.FullName, area = "Sites" }),
                UpdateContent = Url.Action("UpdateContent", new { controller = "InlineEditing", repositoryName = Repository.Current.Name, siteName = Site.Current.FullName, area = "Sites" }),
                DeleteContent = Url.Action("DeleteContent", new { controller = "InlineEditing", repositoryName = Repository.Current.Name, siteName = Site.Current.FullName, area = "Sites" }),
                UpdateHtml = Url.Action("UpdateHtml", new { controller = "InlineEditing", repositoryName = Repository.Current.Name, siteName = Site.Current.FullName, area = "Sites", pageName = pageName, _draft_ = _draft_ }),
                UpdateHtmlBlock = Url.Action("UpdateHtmlBlock", new { controller = "InlineEditing", repositoryName = Repository.Current.Name, siteName = Site.Current.FullName, area = "Sites" }),
                UpdateLable = Url.Action("UpdateLable", new { controller = "InlineEditing", repositoryName = Repository.Current.Name, siteName = Site.Current.FullName, area = "Sites" }),
                MediaLibrary = Url.Action("Selection", new { controller = "MediaContent", repositoryName = Repository.Current.Name, siteName = Site.Current.FullName, area = "Contents", listType = "grid" }),
                ApplicationPath = Url.Content("~"),
                #endregion

                #region anchorBar
                blockMenuAnchorBar_js = new
                {
                    editBtnTitle = "Edit".Localize(),
                    copyBtnTitle = "Copy".Localize(),
                    deleteBtnTitle = "Delete".Localize(),
                    publishBtnTitle = "Publish".Localize(),
                    unpublishedTip = "This item has not been published yet.<br/>Click to publish this item.".Localize()
                },

                fieldMenuAnchorBar_js = new
                {
                    editBtnTitle = "Edit".Localize()
                },

                htmlMenuAnchorBar_js = new
                {
                    editBtnTitle = "Edit".Localize()
                },

                inlineEditorAnchorBar_js = new
                {
                    saveBtnTitle = "Save".Localize(),
                    cancelBtnTitle = "Cancel".Localize(),
                    fontFamilyTitle = "Font family".Localize(),
                    fontSizeTitle = "Font size".Localize(),
                    fontColorTitle = "Font color".Localize(),
                    backColorTitle = "Background color".Localize()
                },
                #endregion

                #region colorPicker
                colorPicker_js = new
                {
                    caption = "Color picker".Localize(),
                    description = "Select a color or insert a hex code value.".Localize(),
                    originalColor = "Original:".Localize(),
                    newColor = "New:".Localize(),
                    hexValue = "Hex value:".Localize(),
                    useNewColorBtn = "OK".Localize(),
                    cancelBtn = "Cancel".Localize()
                },
                #endregion

                #region editor
                sniffer_js = new
                {
                    widthFormatError = "Invalid input width".Localize(),
                    heightFormatError = "Invalid input height".Localize(),
                    imgSizeConfirm = "The image size is too big for this layout.\nAre you sure you want to use this size?".Localize(),
                    deleteImgConfirm = "Are you sure you want to delete this image?".Localize(),
                    unlinkConfirm = "Are you sure you want to delete the link?".Localize()
                },

                toolbarButton_js = new
                {
                    bold = "Bold".Localize(),
                    italic = "Italic".Localize(),
                    underline = "Underline".Localize(),
                    alignLeft = "Align left".Localize(),
                    alignCenter = "Align center".Localize(),
                    alignRight = "Align right".Localize(),
                    alignJustify = "Align justify".Localize(),
                    numberList = "Number list".Localize(),
                    bulletList = "Bullet list".Localize(),
                    indent = "Increase indent".Localize(),
                    outdent = "Decrease indent".Localize(),
                    insertImage = "Insert image".Localize(),
                    insertLink = "Insert link".Localize(),
                    editSource = "Edit source".Localize(),
                    redo = "Redo".Localize(),
                    undo = "Undo".Localize(),
                    unformat = "Remove format".Localize(),
                    horizontalRuler = "Insert horizontal ruler".Localize(),
                    pastePlainText = "Paste with plain text".Localize()
                },
                #endregion

                #region inline
                block_js = new
                {
                    confirmDel = "Are you sure you want to delete this item?".Localize(),
                    networkError = "Network error, the action has been cancelled.".Localize(),
                    copying = "Copying...".Localize(),
                    deleting = "Deleting...".Localize(),
                    publishing = "Publishing...".Localize(),
                    copySuccess = "Copy successfully.".Localize(),
                    deleteSuccess = "Delete successfully.".Localize(),
                    publishSuccess = "Publish successfully.".Localize(),
                    copyFailure = "The attempt to copy has failed.".Localize(),
                    deleteFailure = "The attempt to delete has failed.".Localize(),
                    publishFailure = "The attempt to publish has failed.".Localize()
                },

                field_js = new
                {
                    networkError = "Network error, the action has been cancelled.".Localize(),
                    saving = "Saving...".Localize(),
                    saveSuccess = "Save successfully.".Localize(),
                    saveFailure = "The attempt to save has failed.".Localize()
                },
                #endregion

                #region panel

                imagePanel_js = new
                {
                    title = "Image options".Localize(),
                    imgLibTitle = "Image library".Localize(),
                    attrURL = "URL".Localize(),
                    attrALT = "ALT".Localize(),
                    attrWidth = "Width".Localize(),
                    attrHeight = "Height".Localize(),
                    btnOk = "Save".Localize(),
                    btnCancel = "Cancel".Localize(),
                    btnRemove = "Remove".Localize(),
                    btnLibrary = "Library".Localize(),
                    btnView = "View".Localize(),
                    attrLinkHref = "LINK".Localize(),
                    emptyUrlMsg = "Please input the url.".Localize()
                },

                linkPanel_js = new
                {
                    headTitle = "Link panel".Localize(),
                    btnOk = "OK".Localize(),
                    btnCancel = "Cancel".Localize(),
                    btnUnlink = "Unlink".Localize(),
                    lblText = "Text:".Localize(),
                    lblUrl = "Url:".Localize(),
                    lblTitle = "Title:".Localize(),
                    lblLinkType = "Link type:".Localize(),
                    lblNewWin = "Open in a new window".Localize(),
                    urlEmptyMsg = "Please input the url.".Localize()
                },

                textPanel_js = new
                {
                    headTitle = "Text panel".Localize(),
                    btnPreview = "Preview".Localize(),
                    btnReset = "Reset".Localize(),
                    btnOk = "OK".Localize(),
                    btnCancel = "Cancel".Localize()
                }
                #endregion
            });

            // ret
            return JavaScript(string.Format("var __inlineEditVars = {0};", vars));
        }
        #endregion
    }
}
