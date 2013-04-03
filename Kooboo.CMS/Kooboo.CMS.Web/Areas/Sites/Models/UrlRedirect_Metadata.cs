using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Sites.Models;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Web.Models;
using System.Web.Mvc;
using System.ComponentModel;


namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [GridAction(ActionName = "Edit", RouteValueProperties = "Name=InputUrl", Order = 1, Class = "o-icon edit dialog-link", Title = "Edit")]
    //[GridAction(ActionName = "Delete", ConfirmMessage = "Are you sure you want to delete this item?", RouteValueProperties = "Name", Order = 3)]
    [Grid(Checkable = true, IdProperty = "InputUrl")]
    public class UrlRedirect_Metadata
    {
        public UrlRedirect_Metadata()
        {
            UrlRedirect map = new UrlRedirect();
        }

        [Description("Regular expression to match your URL, for example: oldpage_(\\w+)")]
        [GridColumn(ItemRenderType = typeof(CommonLinkPopColumnRender))]
        [Required(ErrorMessage = "Required")]
        [Remote("IsInputUrlAvailable", "UrlRedirect", AdditionalFields = "SiteName,old_Key")]
        [Display(Name = "Input URL")]
        public string InputUrl { get; set; }

        [GridColumn]
        [Required(ErrorMessage = "Required")]
        [Description("Redirect URL based on regular expression and match values from input URL pattern, for example: newpage_$1")]
        [Display(Name = "Output URL")]
        public string OutputUrl { get; set; }

        [Description("Use either normal text matching or regular expression")]
        [DisplayName("Regular expression")]
        [GridColumn(ItemRenderType = typeof(BooleanColumnRender))]
        [UIHint("RegexOption")]
        public bool Regex { get; set; }

        [GridColumn]
        [UIHint("DropDownList")]
        [EnumDataType(typeof(RedirectType))]
        [Display(Name = "Redirect type")]
        public RedirectType RedirectType { get; set; }

    }
}