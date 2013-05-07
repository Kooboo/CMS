#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [Grid(Checkable = true, IdProperty = "UUID")]
    [MetadataFor(typeof(SubmissionSetting))]
    public class SubmissionSetting_Metadata
    {
        [GridColumnAttribute(HeaderText = "Name", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn), Order = 1)]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [UIHint("DropdownList")]
        [DataSource(typeof(DataSources.SubmissionPluginsDataSource))]
        [Required(ErrorMessage = "Required")]
        public string PluginType { get; set; }
        [UIHint("Dictionary")]
        [Description("The parameter settings for the plugin. e.g: FolderName : News")]
        public Dictionary<string, string> Settings { get; set; }
    }
}
