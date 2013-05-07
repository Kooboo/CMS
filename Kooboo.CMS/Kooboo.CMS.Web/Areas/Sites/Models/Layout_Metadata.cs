#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.CMS.Web.Areas.Sites.Models.Grid2;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(Layout))]
    [Grid(Checkable = true, IdProperty = "UUID", GridItemType = typeof(InheritablGridItem))]
    public class Layout_Metadata
    {
        [GridColumn(Order = 1, GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(InheritableEditGridActionItemColumn))]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [Remote("IsNameAvailable", "layout", AdditionalFields = "SiteName,old_Key")]
        public string Name { get; set; }

        [GridColumn(Order = 2, GridColumnType = typeof(SortableGridColumn))]
        public Site Site { get; set; }

        [UIHintAttribute("TemplateEditor")]
        public string Body { get; set; }

        [UIHint("Plugins")]
        [DataSource(typeof(PluginsDataSource))]
        public List<string> Plugins { get; set; }
    }
}