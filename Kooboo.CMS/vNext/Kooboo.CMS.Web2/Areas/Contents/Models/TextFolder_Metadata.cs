#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Web2.Areas.Contents.Models.DataSources;
using Kooboo.CMS.Web2.Areas.Contents.Models.Grid2;
using Kooboo.Common.ComponentModel;
using Kooboo.Common.Globalization;
using Kooboo.Common.Misc;
using Kooboo.Common.Web.Grid.Design;
using Kooboo.Common.Web.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web2.Areas.Contents.Models
{
    [MetadataFor(typeof(TextFolder))]
    [Grid(Checkable = true, IdProperty = "UUID", GridItemType = typeof(TextFolder_GridItem))]
    public class TextFolder_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [GridColumn(GridItemColumnType = typeof(TextFolder_Name_GridItemColumn))]
        public string Name { get; set; }

        [DisplayName("Display name")]
        public string DisplayName { get; set; }
        //public BroadcastSetting BroadcastSetting { get; set; }

        [UIHint("DropDownList")]
        [DataSource(typeof(SchemaListDataSource))]
        [DisplayName("Content type")]
        public string SchemaName { get; set; }

        [Display(Name = "Category folders")]
        [UIHint("Categories")]
        [DataSource(typeof(TextFoldersDataSource))]
        public IEnumerable<CategoryFolder> Categories { get; set; }

        [Display(Name = "Embedded folders")]
        [Description("Embed content from another content folder. For example an <b>article</b> embeds <b>comments</b>")]
        [UIHint("Multiple_DropDownList")]
        [DataSource(typeof(TextFoldersDataSource))]
        public string[] EmbeddedFolders { get; set; }

        [Display(Name = "Workflow name")]
        [UIHint("DropDownList")]
        [DataSource(typeof(WorkflowsDataSource))]
        public string WorkflowName { get; set; }

        [UIHint("Multiple_DropDownList")]
        [DataSource(typeof(Kooboo.CMS.Web2.Areas.Contents.Models.DataSources.RolesDatasource))]
        public string[] Roles { get; set; }

        [Display(Name = "Enable worflow")]
        public bool EnabledWorkflow { get; set; }

        [UIHint("OrderSetting")]
        public OrderSetting OrderSetting { get; set; }

        [DisplayName("Hide the folder")]
        [Description("Hide the folder from the menu in the left slide bar when it was checked.")]
        public bool? Hidden
        {
            get;
            set;
        }
        [DisplayName("Page size")]
        [Description("The item numbers on management grid page.")]
        public int? PageSize { get; set; }

        [Display(Name = "Sortable")]
        public bool? Sortable
        {
            get;
            set;
        }


        [Display(Name = "Enable paging")]
        public bool? EnablePaging
        {
            get;
            set;
        }
    }

    [MetadataFor(typeof(OrderSetting))]
    public class OrderSetting_Metadata
    {
        [Display(Name = "Order field")]
        [UIHint("Hidden")]
        public string FieldName { get; set; }
        [Display(Name = "Order direction")]
        [UIHint("DropDownList")]
        [DataSource(typeof(OrderDirectionsDataSource))]
        public OrderDirection Direction { get; set; }
    }
}