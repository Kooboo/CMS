using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Sites.Models;

using Kooboo.Web.Mvc;

using System.Web.Mvc.Html;

using Kooboo.Globalization;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    //[GridAction(ActionName = "Delete",
    //    ConfirmMessage = "Are you sure you want to delete this file?",
    //    Order = 3,
    //    RouteValueProperties = "FileName",
    //    VisibleArbiter = typeof(InheritableGridActionVisibleArbiter)
    //    )]
    public class ScriptNameRender : IItemColumnRender
    {

        public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {

            //string str = string.Format(@"<a class=""f-icon file js dialog-link"" href=""javascript:;"" title=""{1}"">{0}</a>", script.Name, "Edit".Localize());
            //return new HtmlString(str);
            var script = (ScriptFile)dataItem;
            if (script.IsLocalized(Site.Current))
            {
                UrlHelper url = new UrlHelper(viewContext.RequestContext);

                return new HtmlString(string.Format(@"<a href=""{0}"" class=""f-icon file js dialog-link"" title=""{1}"">{2}</a>", url.Action("Edit", viewContext.RequestContext.AllRouteValues().Merge("FileName", ((ScriptFile)dataItem).FileName)), "Edit".Localize(), value));
            }
            else
            {
                return new HtmlString(string.Format("{0}", value));
            }
        }
    }
    [GridAction(ActionName = "Edit",
        Order = 0,
        RouteValueProperties = "FileName",
        CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter)
        , Class = "o-icon edit dialog-link", Title = "Edit")]
    [GridAction(DisplayName = "Localize", ActionName = "Localize", ConfirmMessage = "Are you sure you want to localize this item?",
       RouteValueProperties = "FileName", Order = 7, Class = "o-icon localize", Renderer = typeof(LocalizationRender))]
    [Grid(Checkable = true, IdProperty = "FileName", CheckVisible = typeof(InheritableCheckVisible))]
    public class ScriptFile_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [Remote("IsNameAvailable", "Script", AdditionalFields = "SiteName,FileExtension,old_Key")]
        public string Name { get; set; }

        [GridColumn(ItemRenderType = typeof(ScriptNameRender))]
        public string FileName { get; set; }

        [AllowHtml]
        [UIHintAttribute("TemplateEditor")]
        public string Body { get; set; }
    }
}