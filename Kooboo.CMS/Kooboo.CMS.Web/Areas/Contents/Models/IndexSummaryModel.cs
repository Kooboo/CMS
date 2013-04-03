using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Search;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Search.Models;
using System.Text;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.Globalization;
namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    public class FolderRenderType : IItemColumnRender
    {
        public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            var textFolder = ((FolderIndexInfo)dataItem).Folder;
            return new HtmlString(textFolder.FriendlyName);
        }
    }

    public class RebuildingRenderType : IItemColumnRender
    {

        public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            FolderIndexInfo folderIndexInfo = (FolderIndexInfo)dataItem;
            if (folderIndexInfo.Rebuilding)
            {
                return new HtmlString(@"<span class=""o-icon load""></span>");
            }
            else
            {
                UrlHelper urlHelper = new UrlHelper(viewContext.RequestContext);
                return new HtmlString(string.Format("<a href='{0}' class='o-icon rebuild actionCommand' confirm='{1}' title='{2}'>{2}</a>"
                    , urlHelper.Action("Rebuild", "SearchIndex", viewContext.RequestContext.AllRouteValues().Merge("FolderName", folderIndexInfo.Folder.FullName))
                    , "The system will automatically index new or updated content, rebuilding all may take a long time, are you sure you want to continue?".Localize()
                    , "Rebuild index".Localize()));
            }
        }
    }

    public class FolderIndexInfo_Metadata
    {
        [GridColumn(HeaderText = "Folder", Order = 0, ItemRenderType = typeof(FolderRenderType))]
        public TextFolder Folder { get; set; }
        [GridColumn(HeaderText = "Indexed contents", Order = 1, Class = "common")]
        public int IndexedContents { get; set; }
        [GridColumn(HeaderText = "Rebuild", Class = "action", Order = 2, ItemRenderType = typeof(RebuildingRenderType))]
        public bool Rebuilding { get; set; }
    }

    public class LastAction_Metadata
    {
        public TextFolder Folder { get; set; }
        public string ContentSummary { get; set; }
        public ContentAction Action { get; set; }
    }
    public class IndexSummaryModel
    {
        public IEnumerable<FolderIndexInfo> FolderIndexInfoes { get; set; }
        public IEnumerable<LastAction> LastActions { get; set; }
    }
}