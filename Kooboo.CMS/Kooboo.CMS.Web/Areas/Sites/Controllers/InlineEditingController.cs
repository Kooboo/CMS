using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Globalization;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Web.Authorizations;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Authorization(AreaName = "Sites", Group = "Page", Name = "Publish", Order = 1)]
    public class InlineEditingController : AdminControllerBase
    {
        #region Content
        [HttpPost]
        public virtual ActionResult CopyContent(string schema, string uuid)
        {
            var result = new JsonResultEntry();
            try
            {
                var content = Kooboo.CMS.Content.Services.ServiceFactory.TextContentManager.Copy(new Schema(Repository.Current, schema), uuid);
                result.Model = new
                {
                    uuid = content.UUID,
                    schema = content.SchemaName,
                    published = string.Empty,
                    editUrl = Url.Action("Edit", new
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
            }
            catch (Exception e)
            {
                result.AddException(e);
            }

            return Json(result);
        }
        [HttpPost]
        public virtual ActionResult DeleteContent(string schema, string uuid)
        {
            var result = new JsonResultEntry();
            try
            {
                Kooboo.CMS.Content.Services.ServiceFactory.TextContentManager.Delete(Repository.Current, new Schema(Repository.Current, schema), uuid);
            }
            catch (Exception e)
            {
                result.AddException(e);
            }
            return Json(result);
        }
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult UpdateContent(string schema, string uuid, string fieldName, string value)
        {
            var result = new JsonResultEntry();
            try
            {
                var schemaObject = new Schema(Repository.Current, schema).AsActual();

                var fieldValue = Kooboo.CMS.Content.Models.Binder.TextContentBinder.DefaultBinder.ConvertToColumnType(schemaObject, fieldName, value);

                Kooboo.CMS.Content.Services.ServiceFactory.TextContentManager.Update(Repository.Current, schemaObject, uuid, fieldName, fieldValue, User.Identity.Name);
            }
            catch (Exception e)
            {
                result.AddException(e);
            }
            return Json(result);
        }
        #endregion

        #region Html Position
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult UpdateHtml(string positionId, string pageName, string value, bool? _draft_)
        {
            var result = new JsonResultEntry();
            try
            {
                Page page = CMS.Sites.Models.IPersistableExtensions.AsActual(PageHelper.Parse(Site.Current, pageName));

                if (page != null)
                {
                    bool isDraft = _draft_.HasValue && _draft_.Value == true;
                    if (isDraft)
                    {
                        page = Kooboo.CMS.Sites.Services.ServiceFactory.PageManager.PageProvider.GetDraft(page);
                    }
                    var position = page.PagePositions.Where(it => it.PagePositionId.EqualsOrNullEmpty(positionId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (position != null && position is HtmlPosition)
                    {
                        ((HtmlPosition)position).Html = value;
                    }
                    if (isDraft)
                    {
                        CMS.Sites.Services.ServiceFactory.PageManager.PageProvider.SaveAsDraft(page);
                    }
                    else
                    {
                        CMS.Sites.Services.ServiceFactory.PageManager.Update(Site.Current, page, page);
                    }
                }
            }
            catch (Exception e)
            {
                result.AddException(e);
            }
            return Json(result);
        }
        #endregion

        #region Label
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult UpdateLable(string key, string category, string value)
        {
            var result = new JsonResultEntry();
            try
            {
                var element = new Element() { Name = key, Category = category, Value = value };
                CMS.Sites.Services.ServiceFactory.LabelManager.Update(Site.Current, element);
            }
            catch (Exception e)
            {
                result.AddException(e);
            }
            return Json(result);
        }
        #endregion

        #region Html Block
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult UpdateHtmlBlock(string blockName, string value)
        {
            var result = new JsonResultEntry();
            try
            {
                var manager = ServiceFactory.GetService<HtmlBlockManager>();
                var old = manager.Get(this.Site, blockName);
                var @new = new HtmlBlock(this.Site, blockName) { Body = value };
                manager.Update(this.Site, @new, old);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.AddException(ex);
            }
            return Json(result);
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
                MediaLibrary = Url.Action("Selection", new { controller = "MediaContent", repositoryName = Repository.Current.Name, siteName = Site.Current.FullName, area = "Contents" }),
                ApplicationPath = Url.Content("~"),
                #endregion

                #region anchor
                blockAnchor_js = new
                {
                    editBtnTitle = "edit".Localize(),
                    copyBtnTitle = "copy".Localize(),
                    deleteBtnTitle = "delete".Localize(),
                    publishBtnTitle = "publish".Localize(),
                    unpublishedTip = "This item has not been published yet.<br/>Click to publish this item.".Localize()
                },

                fieldAnchor_js = new
                {
                    editBtnTitle = "edit".Localize()
                },

                htmlAnchor_js = new
                {
                    editBtnTitle = "edit".Localize()
                },

                editorAnchor_js = new
                {
                    saveBtnTitle = "save".Localize(),
                    cancelBtnTitle = "cancel".Localize(),
                    fontFamilyTitle = "font family".Localize(),
                    fontSizeTitle = "font size".Localize(),
                    fontColorTitle = "font color".Localize(),
                    backColorTitle = "background color".Localize()
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
                    bold = "bold".Localize(),
                    italic = "italic".Localize(),
                    underline = "underline".Localize(),
                    alignLeft = "align left".Localize(),
                    alignCenter = "align center".Localize(),
                    alignRight = "align right".Localize(),
                    alignJustify = "align justify".Localize(),
                    numberList = "number list".Localize(),
                    bulletList = "bullet list".Localize(),
                    indent = "increase indent".Localize(),
                    outdent = "decrease indent".Localize(),
                    insertImage = "insert image".Localize(),
                    insertLink = "insert link".Localize(),
                    editSource = "edit source".Localize(),
                    redo = "redo".Localize(),
                    undo = "undo".Localize(),
                    unformat = "remove format".Localize(),
                    horizontalRuler = "insert horizontal ruler".Localize(),
                    pastePlainText = "paste with plain text".Localize()
                },
                #endregion

                #region inline
                block_js = new
                {
                    confirmDel = "Are you sure you want to delete this item?".Localize(),
                    networkError = "Network error, the action has been cancelled.".Localize(),
                    copying = "copying...".Localize(),
                    deleting = "deleting...".Localize(),
                    publishing = "publishing...".Localize(),
                    copySuccess = "copy successfully.".Localize(),
                    deleteSuccess = "delete successfully.".Localize(),
                    publishSuccess = "publish successfully.".Localize(),
                    copyFailure = "The attempt to copy has failed.".Localize(),
                    deleteFailure = "The attempt to delete has failed.".Localize(),
                    publishFailure = "The attempt to publish has failed.".Localize()
                },

                field_js = new
                {
                    networkError = "Network error, the action has been cancelled.".Localize(),
                    saving = "saving...".Localize(),
                    saveSuccess = "save successfully.".Localize(),
                    saveFailure = "The attempt to save has failed.".Localize()
                },
                #endregion

                #region panel

                imagePanel_js = new
                {
                    title = "image options".Localize(),
                    imgLibTitle = "Image library".Localize(),
                    attrURL = "URL:".Localize(),
                    attrALT = "ALT:".Localize(),
                    attrWidth = "Width:".Localize(),
                    attrHeight = "Height:".Localize(),
                    btnOk = "save".Localize(),
                    btnCancel = "cancel".Localize(),
                    btnRemove = "remove".Localize(),
                    btnLibrary = "library".Localize(),
                    btnView = "view".Localize(),
                    attrLinkHref = "LINK:".Localize(),
                    emptyUrlMsg = "please input the url.".Localize()
                },

                linkPanel_js = new
                {
                    headTitle = "link panel".Localize(),
                    btnOk = "ok".Localize(),
                    btnCancel = "cancel".Localize(),
                    btnUnlink = "unlink".Localize(),
                    lblText = "Text:".Localize(),
                    lblUrl = "Url:".Localize(),
                    lblTitle = "Title:".Localize(),
                    lblLinkType = "Link Type:".Localize(),
                    lblNewWin = "Open in a new window".Localize(),
                    urlEmptyMsg = "please input the url.".Localize()
                },

                textPanel_js = new
                {
                    headTitle = "text panel".Localize(),
                    btnPreview = "preview".Localize(),
                    btnReset = "reset".Localize(),
                    btnOk = "ok".Localize(),
                    btnCancel = "cancel".Localize()
                }
                #endregion
            });

            // ret
            return JavaScript(string.Format("var __inlineEditVars = {0};", vars));
        }
        #endregion
    }
}
