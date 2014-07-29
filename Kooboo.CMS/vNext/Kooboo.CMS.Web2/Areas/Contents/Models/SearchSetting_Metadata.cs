#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Web2.Models;
using System.ComponentModel.DataAnnotations;

using Kooboo.CMS.Content.Models;
using System.ComponentModel;
using Kooboo.Common.ComponentModel;
using Kooboo.CMS.Web2.Areas.Contents.Models.DataSources;
using Kooboo.Common.Web.Grid.Design;
using Kooboo.CMS.Web2.Grid2;
using System.Collections.Generic;
using Kooboo.Common.Web.Metadata;

namespace Kooboo.CMS.Web2.Areas.Contents.Models
{
    [MetadataFor(typeof(SearchSetting))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class SearchSetting_Metadata
    {
        public string Name { get; set; }

        [GridColumn(Order = 1, HeaderText = "Folder name", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]
        [Required]
        [UIHint("SingleFolderTree")]
        //[RemoteEx("IsNameAvailable", "SearchSetting", RouteFields = "RepositoryName")]
        [Display(Name = "Folder name")]
        public string FolderName { get; set; }

        [GridColumn(Order = 2, HeaderText = "Link page", GridColumnType = typeof(SortableGridColumn))]
        [Display(Name = "Link page")]
        [Description("The detail page to dispaly the search result item.")]
        [UIHint("DropDownList")]
        //todo:restore
        //[DataSource(typeof(Kooboo.CMS.Web2.Areas.Sites.Models.DataSources.PagesDataSource))] 
        public string LinkPage { get; set; }

        [UIHint("Dictionary")]
        [Display(Name = "Route fields")]
        [Description("The fields in search result items that will be appended to the detail page route values. Use {key} to match the value in the search result collection. ")]
        public Dictionary<string, string> RouteValueFields
        {
            get;
            set;
        }
    }
}