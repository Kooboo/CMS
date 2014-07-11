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
using Kooboo.CMS.Sites.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Common.ComponentModel;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.Common.Web.Grid.Design;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Areas.Sites.Models.Grid2;
using Kooboo.Common.Web.Metadata;
using Kooboo.Common.Misc;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(View))]
    [Grid(Checkable = true, IdProperty = "UUID", GridItemType = typeof(InheritablGridItem))]
    [GridColumn(GridItemColumnType = typeof(Inheritable_Status_GridItemColumn), HeaderText = "Inheritance", Order = 2)]
    public class View_Metadata
    {
        [GridColumn(Order = 1, GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(InheritableEditGridActionItemColumn))]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [Remote("IsNameAvailable", "view", AdditionalFields = "SiteName,old_Key")]
        public string Name { get; set; }

        [UIHintAttribute("TemplateEditor")]
        public string Body { get; set; }

        [GridColumn(Order = 3, GridColumnType = typeof(SortableGridColumn))]
        public Site Site { get; set; }

        [UIHint("Plugins")]
        [DataSource(typeof(PluginsDataSource))]
        public List<string> Plugins { get; set; }

        [UIHint("Parameters")]
        public List<Parameter> Parameters { get; set; }

        [UIHint("Plugins")]
        [DataSource(typeof(DataSourceSetting_DataSource))]
        public List<string> DataSources { get; set; }
    }
}