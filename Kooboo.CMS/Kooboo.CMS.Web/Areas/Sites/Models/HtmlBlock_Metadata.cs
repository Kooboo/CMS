using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Models;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class HtmlBlockNameColumnRender : IItemColumnRender
    {
        public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            var htmlBlock = ((HtmlBlock)dataItem).AsActual();

            UrlHelper url = new UrlHelper(viewContext.RequestContext);

            if (htmlBlock.IsLocalized(Site.Current))
            {
                return new HtmlString(string.Format(@"<a href=""{0}"" class=""dialog-link"" title=""{2}"">{1}</a>",
                    url.Action("Edit", viewContext.RequestContext.AllRouteValues().Merge("Name", dataItem).
                    Merge("ReturnUrl", viewContext.RequestContext.HttpContext.Request.RawUrl)), value, "Edit".Localize()));
            }
            else
            {
                return new HtmlString(dataItem.ToString());
            }

        }
    }
    [Grid(Checkable = true, IdProperty = "Name", CheckVisible = typeof(InheritableCheckVisible))]
    [GridAction(ActionName = "Edit", Class = "o-icon edit dialog-link", RouteValueProperties = "Name", Order = 3, CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter))]
    [GridAction(DisplayName = "Localize", ActionName = "Localize", Class = "o-icon localize", ConfirmMessage = "Are you sure you want to localize this item?",
        RouteValueProperties = "Name", Order = 5, Renderer = typeof(LocalizationRender))]
    [GridAction(DisplayName = "Version", ActionName = "Version", RouteValueProperties = "Name", Order = 7, ColumnVisibleArbiter = typeof(VersionGridActionVisibleArbiter), Class = "o-icon version dialog-link")]
    public class HtmlBlock_Metadata
    {
        [GridColumn(Order = 1, ItemRenderType = typeof(HtmlBlockNameColumnRender))]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [RemoteEx("IsNameAvailable", "HtmlBlock", AdditionalFields = "SiteName,old_Key")]
        public string Name { get; set; }
        [UIHint("Tinymce")]
        public string Body { get; set; }
    }
}