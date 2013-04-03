using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using System.Web.Mvc;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace Kooboo.CMS.Web.Areas.Account.Models
{
    public class UserNameRender : IItemColumnRender
    {

        public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {

            UrlHelper url = new UrlHelper(viewContext.RequestContext);

            return new HtmlString(string.Format(@"<a href=""{0}"" class=""dialog-link"" title=""{1}"">{2}</a>", url.Action("Edit", viewContext.RequestContext.AllRouteValues().Merge("UserName", value)), "Edit".Localize(), value));
        }
    }
    [GridAction(DisplayName = "Reset password", Title = "Reset password", ActionName = "ResetPassword", RouteValueProperties = "UserName", Order = 1, Class = "o-icon password dialog-link")]
    [GridAction(Title = "Edit user", ActionName = "Edit", RouteValueProperties = "UserName", Order = 2, Class = "o-icon edit dialog-link")]
    [Grid(Checkable = true, IdProperty = "UserName")]
    public class User_Metadata
    {
        [GridColumn(Order = 1, ItemRenderType = typeof(UserNameRender))]
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        [GridColumn(Order = 2)]
        public string Email { get; set; }

        public string Password { get; set; }

        [GridColumn(Order = 3)]
        public bool IsAdministrator { get; set; }
        [GridColumn(Order = 4)]
        public bool IsLockedOut { get; set; }
        [UIHintAttribute("DropDownList")]
        [DataSource(typeof(CultureSelectListDataSource))]
        [Description("The culture represents the current culture used by the Resource Manager to look up culture-specific resources at run time.")]
        public string UICulture { get; set; }

    }

    public class PasswordModel
    {
        [UIHint("PasswordStrength")]
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Re-enter password")]
        [Required(ErrorMessage = "Required")]
        [Compare("NewPassword")]
        public string NewPassword1 { get; set; }
    }

    public class ChangePasswordModel : PasswordModel
    {
        [Display(Name = "Old password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required")]
        public string OldPassword { get; set; }
    }

    public class ResetPasswordModel : PasswordModel
    {
        public string UserName { get; set; }
    }

    public class ForgotPasswordModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class ResetPasswordByTokenModel : PasswordModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Token { get; set; }
    }
}