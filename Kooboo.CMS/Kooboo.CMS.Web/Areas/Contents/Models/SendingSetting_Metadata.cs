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
using Kooboo.CMS.Content.Models;

using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Web.Models;
using Kooboo.Globalization;
using System.Web.Mvc;
using System.ComponentModel;
using Kooboo.ComponentModel;
using Kooboo.CMS.Web.Areas.Contents.Models.DataSources;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.CMS.Web.Areas.Contents.Models.Grid2;
using Kooboo.CMS.Web.Grid2;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    [MetadataFor(typeof(SendingSetting))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class SendingSetting_Metadata
    {
        public Repository Repository { get; set; }

        public string Name { get; set; }

        [GridColumn(Order = 1, HeaderText = "Folder name", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]
        [UIHint("MultiFolderTree")]
        [Display(Name = "Folder name")]
        //[RemoteEx("IsNameAvailable", "*", RouteFields = "RepositoryName")]
        public string FolderName { get; set; }

        [GridColumn(Order = 2, HeaderText = "Send received", GridItemColumnType = typeof(BooleanGridItemColumn))]
        [Display(Name = "Send recieved content")]
        [Description("Broadcasting contents that received from other websites.")]
        [UIHint("BooleanEx")]
        public bool? SendReceived { get; set; }

        [GridColumn(Order = 3, HeaderText = "Send to sub sites", GridItemColumnType = typeof(BooleanGridItemColumn))]
        [DisplayName("Send to sub sites")]
        [Description("broadcast the content to the selected child websites.")]
        public bool? SendToChildSites { get; set; }

        [UIHint("RadioButtonList")]
        [EnumDataTypeAttribute(typeof(ChildLevel))]
        [DisplayName("Child level")]
        [Description("Only send to first level child sites or all level child sites.")]
        public ChildLevel ChildLevel { get; set; }

        [Display(Name = "Keep content status")]
        public bool KeepStatus { get; set; }
    }
}