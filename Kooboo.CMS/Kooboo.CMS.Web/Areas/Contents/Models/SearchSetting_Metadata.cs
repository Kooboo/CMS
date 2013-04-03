using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Search.Models;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Web.Models;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Content.Models;
using System.ComponentModel;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    [Grid(Checkable = true, IdProperty = "Name")]
    [GridAction(ActionName = "Edit", Class = "o-icon edit dialog-link", RouteValueProperties = "Name")]
    public class SearchSetting_Metadata
    {
        public string Name { get; set; }

        [Required]
        [GridColumn(ItemRenderType = typeof(CommonLinkPopColumnRender), Order = 0)]
        [UIHint("SingleFolderTree")]
        [DataSource(typeof(FolderTreeDataSource))]
        [RemoteEx("IsNameAvailable", "SearchSetting", RouteFields = "RepositoryName")]
        [Display(Name = "Folder name")]
        public string FolderName { get; set; }

        [Display(Name = "Include all fields")]
        [Description("Include all content fields in the search index or select special fields.")]
        public bool IncludeAllFields { get; set; }
        [UIHint("DropDownArray")]
        public List<string> Fields { get; set; }

        [GridColumn(Order = 1)]
        [Display(Name="Content Url format")]
        [Description("The URL format with parameters of content detail page. e.g.: /article/detail/{userkey}")]
        public string UrlFormat { get; set; }

        public Repository Repository { get; set; }
    }
}