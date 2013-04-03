using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Models;
using System.Web.Mvc;
using Kooboo.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{

    [GridAction(ActionName = "Copy", Class = "common-copy o-icon copy", Order = 1, RouteValueProperties = "sourceName=Name")]
    [GridAction(ActionName = "Relations", RouteValueProperties = "Name", Order = 2, CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter), Class = "o-icon relation dialog-link", Title = "Relations")]
    [GridAction(DisplayName = "Localize", ActionName = "Localize", ConfirmMessage = "Are you sure you want to localize this item?", RouteValueProperties = "Name", Order = 3, Class = "o-icon localize", Renderer = typeof(LocalizationRender))]
    [GridAction(ActionName = "Edit", RouteValueProperties = "Name", Order = 4, CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter), Class = "o-icon edit")]
    [GridAction(DisplayName = "Version", ActionName = "Version", RouteValueProperties = "Name", Order = 5, ColumnVisibleArbiter = typeof(VersionGridActionVisibleArbiter), Class = "o-icon version dialog-link")]
    [Grid(Checkable = true, IdProperty = "Name", CheckVisible = typeof(InheritableCheckVisible))]
    public class Layout_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [Remote("IsNameAvailable", "layout", AdditionalFields = "SiteName,old_Key")]
        [GridColumn(Order = 1, ItemRenderType = typeof(CommonLinkNameColumnRender))]
        public string Name { get; set; }
        [GridColumn(Order = 8)]
        public Site Site { get; set; }
        [UIHintAttribute("TemplateEditor")]
        public string Body { get; set; }

        [UIHint("Plugins")]
        [DataSource(typeof(PluginDataSource))]
        public List<string> Plugins { get; set; }
    }
}