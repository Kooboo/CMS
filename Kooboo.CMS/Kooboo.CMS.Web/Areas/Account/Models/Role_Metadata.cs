using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Web.Models;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Account.Services;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Account.Models
{
    [GridAction(ActionName = "Edit", RouteValueProperties = "Name", Order = 1, Class = "o-icon edit dialog-link")]
    [Grid(Checkable = true, IdProperty = "Name")]
    public class Role_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [GridColumn(Order = 1, ItemRenderType = typeof(CommonLinkPopColumnRender))]
        public string Name { get; set; }
    }

    public class RoleDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            return ServiceFactory
                .RoleManager
                .All()
                .Select(o => new SelectListItem
                {
                    Text = o.Name,
                    Value = o.Name
                });
        }

        public static RoleDataSource DataSource()
        {
            return new RoleDataSource();
        }
    }

}