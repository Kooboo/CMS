#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Kooboo.ComponentModel;
using Kooboo.CMS.Account.Models;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.CMS.Web.Grid2;
using System.Web.Mvc;
using Kooboo.CMS.Web.Areas.Account.Models.DataSources;
namespace Kooboo.CMS.Web.Areas.Account.Models
{

    [MetadataFor(typeof(User))]
    [Grid(Checkable = true, IdProperty = "UserName")]
    public class User_Metadata
    {
        [GridColumn(Order = 1, GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]
        public string UserName { get; set; }

        [GridColumn(Order = 2, GridColumnType = typeof(SortableGridColumn))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Password { get; set; }

        [GridColumn(Order = 4, HeaderText = "Is administrator", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(BooleanGridItemColumn))]
        public bool IsAdministrator { get; set; }

        [GridColumn(Order = 5, HeaderText = "Is locked out", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(BooleanGridItemColumn))]
        public bool IsLockedOut { get; set; }

        [UIHintAttribute("DropDownList")]
        [DataSource(typeof(CultureSelectListDataSource))]
        [Description("The culture represents the current culture used by the Resource Manager to look up culture-specific resources at run time.")]
        public string UICulture { get; set; }

        [GridColumn(HeaderText = "Global roles", Order = 6)]     
        public string GlobalRoles { get; set; }
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