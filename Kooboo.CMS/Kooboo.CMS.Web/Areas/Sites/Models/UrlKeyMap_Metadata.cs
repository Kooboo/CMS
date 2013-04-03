using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc.Grid;
using Kooboo.Web.Mvc;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using System.Web.Routing;
using Kooboo.CMS.Web.Models;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class PagesDataSource : ISelectListDataSource
    {
        #region ISelectListDataSource Members

        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var site = Site.Current;
            var pages = Kooboo.CMS.Sites.Persistence.Providers.PageProvider.All(site);
            List<SelectListItem> list = new List<SelectListItem> { 
                new SelectListItem(){Text="",Value=""}
            };

            foreach (var item in pages)
            {
                GetPageItems(item, list);
            }

            return list;
        }
        private void GetPageItems(Page page, List<SelectListItem> items)
        {
            items.Add(new SelectListItem() { Text = page.FriendlyName, Value = page.FriendlyName });
            foreach (var child in Kooboo.CMS.Sites.Persistence.Providers.PageProvider.ChildPages(page))
            {
                GetPageItems(child, items);
            }
        }

        #endregion
    }
    [GridAction(ActionName = "Edit", RouteValueProperties = "Name=Key", Order = 1, Class = "o-icon edit dialog-link", Title = "Edit")]
    //[GridAction(ActionName = "Delete", ConfirmMessage = "Are you sure you want to delete this item?", RouteValueProperties = "Name=Key", Order = 3)]
    [Grid(Checkable = true, IdProperty = "Key")]
    public class UrlKeyMap_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [Remote("IsKeyAvailable", "UrlKeyMap", AdditionalFields = "SiteName,old_Key")]
        [GridColumn(Order = 1, ItemRenderType = typeof(CommonLinkPopColumnRender))]
        public string Key { get; set; }

        [Required(ErrorMessage = "Required")]
        [GridColumn(Order = 1)]
        [UIHintAttribute("DropDownList")]//tree dropdownlist
        [DataSource(typeof(PagesDataSource))]
        public string PageFullName { get; set; }
    }
}