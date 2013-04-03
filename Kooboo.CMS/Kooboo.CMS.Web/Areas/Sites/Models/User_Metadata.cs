using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Account.Services;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Extensions;
using Kooboo.CMS.Web.Models;
using System.ComponentModel;
namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class RoleRender : IItemColumnRender
    {
        public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            if (value != null)
            {
                return new HtmlString(string.Join(",", ((IEnumerable<string>)value).ToArray()));
            }
            return new HtmlString("");
        }
    }
    public class UsersDatasource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var users = Kooboo.CMS.Account.Services.ServiceFactory.UserManager.All();
            if (!string.IsNullOrEmpty(filter))
            {
                users = users.Where(it => it.UserName.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase));
            }
            return users.Select(it => new SelectListItem() { Text = it.UserName, Value = it.UserName });
        }
    }
    public class RolesDatasource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var roles = Kooboo.CMS.Account.Services.ServiceFactory.RoleManager.All();
            return roles.Select(it => new SelectListItem() { Text = it.Name, Value = it.Name });
        }
    }
    [GridAction(ActionName = "Edit", RouteValueProperties = "Name=UserName", Order = 1, Title = "Edit user", Class = "o-icon edit dialog-link")]
    //[GridAction(ActionName = "Delete", Order = 3, RouteValueProperties = "Name=UserName", ConfirmMessage = "Are you sure you want to delete this item?")]
    [Grid(Checkable = true, IdProperty = "UserName")]
    public class User_Metadata
    {
        [DisplayName("User name")]
        [Description("Add an user to your website <br />The user must be created first under Users menu")]
        [Required(ErrorMessage = "Required")]
        [GridColumn(Order = 1, ItemRenderType = typeof(CommonLinkPopColumnRender))]
        [UIHint("DropDownList")]
        [DataSource(typeof(UsersDatasource))]
        [RemoteEx("IsUserAvailable", "Users", RouteFields = "siteName", AdditionalFields = "old_Key")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required")]
        [UIHint("DropDownArray")]
        [DataSource(typeof(RolesDatasource))]
        [GridColumn(Order = 2, ItemRenderType = typeof(RoleRender))]
        public List<string> Roles { get; set; }
    }
}