using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
namespace Kooboo.CMS.ModuleArea.Models
{
    public class TitleColumnRender : IItemColumnRender
    {
        public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            var news = (News)dataItem;
            UrlHelper url = new UrlHelper(viewContext.RequestContext);

            return new HtmlString(string.Format(@"<a href=""{0}"">{1}</a>", url.Action("Edit", viewContext.RequestContext.AllRouteValues().Merge("Id", news.Id)), value));

        }
    }
    [GridAction(DisplayName = "Edit", ActionName = "Edit", RouteValueProperties = "Id", Order = 1, Class = "o-icon edit", Title = "Edit")]
    [Grid(Checkable = true, IdProperty = "Id")]
    public class News
    {
        [GridColumnAttribute()]
        public int Id { get; set; }
        [Required]
        [GridColumnAttribute(ItemRenderType = typeof(TitleColumnRender))]
        public string Title { get; set; }
        [DataType("Tinymce")]
        public string Body { get; set; }
    }
}