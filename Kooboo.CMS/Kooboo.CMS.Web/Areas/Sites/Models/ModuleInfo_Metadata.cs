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
using System.Web.Mvc;
using Kooboo.Globalization;
using System.ComponentModel.DataAnnotations;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Areas.Sites.Models.Grid2;
namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(ModuleInfo))]
    [Grid(IdProperty = "UUID", Checkable = true)]
    public class ModuleInfo_Metadata
    {
        [GridColumn(Order = 1, GridColumnType = typeof(SortableGridColumn))]
        [Display(Name = "Module name")]
        public string ModuleName { get; set; }

        [GridColumn(Order = 2)]
        public string Version { get; set; }

        public string KoobooCMSVersion { get; set; }
    }

    public class InstallModuleModel
    {
        [Required]
        [UIHint("File")]
        [AdditionalMetadata("accept", ".zip")]
        [Display(Name = "Select file source")]
        public string ModuleFile { get; set; }
    }

    [Grid(IdProperty = "ModuleName", Checkable = true, GridItemType = typeof(ModuleInSite_GridItem))]
    public class ModuleListInSiteModel
    {
        [Required]
        [GridColumn]
        public string ModuleName { get; set; }
        [GridColumn(GridItemColumnType = typeof(BooleanGridItemColumn))]
        public bool Included { get; set; }
    }
}