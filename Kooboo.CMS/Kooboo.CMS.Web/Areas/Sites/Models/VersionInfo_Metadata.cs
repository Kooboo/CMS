using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Web.Models;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [Grid]
    [GridAction(ActionName = "PreviewVersion", DisplayName = "Preview", RouteValueProperties = "Version", Order = 1, Class = "o-icon preview dialog-link")]
    [GridAction(ActionName = "Revert", DisplayName = "Revert", RouteValueProperties = "Version", Order = 1, Class = "o-icon revert ajax-post-link",ConfirmMessage="Are you sure you want to rever to this version?")]
    public class VersionInfo_Metadata
    {
        [GridColumn()]
        public int Version { get; set; }
        [GridColumn(ItemRenderType = typeof(DateTimeColumnRender))]
        public DateTime Date { get; set; }
        [GridColumn()]
        public string UserName { get; set; }
    }
}